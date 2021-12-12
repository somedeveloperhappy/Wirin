using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Bullets
{
	public abstract class BulletBase : MonoBehaviour
	{
		static public List<BulletBase> instances = new List<BulletBase>();
		
		protected virtual void Awake()
		{
			instances.Add(this);
		}

		protected virtual void OnDestroy()
		{
			instances.Remove(this);
		}


	}
}
