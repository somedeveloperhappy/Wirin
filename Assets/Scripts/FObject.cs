using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSystem
{
    public class FObject : MonoBehaviour
    {
        internal FComponent[] components;
        
        public T GetComponent<T>(ushort index = 0) where T : FComponent
        { 
            foreach(var c in components) 
                if(c is T && index-- == 0) 
                    return (T)c;
            return null;
        }
    }
}
