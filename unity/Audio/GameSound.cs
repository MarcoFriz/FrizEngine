using UnityEngine;

namespace FrizEngine.Audio
{
	public class GameSound:MonoBehaviour
	{
		[SerializeField]string startState = "";
		[SerializeField]AudioSource Source = null;
		[SerializeField]SoundStates[] Sounds = new SoundStates[1];

		void Start ()
		{
			Play (startState);
		}

		public void Play (string state)
		{
			foreach (var sound in Sounds) {
				if (sound == null)
					continue;
				if (sound.name == state) {
					Source.clip = sound.clip;
					Source.Play ();
					break;
				}
			}
		}

		public static GameSound Find (string name)
		{
			var sounds = GameObject.FindObjectsOfType <GameSound> ();
			foreach (var item in sounds) {
				if (item.name == name)
					return item;
			}
			return null;
		}
	}

	[System.Serializable]
	public class SoundStates
	{
		public string name;
		public AudioClip clip;
	}
}