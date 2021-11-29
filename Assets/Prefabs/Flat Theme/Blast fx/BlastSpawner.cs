using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatVFX
{
	public class BlastSpawner : MonoBehaviour
	{
		public Bullet bullet;

		[System.Serializable]
		public class BlastFX
		{
			public GameObject gamebject;
			[Range(0f, 1f)] public float maxT;
		}
		public BlastFX[] blasts;

		private void Awake() {
			bullet.onInit_fine += Init;
			bullet.onHit += onBulletHit;
		}

		float t;

		private void Init(float t) {
			this.t = t;

			// sorting blasts
			// SortBlastsBasedOnMaxT ();
		}

        [ContextMenu("Sort")]
		public void SortBlastsBasedOnMaxT() {
			for (int i = 0; i < blasts.Length - 1; i++) {
				for (int j = i + 1; j < blasts.Length; j++) {
					if (blasts[ i ].maxT > blasts[ j ].maxT) {
						// swap
						var tmp = blasts[ i ];
						blasts[ i ] = blasts[ j ];
						blasts[ j ] = tmp;
					}
				}
			}
		}

		private void onBulletHit(float damage) 
        {
            // checkig the blast from first to last
            for (int i = 0; i < blasts.Length; i++)
            {
                if(blasts[i].maxT >= t)
                {
                    // blast!
                    Vector3 rot = bullet.transform.eulerAngles;
                    rot.z = -rot.z;
                    Instantiate(blasts[i].gamebject, transform.position, Quaternion.Euler(rot));
                    Debug.Log($"blast {i+1}. T was {t}");
                    return;
                }
            }
		}
	}
}
