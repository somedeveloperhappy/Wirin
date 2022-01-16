using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.PressSystem
{
    /// <summary>
    ///     provides a method for a fine tuned press normalized curve for effects.
    ///     you should Initialize it before using it. at Start
    /// </summary>
    public interface IOnPressFx
    {
        /// <param name="normalizedT">normalized</param>
        [ContextMenu("Apply")]
        public void Apply(float normalizedT);
    }

    public static class IOnPressFXSettingsHelper
    {
        public static List<IOnPressFx> instances = new List<IOnPressFx>();

        public static void DefaultInitialize(this IOnPressFx instance)
        {
            if (!instances.Contains(instance))
                instances.Add(instance);
        }
        public static void DefaultDestroy(this IOnPressFx instance)
        {
            instances.Remove(instance);
        }
    }
}