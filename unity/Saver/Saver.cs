/// <summary>
/// Saver. v1
/// </summary>
using UnityEngine;

namespace FrizEngine.Saves
{
	[System.Serializable]
	public class Saver
	{
		#region Class members

		public string directoryPath = string.Empty;
		public string path = "file";
		public string version = "1.1";

		#endregion

		#region Class Accesors

		public void Load ()
		{
			if (!XMLSaver.CheckIfFileExist (path + ".json"))
				return;
			string json = XMLSaver.GetFileData (path + ".json");
			JsonUtility.FromJsonOverwrite (json, this);
		}

		public void Save ()
		{
			XMLSaver.SaveJSON (this, path);
		}

		public void Upgrade ()
		{
			directoryPath = XMLSaver.GetDirectoryPath ();
			Saver tmp;
			tmp = new Saver ();
			tmp.path = path;
			tmp.Load ();
			if (tmp.version == this.version)
				return;
			#if !RELEASE_VERSION
			Debug.LogFormat ("<color=red>{0}: {1}</color>", "Upgrading", GetType ().ToString ());
			#endif
			OnUpgrade ();
			Save ();
			Load ();
		}

		protected virtual void OnUpgrade ()
		{
			Debug.LogError ("This Method Need Be Overrided");
		}

		#endregion

		#region MonoBehaviour overrides

		///Initialization
		//void Reset(){}
		//void Awake(){}
		//void OnEnable(){}
		//void Start (){}

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


