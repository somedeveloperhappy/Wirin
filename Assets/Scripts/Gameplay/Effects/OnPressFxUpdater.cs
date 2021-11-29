using UnityEngine;

namespace PressFX
{
	[RequireComponent (typeof (PlayerInfo))]
	public class OnPressFxUpdater : MonoBehaviour, IOnPlayerPress
	{
		// public IOnPressFx[] instances;
		public PlayerInfo playerInfo;
		
		System.Collections.Generic.List<IOnPressFx> instances => PressFX.IOnPressFXSettingsHelper.instances;
		

		[Range(0f, 1f)] public float currentT = -1; // keeping track so it won't calculate something twice in a row

		private void Start()
		{
			this.Initialize();
			playerInfo = GetComponent<PlayerInfo> ();
		}

		[ContextMenu ("FindInstances")]
		public void FindInstances()
		{
			// instances = transform.GetComponentsInChildren<IOnPressFx> ();
			// Debug.Log($"found {instances.Length} instances");
		}

		void IOnPlayerPress.OnPressDown(float duration) { }
		void IOnPlayerPress.OnPressUp(float duration) { }

		void IOnPlayerPress.OnPressDownUpdate()
		{
			UpdateInstances ();
		}

		void IOnPlayerPress.OnPressUpUpdate()
		{
			UpdateInstances ();
		}

		[ContextMenu("Update all")]
		private void UpdateInstances()
		{
			// Debug.Log($"going into update phase. instance count : {instances.Count}");
			float t = playerInfo.GetNormalCharge ();
			if (t != currentT)
			{
				currentT = t;
				foreach (var instance in instances)
				{
					// Debug.Log($"updating {instance}");
					instance.Apply (currentT);
				}
			}
		}
	}
}
