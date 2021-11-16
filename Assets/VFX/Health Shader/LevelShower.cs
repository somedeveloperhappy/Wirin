using System;
using UnityEngine;

public class LevelShower : MonoBehaviour
{
	public UnityEngine.UI.Image image;

	#region handy refs
	LevelManaging.LevelManager levelManager => References.levelManager;
	#endregion

	float previousValue = -1;

	public void Update() {

		if (IsPlaying ()) {
			;
			float value = Mathf.InverseLerp (0, levelManager.levelContaining.goalPoints, levelManager.levelContaining.pointsTaken);
			if (previousValue != value) {
				previousValue = value;
				image.material.SetFloat ("_value", value);
			}
		}

	}

	private bool IsPlaying() => References.gameController.isPlaying;

}
