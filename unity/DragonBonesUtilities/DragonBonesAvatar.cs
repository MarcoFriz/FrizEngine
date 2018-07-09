#if DragonBonesApi
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

namespace FrizEngine.DragonBonesUtilities
{
	public class DragonBonesAvatar : MonoBehaviour
	{
		#if !RELEASE_VERSION
		[SerializeField] bool showGUI = false;

		void OnGUI ()
		{
			if (!showGUI)
				return;
			if (GUILayout.Button ("Layer 1") || Input.GetKey (KeyCode.Alpha1))
				SetLayer (0);
			if (GUILayout.Button ("Layer 2") || Input.GetKey (KeyCode.Alpha2))
				SetLayer (1);
			if (GUILayout.Button ("Layer 3") || Input.GetKey (KeyCode.Alpha3))
				SetLayer (2);
			if (GUILayout.Button ("Layer 4") || Input.GetKey (KeyCode.Alpha4))
				SetLayer (3);
			if (GUILayout.Button ("Animacion 1") || Input.GetKey (KeyCode.Q))
				SetAnimation (0, false, -1, 1f);
			if (GUILayout.Button ("Animacion 2") || Input.GetKey (KeyCode.W))
				SetAnimation (1, false, -1, 1f);
			if (GUILayout.Button ("Animacion 3") || Input.GetKey (KeyCode.E))
				SetAnimation (2, false, -1, 1f);
			if (GUILayout.Button ("Animacion 4") || Input.GetKey (KeyCode.R))
				SetAnimation (3, false, -1, 1f);
		}

		#endif
		#region Class members

		[SerializeField] UnityArmatureComponent avatar = null;
		[SerializeField] string[] layers = new string[0];
		[SerializeField] string[] animations = new string[0];
		[SerializeField] int animationIndex = 0;
		int _lastAnimationIndex = -1;

		#endregion

		#region Class acsesor

		public bool IsPlaying (string animationName)
		{
			var state = avatar.animation.GetState (animationName);
			return state != null && state.isPlaying;
		}

		public int GetLayersCount ()
		{
			return (layers != null) ? layers.Length : 0;
		}

		public int GetAnimationsCount ()
		{
			return (animations != null) ? animations.Length : 0;
		}

		public void SetLayer (string name)
		{
			for (int i = 0, layersLength = layers.Length; i < layersLength; i++) {
				if (layers [i] == name) {
					SetLayer (i);
					return;
				}
			}
		}

		public void SetLayer (int index)
		{
			if (layers == null || layers.Length <= index) {
				Debug.LogError ("Index no found");
				return;
			}
			string animationName = layers [index];
			var state = avatar.animation.GetState (animationName);
			if (state != null && state.isPlaying)
				return;
			var animConfig = avatar.animation.animationConfig;
			animConfig.resetToPose = true;
			animConfig.animation = animationName;
			animConfig.layer = 0;
			avatar.animation.PlayConfig (animConfig);
		}

		public void SetAnimation (string name, bool force = false, int playTimes = -1, float fadeInTime = -1.0f, string group = null,
		                          AnimationFadeOutMode fadeOutMode = AnimationFadeOutMode.SameLayerAndGroup)
		{
			for (int i = 0, animationsLength = animations.Length; i < animationsLength; i++) {
				if (animations [i] == name) {
					SetAnimation (i, force, playTimes, fadeInTime, group, fadeOutMode);
					return;
				}
			}
		}

		public void SetAnimation (int index, bool force = false, int playTimes = -1, float fadeInTime = -1.0f, string group = null,
		                          AnimationFadeOutMode fadeOutMode = AnimationFadeOutMode.SameLayerAndGroup)
		{
			if (animations == null || animations.Length <= index) {
				Debug.LogError ("Index no found");
				return;
			}
			string animationName = animations [index];
			if (avatar.animation == null)
				return;
			var state = avatar.animation.GetState (animationName);
			if (state != null && (state.isPlaying && !force))
				return;
			var config = avatar.animation.animationConfig;
			config.playTimes = playTimes;
			config.resetToPose = false;
			config.animation = animationName;
			config.layer = 1;
			config.fadeInTime = fadeInTime;
			config.group = group != null ? group : string.Empty;
			config.fadeOutMode = fadeOutMode;
			avatar.animation.PlayConfig (config);
		}

		#endregion

		#region MonoBehaviour overrides

		///Initialization
		//void Reset(){}
		//void Awake(){}
		//void OnEnable(){}
		void Start ()
		{
			if (avatar == null)
				avatar = GetComponent <UnityArmatureComponent> ();
		}

		///Physics
		void FixedUpdate ()
		{
			if (_lastAnimationIndex == animationIndex)
				return;
			_lastAnimationIndex = animationIndex;
			SetAnimation (animationIndex, true, -1, 0.2f);
		}
		//void OnTriggerEnter(Collider other){}
		//void OnCollisionEnter(Collision collision){}
	
		///Game Logic
		//void Update(){}
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
}
#endif