using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrizEngine.Utilities
{
	public class ScreenManager : MonoBehaviour
	{
		#if !RELEASE_VERSION
		[SerializeField] bool showGUI = true;
		[SerializeField] bool verbose = false;


		void OnGUI ()
		{
			if (!showGUI)
				return;
			GUILayout.BeginVertical ();
			if (GUILayout.Button ("Hide All")) {
				ChangeScreen ("null");
			}

			foreach (var item in screens) {
				GUILayout.BeginHorizontal ();
				GUILayout.TextField (item.name + " ");
				if (GUILayout.Button ("Active")) {
					ActiveScreen (item.name);
				}
				if (GUILayout.Button ("Change")) {
					ChangeScreen (item.name);
				}
				GUILayout.EndHorizontal ();
			}
			GUILayout.EndVertical ();
		}
		#endif



		[SerializeField] string startScreen = "null";
		[SerializeField] MenuMangerScreens[] screens = new MenuMangerScreens[1];
		public MonoBehaviourEvents events;

		#region Class members

		#endregion

		#region Class Accesors

		public void SetStartScreen (string screen)
		{
			startScreen = screen;
		}

		public void ChangeScreen (string name)
		{
			#if !RELEASE_VERSION
			if (verbose)
				Debug.LogFormat ("Chanching <b>{0}</b>", name);
			#endif
			foreach (MenuMangerScreens screen in screens) {
				screen.SetActive (false);
			}
			foreach (MenuMangerScreens screen in screens) {
				if (screen.name == name)
					screen.SetActive (true);
			}
		}

		public void ActiveScreen (string name)
		{
			ActiveScreen (name, true);
		}

		public void InactiveScreen (string name)
		{
			ActiveScreen (name, false);
		}

		public void ActiveScreen (string name, bool active)
		{
			#if !RELEASE_VERSION
			if (verbose)
				Debug.LogFormat ("Changing <b>{0} : {1}</b>", name, active);
			#endif
			foreach (MenuMangerScreens screen in screens) {
				if (screen.name == name)
					screen.SetActive (active);
			}
		}

		#endregion

		#region MonoBehaviour overrides

		///Initialization
		//void Reset(){}
		void Awake ()
		{
			ChangeScreen (startScreen);
		}
		//void OnEnable(){}
		//void Start(){}

		//Physics
		//void FixedUpdate() {}
		//void OnTriggerEnter(Collider other){}
		//void OnCollisionEnter(Collision collision){}

		//Game Logic
		//void Update (){}
		//void LateUpdate(){}

		//Decommissioning
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

	[System.Serializable]
	public struct MenuMangerScreens
	{
		public string name;
		public GameObject[] screens;

		public void SetActive (bool active)
		{
			foreach (GameObject item in screens) {
				item.SetActive (active);
			}
		}
	}
}