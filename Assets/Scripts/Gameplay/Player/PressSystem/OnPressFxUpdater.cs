using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.PressSystem
{
	[RequireComponent(typeof(PlayerInfo))]
	public class OnPressFxUpdater : MonoBehaviour, IOnPlayerPress
	{


		[Range(0f, 1f)] public float currentT = -1; // keeping track so it won't calculate something twice in a row

		// public IOnPressFx[] instances;
		public PlayerInfo playerInfo;

		private List<IOnPressFx> instances => IOnPressFXSettingsHelper.instances;

		void IOnPlayerPress.OnPressDown(float duration) { }
		void IOnPlayerPress.OnPressUp(float duration) { }

		void IOnPlayerPress.OnPressDownUpdate()
		{
			UpdateInstances();
		}

		void IOnPlayerPress.OnPressUpUpdate()
		{
			UpdateInstances();
		}

		private void Start()
		{
			this.Initialize();
			playerInfo = GetComponent<PlayerInfo>();
		}

		[ContextMenu("FindInstances")]
		public void FindInstances()
		{
			// instances = transform.GetComponentsInChildren<IOnPressFx> ();
			// Debug.Log($"found {instances.Length} instances");
		}

		[ContextMenu("Update all")]
		private void UpdateInstances()
		{
			// Debug.Log($"going into update phase. instance count : {instances.Count}");
			var t = playerInfo.GetNormalCharge();
			if (t != currentT)
			{
				currentT = t;
				foreach (var instance in instances)
				{
					// Debug.Log($"updating {instance}");
					instance.Apply(currentT);
				}
			}
		}
	}
}