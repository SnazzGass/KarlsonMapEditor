using MoonSharp.Interpreter;
using System;
using TMPro;
using UnityEngine;

namespace KarlsonMapEditor.Scripting_API
{
    public class ObjectProxy
    {
        [MoonSharpHidden]
        public UnityEngine.Object target;
        [MoonSharpHidden]
        public bool IsValid => target != null;

        [MoonSharpHidden]
        public ObjectProxy(UnityEngine.Object instance)
        {
            target = instance;
        }
        public ObjectProxy()
        {
            target = new UnityEngine.Object();
        }

        // static methods
        //public static Action<UnityEngine.Object> Destroy = UnityEngine.Object.Destroy;
        //public static Func<UnityEngine.Object, UnityEngine.Object> Instantiate = UnityEngine.Object.Instantiate;
        public UnityEngine.Object FindObjectOfType(string type)
        { return UnityEngine.Object.FindObjectOfType(Type.GetType(type)); }
        public UnityEngine.Object[] FindObjectsOfType(string type)
        { return UnityEngine.Object.FindObjectsOfType(Type.GetType(type)); }

        // instance methods
        public override string ToString()
        {
            return target.ToString();
        }

        public override bool Equals(object obj)
        {
            return target.Equals(obj);
        }
        public override int GetHashCode()
        {
            return target.GetHashCode();
        }

        // instance properties
        public string name
        {
            get { return target.name; }
            set { target.name = value; }
        }
    }

    public class GameObjectProxy : ObjectProxy
    {
        [MoonSharpHidden]
        new public GameObject target;

        [MoonSharpHidden]
        public GameObjectProxy(GameObject instance) : base(instance)
        {
            target = instance;
            SetActive = instance.SetActive;
        }

        // static methods
        //public static Func<string, GameObject> Find = GameObject.Find;
        
        // instance methods
        public Action<bool> SetActive;
        public Component GetComponent(string type)
        { return target.GetComponent(type); }
        public Component GetComponentInChildren(string type)
        { return target.GetComponentInChildren(Type.GetType(type)); }
        public Component GetComponentInParent(string type)
        { return target.GetComponentInParent(Type.GetType(type)); }
        public Component AddComponent(string type)
        {
            Loadson.Console.Log(type);
            Loadson.Console.Log(Type.GetType(type).ToString());
            return target.AddComponent(Type.GetType(type));
        }

        // instance properties
        public bool activeSelf
        {
            get { return target.activeSelf; }
        }
        public bool activeInHierarchy
        {
            get { return target.activeInHierarchy; }
        }
        public int layer
        {
            get { return target.layer; }
            set { target.layer = value; }
        }
        public Transform transform
        {
            get { return target.transform; }
        }
        public Material material
        {
            get { return target.GetComponent<MeshRenderer>().material; }
            set { target.GetComponent<MeshRenderer>().material = value; }
        }
    }

    public class ComponentProxy : ObjectProxy
    {
        [MoonSharpHidden]
        new public Component target;

        [MoonSharpHidden]
        public ComponentProxy(Component instance) : base(instance)
        {
            target = instance;
            GetComponent = instance.GetComponent;
        }

        // instance methods
        public Func<string, Component> GetComponent;

        // instance properties
        public GameObject gameObject
        {
            get { return target.gameObject; }
        }
        public Transform transform
        {
            get { return target.transform; }
        }
    }

    public class TransformProxy : ComponentProxy
    {
        [MoonSharpHidden]
        new public Transform target;
        [MoonSharpHidden]
        public TransformProxy(Transform instance) : base(instance)
        {
            target = instance;
            Translate = instance.Translate;
            Rotate = instance.Rotate;
            LookAt = instance.LookAt;
            SetParent = instance.SetParent;
            TransformPoint = instance.TransformPoint;
            InverseTransformPoint = instance.InverseTransformPoint;
            TransformDirection = instance.TransformDirection;
            InverseTransformDirection = instance.InverseTransformDirection;
            TransformVector = instance.TransformVector;
            InverseTransformVector = instance.InverseTransformVector;
            RotateAround = instance.RotateAround;
            GetChild = instance.GetChild;
            IsChildOf = instance.IsChildOf;
            DetachChildren = instance.DetachChildren;
            SetSiblingIndex = instance.SetSiblingIndex;
            GetSiblingIndex = instance.GetSiblingIndex;
            Find = instance.Find;
        }

        // instance methods
        public Action<Vector3> Translate;
        public Action<Vector3> Rotate;
        public Action<Transform> LookAt;
        public Action<Transform> SetParent;
        public Func<Vector3, Vector3> TransformPoint;
        public Func<Vector3, Vector3> InverseTransformPoint;
        public Func<Vector3, Vector3> TransformDirection;
        public Func<Vector3, Vector3> InverseTransformDirection;
        public Func<Vector3, Vector3> TransformVector;
        public Func<Vector3, Vector3> InverseTransformVector;
        public Action<Vector3, Vector3, float> RotateAround;
        public Func<int, Transform> GetChild;
        public Func<Transform, bool> IsChildOf;
        public Action DetachChildren;
        public Action<int> SetSiblingIndex;
        public Func<int> GetSiblingIndex;
        public Func<string, Transform> Find;

        // instance properties
        public Vector3 position
        {
            get { return target.position; }
            set { target.position = value; }
        }
        public Vector3 localPosition
        {
            get { return target.localPosition; }
            set { target.localPosition = value; }
        }
        public Quaternion rotation
        {
            get { return target.rotation; }
            set { target.rotation = value; }
        }
        public Quaternion localRotation
        {
            get { return target.localRotation; }
            set { target.localRotation = value; }
        }
        public Vector3 eulerAngles
        {
            get { return target.eulerAngles; }
            set { target.eulerAngles = value; }
        }
        public Vector3 localEulerAngles
        {
            get { return target.localEulerAngles; }
            set { target.localEulerAngles = value; }
        }
        public Vector3 localScale
        {
            get { return target.localScale; }
            set { target.localScale = value; }
        }
        public Vector3 lossyScale
        {
            get { return target.lossyScale; }
        }
        public Transform parent
        {
            get { return target.parent; }
            set { target.parent = value; }
        }
        public Transform root
        {
            get { return target.root; }
        }
        public int childCount
        {
            get { return target.childCount; }
        }
        public Vector3 forward
        {
            get { return target.forward; }
            set { target.forward = value; }
        }
        public Vector3 right
        {
            get { return target.right; }
            set { target.right = value; }
        }
        public Vector3 up
        {
            get { return target.up; }
            set { target.up = value; }
        }
        public int hierarchyCount
        {
            get { return target.hierarchyCount; }
        }
    }

    public class MaterialProxy : ObjectProxy
    {
        [MoonSharpHidden]
        new public Material target;

        [MoonSharpHidden]
        public MaterialProxy(Material instance) : base(instance)
        {
            target = instance;
        }
        public MaterialProxy()
        {
            target = LevelEditor.MaterialManager.InstanceMaterial();
        }
        public MaterialProxy(MaterialProxy original) : this()
        {
            target.CopyPropertiesFromMaterial(original.target);
        }

        public Color color
        {
            get { return target.color; }
            set { target.color = value; }
        }

        // textures
        public Texture mainTexture
        {
            get { return target.mainTexture; }
            set { target.mainTexture = value; }
        }
        public Texture normalTexture
        {
            get { return target.GetTexture("_BumpMap"); }
            set { target.SetTexture("_BumpMap", value); }
        }
        public Texture metalicGlossTexture
        {
            get { return target.GetTexture("_MetallicGlossMap"); }
            set { target.SetTexture("_MetallicGlossMap", value); }
        }

        public Vector2 textureOffset
        {
            get { return target.mainTextureOffset; }
            set
            {
                target.SetTextureOffset("_MainTex", value);
                target.SetTextureOffset("_BumpMap", value);
                target.SetTextureOffset("_MetallicGlossMap", value);
            }
        }
        public Vector2 textureScale
        {
            get { return target.mainTextureScale; }
            set
            {
                target.SetTextureScale("_MainTex", value);
                target.SetTextureScale("_BumpMap", value);
                target.SetTextureScale("_MetallicGlossMap", value);
            }
        }

        public LevelEditor.MaterialManager.ShaderBlendMode Mode
        {
            get { return (LevelEditor.MaterialManager.ShaderBlendMode)target.GetFloat("_Mode"); }
            set { LevelEditor.MaterialManager.UpdateMode(target, value); }
        }

        public float metallic
        {
            get { return target.GetFloat("_Metallic"); }
            set { target.SetFloat("_Metallic", value); }
        }
        public float glossiness
        {
            get { return target.GetFloat("_Glossiness"); }
            set { target.SetFloat("_Glossiness", value); }
        }
        public bool specularHighlight
        {
            get { return target.GetFloat("_SpecularHighlights") != 0; }
            set { target.SetFloat("_SpecularHighlights", value ? 1 : 0); }
        }
        public bool specularReflection
        {
            get { return target.GetFloat("_GlossyReflections") != 0; }
            set { target.SetFloat("_GlossyReflections", value ? 1 : 0); }
        }
    }

    public class TextMeshProProxy : ComponentProxy
    {
        [MoonSharpHidden]
        new public TextMeshPro target;

        [MoonSharpHidden]
        public TextMeshProProxy(TextMeshPro instance) : base(instance)
        {
            target = instance;
        }

        public string text
        {
            get { return target.text; }
            set { target.text = value; }
        }

        public Color color
        {
            get { return target.color; }
            set { target.color = value; }
        }
    }

    public class LightProxy : ComponentProxy
    {
        [MoonSharpHidden]
        new public Light target;

        [MoonSharpHidden]
        public LightProxy(Light instance) : base(instance)
        {
            target = instance;
        }
        public Color color
        {
            get { return target.color; }
            set { target.color = value; }
        }
        public bool SpotLight
        {
            get { return target.type == LightType.Spot; }
            set { if (target.type != LightType.Directional) target.type = value ? LightType.Spot : LightType.Point; }
        }
        public float intensity
        {
            get { return target.intensity; }
            set { target.intensity = value; }
        }
        public float range
        {
            get { return target.range; }
            set { target.range = value; }
        }
        public float spotAngle
        {
            get { return target.spotAngle; }
            set { target.spotAngle = value; }
        }
    }

    public class RigidbodyProxy : ComponentProxy
    {
        [MoonSharpHidden]
        new public Rigidbody target;
        [MoonSharpHidden]
        public RigidbodyProxy(Rigidbody instance) : base(instance)
        {
            target = instance;
            AddForce = instance.AddForce;
            AddRelativeForce = instance.AddRelativeForce;
            AddTorque = instance.AddTorque;
            AddRelativeTorque = instance.AddRelativeTorque;
            AddForceAtPosition = instance.AddForceAtPosition;
            AddExplosionForce = instance.AddExplosionForce;
            MovePosition = instance.MovePosition;
            MoveRotation = instance.MoveRotation;
            Sleep = instance.Sleep;
            WakeUp = instance.WakeUp;
            IsSleeping = instance.IsSleeping;
            GetPointVelocity = instance.GetPointVelocity;
            GetRelativePointVelocity = instance.GetRelativePointVelocity;
            ClosestPointOnBounds = instance.ClosestPointOnBounds;
        }

        // instance methods
        public Action<Vector3> AddForce;
        public Action<Vector3> AddRelativeForce;
        public Action<Vector3> AddTorque;
        public Action<Vector3> AddRelativeTorque;
        public Action<Vector3, Vector3> AddForceAtPosition;
        public Action<float, Vector3, float, float> AddExplosionForce;
        public Action<Vector3> MovePosition;
        public Action<Quaternion> MoveRotation;
        public Action Sleep;
        public Action WakeUp;
        public Func<bool> IsSleeping;
        public Func<Vector3, Vector3> GetPointVelocity;
        public Func<Vector3, Vector3> GetRelativePointVelocity;
        public Func<Vector3, Vector3> ClosestPointOnBounds;

        // instance properties
        public Vector3 velocity
        {
            get { return target.velocity; }
            set { target.velocity = value; }
        }
        public Vector3 angularVelocity
        {
            get { return target.angularVelocity; }
            set { target.angularVelocity = value; }
        }
        public float mass
        {
            get { return target.mass; }
            set { target.mass = value; }
        }
        public float drag
        {
            get { return target.drag; }
            set { target.drag = value; }
        }
        public float angularDrag
        {
            get { return target.angularDrag; }
            set { target.angularDrag = value; }
        }
        public bool useGravity
        {
            get { return target.useGravity; }
            set { target.useGravity = value; }
        }
        public bool isKinematic
        {
            get { return target.isKinematic; }
            set { target.isKinematic = value; }
        }
        public bool freezeRotation
        {
            get { return target.freezeRotation; }
            set { target.freezeRotation = value; }
        }
        public RigidbodyConstraints constraints
        {
            get { return target.constraints; }
            set { target.constraints = value; }
        }
        public CollisionDetectionMode collisionDetectionMode
        {
            get { return target.collisionDetectionMode; }
            set { target.collisionDetectionMode = value; }
        }
        public RigidbodyInterpolation interpolation
        {
            get { return target.interpolation; }
            set { target.interpolation = value; }
        }
        public Vector3 centerOfMass
        {
            get { return target.centerOfMass; }
            set { target.centerOfMass = value; }
        }
        public Vector3 worldCenterOfMass
        {
            get { return target.worldCenterOfMass; }
        }
        public Vector3 inertiaTensor
        {
            get { return target.inertiaTensor; }
            set { target.inertiaTensor = value; }
        }
        public Quaternion inertiaTensorRotation
        {
            get { return target.inertiaTensorRotation; }
            set { target.inertiaTensorRotation = value; }
        }
        public float maxAngularVelocity
        {
            get { return target.maxAngularVelocity; }
            set { target.maxAngularVelocity = value; }
        }
        public float maxDepenetrationVelocity
        {
            get { return target.maxDepenetrationVelocity; }
            set { target.maxDepenetrationVelocity = value; }
        }
        public float sleepThreshold
        {
            get { return target.sleepThreshold; }
            set { target.sleepThreshold = value; }
        }
        public bool detectCollisions
        {
            get { return target.detectCollisions; }
            set { target.detectCollisions = value; }
        }
        public Vector3 position
        {
            get { return target.position; }
            set { target.position = value; }
        }
        public Quaternion rotation
        {
            get { return target.rotation; }
            set { target.rotation = value; }
        }
        public int solverIterations
        {
            get { return target.solverIterations; }
            set { target.solverIterations = value; }
        }
        public int solverVelocityIterations
        {
            get { return target.solverVelocityIterations; }
            set { target.solverVelocityIterations = value; }
        }
    }

    public class ColliderProxy : ComponentProxy
    {
        [MoonSharpHidden]
        new public Collider target;

        [MoonSharpHidden]
        public ColliderProxy(Collider instance) : base(instance)
        {
            target = instance;
            ClosestPoint = target.ClosestPoint;
            ClosestPointOnBounds = target.ClosestPointOnBounds;
        }

        // instance methods
        public Func<Vector3, Vector3> ClosestPoint;
        public Func<Vector3, Vector3> ClosestPointOnBounds;

        public bool enabled
        {
            get { return target.enabled; }
            set { target.enabled = value; }
        }
        public PhysicMaterial material
        {
            get { return target.material; }
            set { target.material = value; }
        }

        public Table Raycast(Vector3 origin, Vector3 direction, float maxDistance = Mathf.Infinity)
        {
            Ray ray = new Ray(origin, direction);
            if (target.Raycast(ray, out RaycastHit info, maxDistance))
            {
                Table result = new Table(LevelPlayer.LuaScript.script);
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
    }

    public class PhysicMaterialProxy : ObjectProxy
    {
        [MoonSharpHidden]
        new public PhysicMaterial target;

        [MoonSharpHidden]
        public PhysicMaterialProxy(PhysicMaterial instance) : base(instance)
        {
            target = instance;
        }

        public float bounciness
        {
            get { return target.bounciness; }
            set { target.bounciness = value; }
        }
        public float dynamicFriction
        {
            get { return target.dynamicFriction; }
            set { target.dynamicFriction = value; }
        }
        public float staticFriction
        {
            get { return target.staticFriction; }
            set { target.staticFriction = value; }
        }
        public PhysicMaterialCombine bounceCombine
        {
            get { return target.bounceCombine; }
            set { target.bounceCombine = value; }
        }
        public PhysicMaterialCombine frictionCombine
        {
            get { return target.frictionCombine; }
            set { target.frictionCombine = value; }
        }
    }

    public class PlayerProxy : ComponentProxy
    {
        [MoonSharpHidden]
        new public PlayerMovement target;

        [MoonSharpHidden]
        public PlayerProxy(PlayerMovement instance) : base(instance)
        {
            target = instance;
        }

        public void Kill()
        {
            target.KillPlayer();
        }
        public void Win()
        {
            Game.Instance.Win();
        }

        public bool grounded
        {
            get { return target.grounded; }
        }
        public bool crouching
        {
            get { return target.IsCrouching(); }
        }
        public bool dead
        {
            get { return target.IsDead(); }
        }
        public bool hasGun
        {
            get { return target.HasGun(); }
        }
    }
}
