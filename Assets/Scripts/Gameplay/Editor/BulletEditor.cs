using Gameplay.Player;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Editor
{
    [CustomEditor(typeof(PlayerNormalBullet))]
    public class BulletEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var tar = target as PlayerNormalBullet;

            var boundryRange = (float)tar.GetType().GetField("boundryRange",
                BindingFlags.Instance | BindingFlags.NonPublic).GetValue(tar);

            Handles.color = Color.green;
            Handles.DrawWireCube(tar.transform.position, Vector3.one * boundryRange);
        }
    }
}