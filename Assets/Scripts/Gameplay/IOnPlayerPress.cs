using System;
using System.Collections.Generic;

namespace Gameplay
{
    /// <summary>
    /// should call <code>this.OnPlayerPressInit()</code> and <code>this.OnPlayerPressDestroy()</code>
    /// </summary>
    public interface IOnPlayerPress
    {
        void OnPressDown(float duration);
        void OnPressUp(float duration);
        void OnPressDownUpdate();
        void OnPressUpUpdate();
    }

    public static class OnPlayerPress
    {
        public static List<IOnPlayerPress> instances =
            new List<IOnPlayerPress>();

        public static void OnPlayerPressInit(this IOnPlayerPress onPlayerPress)
        {
            instances.Add(onPlayerPress);
        }
        public static void OnPlayerPressDestroy(this IOnPlayerPress onPlayerPress)
        {
            instances.Remove(onPlayerPress);
        }

        public static void ForeachInstance(Action<IOnPlayerPress> function)
        {
            instances.ForEach(function);
        }
    }
}