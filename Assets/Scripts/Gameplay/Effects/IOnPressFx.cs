using UnityEngine;

namespace PressFX
{
    /// <summary>
    /// provides a method for a fine tuned press normalized curve for effects.
    /// you should Initialize it before using it. at Start
    /// </summary>
    public interface IOnPressFx
    {
        /// <param name="normalizedT">normalized</param>
        [ContextMenu("Apply")]
        public void Apply(float normalizedT);
        
        /// <summary>
        /// this should be called as soon as possible. preferably in Start
        /// </summary>
        public void Initialize();
        
    }
    
    static public class IOnPressFXSettingsHelper
    {
        static public System.Collections.Generic.List<IOnPressFx> instances = new System.Collections.Generic.List<IOnPressFx>();
        
        static public void DefaultInitialize(this IOnPressFx instance)
        {
            if(!instances.Contains(instance))
                instances.Add(instance);
        }
    }
}
