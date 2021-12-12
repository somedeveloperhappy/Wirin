using System;
using UnityEngine;

namespace Statics
{
	public class InputGetter : MonoBehaviour
	{

		/// <summary>
		///     returns true only during the first frame the pointer is down
		/// </summary>
		[HideInInspector] public static bool isPointerJustDown;

		/// <summary>
		///     returns true only during the frame the pointer is let go
		/// </summary>
		[HideInInspector] public static bool isPointerJustUp;

		/// <summary>
		///     returns true if the pointer is held down ( also true for the first and last frame )
		/// </summary>
		/// <value></value>
		public static bool isPoinerDown
		{
			get
			{
				return Inputs.InputHandler.current.isTouchDown ();
			}
		}

		[HideInInspector] public static Vector2 pointerPosition { get; private set; }

		private void Update()
		{
			if (isPoinerDown)
				pointer_down_update ();
			else
				pointer_up_update ();
		}

		private void pointer_down_update()
		{
			// save pointer position
			pointerPosition = GetPointerPosition ();

			if (!first_frame_up) first_frame_up = true;
			if (isPointerJustUp) isPointerJustUp = false;

			if (first_frame_down)
			{
				// on pointer down start
				isPointerJustDown = true;
				onPointerJustDown?.Invoke (pointerPosition);
			}
			else
			{
				isPointerJustDown = false;
			}

			first_frame_down = false;

			// happens all the frames when pointer is down
			onPointerDown?.Invoke (pointerPosition);
		}

		private void pointer_up_update()
		{
			if (!first_frame_down) first_frame_down = true;
			if (isPointerJustDown) isPointerJustDown = false;

			if (first_frame_up)
			{
				// on pointer up start
				isPointerJustUp = true;
				onPointerUp?.Invoke (pointerPosition);
			}
			else
			{
				isPointerJustUp = false;
			}

			first_frame_up = false;
		}


		/// <summary>
		///     returns pointer position. if none was held down, returns positive infinity
		/// </summary>
		public static Vector2 GetPointerPosition()
		{
			return Inputs.InputHandler.current.getTouchPosition ();
		}

		/// <summary>
		///     returns the pointer position in world in 2D. where Z is ignored
		/// </summary>
		public static Vector2 GetPointerWorldPosition(int index = 0)
		{
			return Camera.main.ScreenToWorldPoint (pointerPosition);
		}

		#region events

		/// <summary>
		///     gets called during the frames where the pointer is down
		/// </summary>
		public static event Action<Vector2> onPointerDown;

		/// <summary>
		///     gets called during the frame where the pointer is let go
		/// </summary>
		public static event Action<Vector2> onPointerUp;

		/// <summary>
		///     gets called only during the first frame the pointer is down
		/// </summary>
		public static event Action<Vector2> onPointerJustDown;

		#endregion

		#region helper vars

		private bool first_frame_down = true;
		private bool first_frame_up = true;

		#endregion

	}
}
