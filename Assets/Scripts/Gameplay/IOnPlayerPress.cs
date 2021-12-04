using System;
using System.Collections.Generic;

namespace Gameplay
{
	public interface IOnPlayerPress
	{
		void OnPressDown(float duration);
		void OnPressUp(float duration);
		void OnPressDownUpdate();
		void OnPressUpUpdate();
	}

	public static class IOnPlayerPressHelper
	{
		public static List<IOnPlayerPress> instances =
			new List<IOnPlayerPress>();

		public static void Initialize(this IOnPlayerPress onPlayerPress)
		{
			instances.Add(onPlayerPress);
		}

		public static void ForeachInstance(Action<IOnPlayerPress> function)
		{
			instances.ForEach(function);
		}
	}
}