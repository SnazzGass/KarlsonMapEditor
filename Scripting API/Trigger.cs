using MoonSharp.Interpreter;
using System;
using UnityEngine;

namespace KarlsonMapEditor.Scripting_API
{
    internal class Trigger : MonoBehaviour
    {
        public DynValue Enter;
        public DynValue Exit;
        public DynValue Stay;

        [MoonSharpHidden]
        public void OnTriggerEnter(Collider other)
        {
            if (Enter != null && Enter.Type == DataType.Function)
            {
                Enter.Function.Call(other.gameObject);
            }
        }
        [MoonSharpHidden]
        public void OnTriggerExit(Collider other)
        {
            if (Exit != null && Exit.Type == DataType.Function)
            {
                Exit.Function.Call(other.gameObject);
            }
        }
        [MoonSharpHidden]
        public void OnTriggerStay(Collider other)
        {
            if (Stay != null && Stay.Type == DataType.Function)
            {
                Stay.Function.Call(other.gameObject);
            }
        }
    }
}
