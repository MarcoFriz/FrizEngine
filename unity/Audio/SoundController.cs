using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace FrizEngine.Audio
{
	public class SoundController : MonoBehaviour
	{
		#region Class members

		public static SoundController instance = null;
		[SerializeField] Slider bgm = null;
		[SerializeField] Slider sfx = null;
		[SerializeField] AudioMixer mixer = null;

		void VolumeChange (float value)
		{
			if (bgm)
				mixer.SetFloat ("BGMVolume", SliderToMixerValue (bgm.value));
			if (sfx)
				mixer.SetFloat ("SFX1Volume", SliderToMixerValue (sfx.value));
			if (sfx)
				mixer.SetFloat ("SFX2Volume", SliderToMixerValue (sfx.value));
			Save (false);
		}

		float SliderToMixerValue (float value)
		{
			float result = value * 35 - 35;
			return result;
		}

		#endregion

		#region Class Accesors

		public void Save (bool back = true)
		{
			if (bgm)
				PlayerPrefs.SetFloat ("bgm", bgm.value);
			if (sfx)
				PlayerPrefs.SetFloat ("sfx", sfx.value);

			if (bgm)
				mixer.SetFloat ("BGMVolume", SliderToMixerValue (bgm.value));
			if (sfx)
				mixer.SetFloat ("SFX1Volume", SliderToMixerValue (sfx.value));
			if (sfx)
				mixer.SetFloat ("SFX2Volume", SliderToMixerValue (sfx.value));
			PlayerPrefs.Save ();
			if (back)
				Back ();
		}

		public void Back ()
		{
			gameObject.SetActive (false);
		}

		/// <summary>
		/// Mute the specified type.
		/// </summary>
		/// <param name="type">Type: bgm or sfx</param>
		public void Mute (string type, bool b = true)
		{
			float value = (b) ? -1 : 1;
			switch (type) {
			case "bgm":
				mixer.SetFloat ("BGMVolume", SliderToMixerValue (value));
				break;
			case "sfx":
				mixer.SetFloat ("SFX1Volume", SliderToMixerValue (value));
				mixer.SetFloat ("SFX2Volume", SliderToMixerValue (value));
				break;
			}
		}

		public bool IsMute (string type)
		{
			float value = 0f;
			switch (type) {
			case "bgm":
				mixer.GetFloat ("BGMVolume", out value);
				break;
			case "sfx":
				mixer.GetFloat ("SFX1Volume", out value);
				break;
			}
			return (value <= -35f);
		}

		#endregion

		#region MonoBehaviour overrides

		#endregion

		#region MonoBehaviors overrides

		///Initialization
		//void Reset(){}
		void Awake ()
		{
			if (instance == null)
				instance = this;
			else
				Destroy (this);
		}
		//void OnEnable(){}
		void Start ()
		{
			if (bgm) {
				bgm.value = PlayerPrefs.GetFloat ("bgm", 1);
				bgm.onValueChanged.AddListener (VolumeChange);
			}
			if (sfx) {
				sfx.value = PlayerPrefs.GetFloat ("sfx", 1);
				sfx.onValueChanged.AddListener (VolumeChange);
			}
		}

		///Physics
		//void FixedUpdate() {}
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