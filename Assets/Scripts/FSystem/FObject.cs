using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSystem
{
    public partial class FObject : MonoBehaviour
    {
        [SerializeField]
        internal FComponent[] components;
        
        public T GetComponent<T>(ushort index = 0) where T : FComponent
        { 
            // going through all of components, turning until index is 0
            foreach(var c in components) 
                if(c is T && index-- == 0) 
                    return (T)c;
            // if none was found, return null
            return default(T);
        }
    }
}
