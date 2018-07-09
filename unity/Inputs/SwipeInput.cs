using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace FrizEngine.Inputs
{
	[ExecuteInEditMode]
	[RequireComponent (typeof(StandaloneInputModule))]
	public class SwipeInput : MonoBehaviour
	{
		public static SwipeInput instance;

		public static BaseInput input{ get; private set; }

		public event Action onSlide = delegate {};

		public float angleRange = 44f;
		public float minDistance = 40f;
		public float minVelocity = 150f;

		public bool autoDetect = true;

		public bool forceTap;
		public bool forceTapUp;
		public bool forceSlideToUp;
		public bool forceSlideToDown;
		public bool forceSlideToRight;
		public bool forceSlideToLeft;
		//
		Vector2 startPos = Vector2.zero;
		SwipeDirections currentDirection = SwipeDirections.none;

		#region Class members

		#endregion

		#region Class Accesors

		public bool GetTapUp ()
		{
			if (!autoDetect && forceTapUp) {
				forceTapUp = false;
				return true;
			}
			if (forceTapUp)
				return true;
			if (autoDetect && input.touchSupported && input.touchCount > 0 && input.GetTouch (0).phase == TouchPhase.Ended)
				return true;
			#if UNITY_EDITOR
			if (input.GetMouseButtonUp (0))
				return true;
			#endif
			return false;
		}

		public bool GetTap ()
		{
			if (forceTap)
				return true;
			if (input.touchCount > 0)
				return true;
			#if UNITY_EDITOR
			if (input.GetMouseButton (0))
				return true;
			#endif
			return false;
		}

		public bool GetSlideToUp ()
		{
			if (!autoDetect && forceSlideToUp) {
				forceSlideToUp = false;
				return true;
			}
			if (forceSlideToUp)
				return true;
			return SwipeDirections.Up == GetSlide ();
		}

		public bool GetSlideToDown ()
		{
			if (!autoDetect && forceSlideToDown) {
				forceSlideToDown = false;
				return true;
			}
			if (forceSlideToDown)
				return true;
			return SwipeDirections.Down == GetSlide ();
		}

		public bool GetSlideToLeft ()
		{
			if (!autoDetect && forceSlideToLeft) {
				forceSlideToLeft = false;
				return true;
			}
			if (forceSlideToLeft)
				return true;
			return SwipeDirections.Left == GetSlide ();
		}

		public bool GetSlideToRight ()
		{
			if (!autoDetect && forceSlideToRight) {
				forceSlideToRight = false;
				return true;
			}
			if (forceSlideToRight)
				return true;
			return SwipeDirections.Right == GetSlide ();
		}

		SwipeDirections GetSlide ()
		{
			return currentDirection;
		}



		#endregion

		#region MonoBehaviour overrides

		///Initialization
		//void Reset(){}
		//void Awake (){}
		void OnEnable ()
		{
			instance = this;
			StandaloneInputModule o = GetComponent<StandaloneInputModule> ();
			input = o.input;
		}
		//void Start () {}

		///Physics
		//void FixedUpdate() {}
		//void OnTriggerEnter(Collider other){}
		//void OnCollisionEnter(Collision collision){}
	
		///Game Logic
		void Update ()
		{
			if (!autoDetect)
				return;
			bool isDown = false;
			bool isUp = false;
			isDown = input.GetMouseButtonDown (0);
			isUp = input.GetMouseButtonUp (0);
			if (isDown) {
				startPos = input.mousePosition;
			}
			if (isUp) {
				var endPos = input.mousePosition;
				var direction = endPos - startPos;
				var distance = direction.magnitude;
				var velocity = distance / Time.deltaTime;
				if (velocity > minVelocity && distance > minDistance) {
					direction.Normalize ();
					var angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
					if (-angleRange / 2f < angle && angle < angleRange / 2f) {
						currentDirection = SwipeDirections.Right;
						onSlide ();
					} else if (90f - angleRange / 2f < angle && angle < 90 + angleRange / 2f) {
						currentDirection = SwipeDirections.Up;
						onSlide ();
					} else if (180f - angleRange / 2f < angle && angle < 180 + angleRange / 2f) {
						currentDirection = SwipeDirections.Left;
						onSlide ();
					} else if (-90f - angleRange / 2f < angle && angle < -90 + angleRange / 2f) {
						currentDirection = SwipeDirections.Down;
						onSlide ();
					}
				}
				currentDirection = SwipeDirections.none;
			}
		}
		//void LateUpdate(){}
	
		///Decommissioning
		//void OnApplicationQuit(){}
		//void OnDisable(){}
		//void OnDestroy(){}

		#endregion

		#region Super class overrides

		#endregion

		#region Class implementation

		#endregion

		#region Interface implementation

		#endregion
	}

	public enum SwipeDirections
	{
		none,
		Up,
		Down,
		Left,
		Right
	}
}