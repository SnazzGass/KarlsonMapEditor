using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using System;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;
using static KarlsonMapEditor.CustomDescriptors;
using KarlsonMapEditor.Scripting_API;
using System.Linq;
using System.Collections.Generic;

namespace KarlsonMapEditor
{
    public class LuaScriptRunner : MonoBehaviour
    {
        public static readonly string ScriptPath = Path.Combine(Main.directory, "_temp.lua");

        public const string DefaultCode =
@"-- Any code written below will be executed every time the level is (re)started.
-- Lua documentation: https://www.lua.org/pil/contents.html
-- Moonsharp Lua Differences: https://www.moonsharp.org/moonluadifferences.html
-- Scripting API: https://github.com/SnazzGass/KarlsonMapEditor/wiki/Overview

print('Hello, world!');
";

        public string Code;

        private bool running = false;
        public List<PlayerMovement> players;

        public readonly Script script = new Script(CoreModules.Preset_SoftSandbox);

        private void Awake()
        {
            script.Options.DebugPrint = LuaDebug;
            RegisterTypes();
            script.Globals["Players"] = players;
        }
        // logging
        private void LuaDebug(string msg)
        {
            Loadson.Console.Log("[Lua] " + msg);
        }

        private StandardUserDataDescriptor RegisterWithConstructor<T>(bool newConstructor=true)
        {
            StandardUserDataDescriptor descriptor = (StandardUserDataDescriptor)UserData.RegisterType<T>();
            
            // remove the default constructors
            descriptor.RemoveMember("__new");

            // add the desired constructors
            if (newConstructor)
                foreach (ConstructorInfo constructor in typeof(T).GetConstructors())
                    descriptor.AddMember("new", new MethodMemberDescriptor(constructor));

            return descriptor;
        }

        private IUserDataDescriptor RegisterDestructableProxyType<TProxy, TTarget>(Func<TTarget, TProxy> wrapDelegate, bool constructor) where TProxy : class where TTarget : class
        {
            StandardUserDataDescriptor proxyDescriptor = RegisterWithConstructor<TProxy>(constructor);
            DestructableProxyUserDataDescriptor targetDescriptor = new DestructableProxyUserDataDescriptor(new DelegateProxyFactory<TProxy, TTarget>(wrapDelegate), proxyDescriptor);
            return UserData.RegisterType<TTarget>(targetDescriptor);
        }

        // interface
        private void RegisterTypes()
        {
            // KME objects
            RegisterWithConstructor<LevelData.LevelObject>();
            script.Globals["LevelObjectData"] = UserData.CreateStatic<LevelData.LevelObject>();
            RegisterWithConstructor<Trigger>(false);
            RegisterDestructableProxyType<PlayerProxy, PlayerMovement>(o => new PlayerProxy(o), false);

            // unity structs
            RegisterWithConstructor<Vector2>();
            script.Globals["Vector2"] = UserData.CreateStatic<Vector2>();
            RegisterWithConstructor<Vector3>();
            script.Globals["Vector3"] = UserData.CreateStatic<Vector3>();
            RegisterWithConstructor<Quaternion>();
            script.Globals["Quaternion"] = UserData.CreateStatic<Quaternion>();
            RegisterWithConstructor<Color>();
            script.Globals["Color"] = UserData.CreateStatic<Color>();

            // unity objects
            RegisterDestructableProxyType<ObjectProxy, UnityEngine.Object>(o => new ObjectProxy(o), true);
            script.Globals["Object"] = UserData.CreateStatic<UnityEngine.Object>();
            RegisterDestructableProxyType<GameObjectProxy, GameObject>(o => new GameObjectProxy(o), true);
            script.Globals["GameObject"] = UserData.CreateStatic<GameObject>();
            RegisterDestructableProxyType<ComponentProxy, Component>(o => new ComponentProxy(o), false);
            RegisterDestructableProxyType<TransformProxy, Transform>(o => new TransformProxy(o), false);

            // UserData.RegisterType<Texture>(InteropAccessMode.HideMembers);
            RegisterDestructableProxyType<MaterialProxy, Material>(o => new MaterialProxy(o), true);
            script.Globals["Material"] = UserData.CreateStatic<Material>();
            RegisterDestructableProxyType<TextMeshProProxy, TextMeshPro>(o => new TextMeshProProxy(o), false);
            RegisterDestructableProxyType<LightProxy, Light>(o => new LightProxy(o), false);
            RegisterDestructableProxyType<RigidbodyProxy, Rigidbody>(o => new RigidbodyProxy(o), false);
            RegisterDestructableProxyType<ColliderProxy, Collider>(o => new ColliderProxy(o), false);
            RegisterDestructableProxyType<PhysicMaterialProxy, PhysicMaterial>(o => new PhysicMaterialProxy(o), true);

            // methods
            script.Globals["Find"] = (Func<string, GameObject>)GameObject.Find;
            script.Globals["FindAll"] = (Func<string, GameObject[]>)FindAll;
            script.Globals["Destroy"] = (Action<Object>)Object.Destroy;
            script.Globals["Instantiate"] = (Func<Object, Object>)Object.Instantiate;
            script.Globals["IsPlayer"] = (Func<GameObject, bool>) delegate(GameObject go) { return go.GetComponent<PlayerMovement>() != null; };
            script.Globals["Raycast"] = (Func<Vector3, Vector3, float, int, Table>)Raycast;
            script.Globals["CreateExplosion"] = (Action<Vector3>) delegate (Vector3 pos) { Instantiate(PrefabManager.Instance.explosion, pos, Quaternion.identity); };
            // TODO: set gun on enemy

            // enums

            // LevelObjectData
            UserData.RegisterType<ObjectType>();
            script.Globals["LevelObjectDataType"] = typeof(ObjectType);
            UserData.RegisterType<PrefabType>();
            script.Globals["Prefab"] = typeof(PrefabType);
            UserData.RegisterType<GeometryShape>();
            script.Globals["Geometry"] = typeof(GeometryShape);

            // PhysicsMaterial
            UserData.RegisterType<PhysicMaterialCombine>();
            script.Globals["PhysicMaterialCombine"] = typeof(PhysicMaterialCombine);

            // Rigidbody
            UserData.RegisterType<RigidbodyConstraints>();
            script.Globals["RigidbodyConstraints"] = typeof(RigidbodyConstraints);
            UserData.RegisterType<CollisionDetectionMode>();
            script.Globals["CollisionDetectionMode"] = typeof(CollisionDetectionMode);
            UserData.RegisterType<RigidbodyInterpolation>();
            script.Globals["RigidbodyInterpolation"] = typeof(RigidbodyInterpolation);

            // Layers
            UserData.RegisterType<KarlsonLayerBit>();
            script.Globals["Layer"] = typeof(KarlsonLayerBit);
        }

        // callbacks
        private DynValue UpdateFunc;
        private DynValue FixedUpdateFunc;

        // init the lua script, this should be done after the level has been loaded
        public void LuaStart(GameObject Root, GameObject Sun, Action BakeReflections)
        {
            // set up level specific globals
            script.Globals["Root"] = Root;
            script.Globals["Sun"] = Sun;
            script.Globals["BakeReflections"] = BakeReflections;

            // run the lua code
            script.DoString(Code);
            running = true;

            // extract callbacks
            UpdateFunc = script.Globals.Get("Update");
            FixedUpdateFunc = script.Globals.Get("FixedUpdate");
        }
        public void LuaStop()
        {
            running = false;
        }

        // callbacks
        private void Update()
        {
            if (running && UpdateFunc.Type == DataType.Function)
                script.Call(UpdateFunc, Time.deltaTime);
        }
        private void FixedUpdate()
        {
            if (running && FixedUpdateFunc.Type == DataType.Function)
                script.Call(FixedUpdateFunc, Time.fixedDeltaTime);
        }

        // Default, Player, Ground, Object, Enemy, Gun, Glass
        private const int defaultLayerMask = 0b100011011100000001;
        private Table Raycast(Vector3 origin, Vector3 direction, float maxDistance=Mathf.Infinity, int layerMask=defaultLayerMask)
        {
            if (Physics.Raycast(origin, direction, out RaycastHit info, maxDistance, layerMask))
            {
                Table result = new Table(script);
                result["point"] = info.point;
                result["distance"] = info.distance;
                result["collider"] = info.collider;
                result["rigidbody"] = info.rigidbody;
                result["gameObject"] = info.transform.gameObject;
                result["normal"] = info.normal;
                result["textureCoord"] = info.textureCoord;
                return result;
            }
            return null;
        }

        // returns an array of all game objects with the given name
        private GameObject[] FindAll(string name)
        {
            return GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name == name).ToArray();
        }

        private enum KarlsonLayerBit
        {
            Default = 1 << 0,
            TransparentFX = 1 << 1,
            IgnoreRaycast = 1 << 2,
            Water = 1 << 4,
            UI = 1 << 5,
            Player = 1 << 8,
            Ground = 1 << 9,
            Object = 1 << 10,
            PP = 1 << 11,
            Enemy = 1 << 12,
            Gun = 1 << 13,
            Bullet = 1 << 14,
            Equipable = 1 << 15,
            CollideWithGroundOnly = 1 << 16,
            Glass = 1 << 17,
        }
    }
}
