using System.Collections;
using System.Collections.Generic;
using LevelManaging;
using UnityEngine;

namespace Enemies
{
	public class EnemyTrig : Enemy
	{
		public override float Health
		{
			get => throw new System.NotImplementedException ();
			set => throw new System.NotImplementedException ();
		}

		public override bool Equals(object other)
		{
			return base.Equals (other);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		public override void Init(int points)
		{
			base.Init (points);
		}

		public override string ToString()
		{
			return base.ToString ();
		}

		protected override void AfterDestruction()
		{
			base.AfterDestruction ();
		}

		protected override void Awake()
		{
			base.Awake ();
		}

		protected override void BeforeDestruction()
		{
			base.BeforeDestruction ();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy ();
		}

		protected override void OnTakeDamage()
		{
			base.OnTakeDamage ();
		}
	}

}
