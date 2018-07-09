using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FrizEngine.Utilities
{
	
	public class MonoBehaviourEvents : EventTrigger
	{
		public Rigidbody rgb;
		public Rigidbody2D rgb2D;
		new	public Collider collider;
		new	public Collider2D collider2D;

		public event System.Action<MonoBehaviourEvents> onUpdate = delegate {};
		public event System.Action<MonoBehaviourEvents> onFixedUpdate = delegate {};
		public event System.Action<MonoBehaviourEvents> onEnable = delegate {};
		public event System.Action<MonoBehaviourEvents> onDisable = delegate {};
		public event System.Action<MonoBehaviourEvents> onDestroy = delegate {};

		public event System.Action<MonoBehaviourEvents,GameObject> onCollisionEnter = delegate {};
		public event System.Action<MonoBehaviourEvents,GameObject> onTriggerEnter = delegate {};
		public event System.Action<MonoBehaviourEvents,GameObject> onCollisionStay = delegate {};
		public event System.Action<MonoBehaviourEvents,GameObject> onTriggerStay = delegate {};
		public event System.Action<MonoBehaviourEvents,GameObject> onCollisionExit = delegate {};
		public event System.Action<MonoBehaviourEvents,GameObject> onTriggerExit = delegate {};

		public event System.Action<MonoBehaviourEvents> onAnimationStart = delegate {};
		public event System.Action<MonoBehaviourEvents,string> onAnimationEvent = delegate {};
		public event System.Action<MonoBehaviourEvents> onAnimationEnd = delegate {};

		public event System.Action<MonoBehaviourEvents> onMouseDown = delegate {};
		public event System.Action<MonoBehaviourEvents> onMouseDrag= delegate {};
		public event System.Action<MonoBehaviourEvents> onMouseEnter= delegate {};
		public event System.Action<MonoBehaviourEvents> onMouseExit= delegate {};
		public event System.Action<MonoBehaviourEvents> onMouseOver= delegate {};
		public event System.Action<MonoBehaviourEvents> onMouseUp= delegate {};
		public event System.Action<MonoBehaviourEvents> onMouseUpAsButton= delegate {};
		public event System.Action<MonoBehaviourEvents> onMouseClick= delegate {};


		//----------------------------

		void Update ()
		{
			onUpdate (this);
		}

		void FixedUpdate ()
		{
			onFixedUpdate (this);
		}
		//
		void OnEnable ()
		{
			onEnable (this);
		}

		void OnDisable ()
		{
			onDisable (this);
		}

		void OnDestroy ()
		{
			onDestroy (this);
		}

		//
		void OnCollisionEnter (Collision collision)
		{
			onCollisionEnter (this, collision.gameObject);
		}

		void OnTriggerEnter (Collider collider)
		{
			onTriggerEnter (this, collider.gameObject);
		}

		void OnCollisionStay (Collision collision)
		{
			onCollisionStay (this, collision.gameObject);
		}

		void OnTriggerStay (Collider collider)
		{
			onTriggerStay (this, collider.gameObject);
		}

		void OnCollisionExit (Collision collision)
		{
			onCollisionExit (this, collision.gameObject);
		}

		void OnTriggerExit (Collider collider)
		{
			onTriggerExit (this, collider.gameObject);
		}
		//2D
		void OnCollisionEnter2D (Collision2D collision)
		{
			onCollisionEnter (this, collision.gameObject);
		}

		void OnTriggerEnter2D (Collider2D collider)
		{
			onTriggerEnter (this, collider.gameObject);
		}

		void OnCollisionStay2D (Collision2D collision)
		{
			onCollisionStay (this, collision.gameObject);
		}

		void OnTriggerStay2D (Collider2D collider)
		{
			onTriggerStay (this, collider.gameObject);
		}

		void OnCollisionExit2D (Collision2D collision)
		{
			onCollisionExit (this, collision.gameObject);
		}

		void OnTriggerExit2D (Collider2D collider)
		{
			onTriggerExit (this, collider.gameObject);
		}
		//------------------------------------------------
		void OnAnimationStart ()
		{
			onAnimationStart (this);
		}

		void OnAnimationEvent (string eventName)
		{
			onAnimationEvent (this, eventName);
		}

		void OnAnimationEnd ()
		{
			onAnimationEnd (this);
		}
		//-----------------------------------------------
		void OnMouseDown ()
		{
			onMouseDown (this);		
		}

		void OnMouseDrag ()
		{
			onMouseDrag (this);		
		}

		void OnMouseEnter ()
		{
			onMouseEnter (this);		
		}

		void OnMouseExit ()
		{
			onMouseExit (this);		
		}

		void OnMouseOver ()
		{
			onMouseOver (this);		
		}

		void OnMouseUp ()
		{
			onMouseUp (this);		
		}

		void OnMouseUpAsButton ()
		{
			onMouseUpAsButton (this);		
		}

		public override void OnPointerDown (PointerEventData events)
		{
			onMouseDown (this);
		}

		public override void OnPointerClick (PointerEventData events)
		{
			onMouseClick (this);
		}

		public override void OnPointerEnter (PointerEventData events)
		{
			onMouseEnter (this);
		}

		public override void OnPointerExit (PointerEventData events)
		{
			onMouseExit (this);
		}

		public override void OnPointerUp (PointerEventData events)
		{
			onMouseUp (this);
		}
	}
}