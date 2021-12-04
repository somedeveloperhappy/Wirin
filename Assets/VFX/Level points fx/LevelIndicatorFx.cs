using System;
using Gameplay.EnemyNamespace.Types;
using LevelManaging;
using UnityEngine;
using UnityEngine.UI;

namespace CanvasSystem
{
	public class LevelIndicatorFx : MonoBehaviour, IOnCanvasEnabled
	{
		public Image fillImage;
		public Text levelText;
		public Text pointsText;

		public Settings settings;

		private float showingValue;
		private float speed;

		public void OnCanvasEnable() { }

		private void Start()
		{
			References.levelManager.onStartLevel += onLevelStart;
			References.levelManager.onEnemyDestroy += onEnemyDestroy;
		}

		private void onEnemyDestroy(EnemyBase enemy)
		{
			// recalculating speed
			speed = (pointsTaken - showingValue) / settings.reachInSeconds;
		}

		private void onLevelStart()
		{
			// level number display
			levelText.text = string.Concat("Level ", levelManager.levelNumber);
			Show();
		}

		public void Update()
		{
			if (IsNotPlaying()) return;

			// check if should update
			if (showingValue != pointsTaken)
			{
				// updating showing value
				showingValue += speed * Time.deltaTime;
				if (showingValue > pointsTaken) showingValue = pointsTaken;

				// show
				Show();
			}
		}

		private void Show()
		{
			pointsText.text = (int) showingValue + " / " + levelManager.levelStats.goalPoints;
			fillImage.material.SetFloat("_value", showingValue / levelManager.levelStats.goalPoints);
		}

		private bool IsNotPlaying()
		{
			return !References.gameController.gameplayMechanicsActive;
		}

		[Serializable]
		public struct Settings
		{
			[Tooltip(
				"the showing points will take this long to reach the actual points, recalculated each time we got a new points")]
			public float reachInSeconds;
		}

#region handy refs

		private LevelManager levelManager => References.levelManager;
		private int pointsTaken => levelManager.levelStats.pointsTaken;

#endregion

	}
}