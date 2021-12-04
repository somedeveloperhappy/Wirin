using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Bullets
{
	public abstract class Bullet : MonoBehaviour
	{
		/// <summary>
		/// the shooter of this bullet
		/// </summary>
		[HideInInspector] public GameObject shooter;
		
		static public List<Bullet> instances = new List<Bullet>();

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