using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

[System.Serializable][XmlRoot ("Data")]
public class XMLSaver
{
	[XmlAttribute ("id")]    public int Id = 0;
	[XmlAttribute ("class")] public string Class = "XMLSaver";
	[SerializeField] static bool verbose = false;

	public static void Save (object xmlSaver, string path)
	{
		#if !RELEASE_VERSION
		#endif
		//JSON
//		StreamWriter json = new StreamWriter (path + ".json", false);
//		string json_str = JsonUtility.ToJson (xmlSaver, true);
//		if (xmlSaver is TreeBehaviourSaver)
//			json_str = xmlSaver.ToString ();
//		json.WriteLine (json_str);
//		json.Close ();
		//XML
		path += ".xml";
		path = Path.Combine (Application.persistentDataPath, path);
		#if !RELEASE_VERSION
		if (verbose)
			Debug.Log ("Saving in\n" + path);
		#endif
		XmlSerializer serializer = new XmlSerializer (xmlSaver.GetType ());
		FileStream file = new FileStream (path, FileMode.Create);
		serializer.Serialize (file, xmlSaver);
		file.Close ();
	}

	public static void SaveJSON (object xmlSaver, string path)
	{
		path = Path.Combine (Application.persistentDataPath, path);
		#if !RELEASE_VERSION
		if (verbose)
			Debug.Log ("Saving in\n" + path);
		#endif
		//JSON
		StreamWriter json = new StreamWriter (path + ".json", false);
		string json_str = JsonUtility.ToJson (xmlSaver, true);
		json.WriteLine (json_str);
		json.Close ();
	}

	public static T Load<T> (string path)
	{
		path += ".xml";
		path = Path.Combine (Application.persistentDataPath, path);

		#if !RELEASE_VERSION
		if (verbose)
			Debug.Log ("Loading\n" + path);
		#endif
		XmlSerializer serializer = new XmlSerializer (typeof(T));
		FileStream stream = new FileStream (path, FileMode.Open);
		T output = (T)serializer.Deserialize (stream);
		stream.Close ();
		return output;
	}

	public static bool CheckIfFileExist (string path)
	{
		path = Path.Combine (Application.persistentDataPath, path);
		return File.Exists (path);
	}

	public static string GetFileData (string path)
	{
		path = Path.Combine (Application.persistentDataPath, path);
		return File.ReadAllText (path);
	}

	public static string GetDirectoryPath ()
	{
		return Application.persistentDataPath;
	}
}

/// <summary>
/// XML saver array.
/// </summary>
[System.Serializable][XmlRoot ("Info")]
public class TelemetriasArray:XMLSaver
{
	[XmlArray ("DataArray"),XmlArrayItem ("Data")]
	public List<TelemetriasXMLSaver> DataArray = new List<TelemetriasXMLSaver> ();

	public TelemetriasArray ()
	{
		Class = GetType ().ToString ();
	}

	/// <summary>
	/// Add the specified telemetria and avoid duplicate info by ID
	/// </summary>
	/// <param name="telemetria">Telemetria.</param>
	public void Add (TelemetriasXMLSaver telemetria)
	{
		bool reemplazar = false;
		for (int i = 0, DataArrayCount = DataArray.Count; i < DataArrayCount; i++) {
			if (DataArray [i].Id == telemetria.Id) {
				reemplazar = true;
				DataArray [i] = telemetria;
				break;
			}
		}
		if (!reemplazar)
			DataArray.Add (telemetria);
	}

	public TelemetriasXMLSaver Get (int index)
	{
		return (index >= DataArray.Count) ? null : DataArray [index];
	}

	public override string ToString ()
	{
		string output = string.Format ("[TelemetriasArray] Id:{0} Class:{1} ", Id, Class);
		for (int i = 0; i < DataArray.Count; i++) {
			output += "\n\t" + DataArray [i].ToString ();
		}
		return output;
	}
}

/// <summary>
/// Telemetrias XML saver.
/// </summary>
[System.Serializable][XmlRoot ("Data")]
public class TelemetriasXMLSaver:XMLSaver
{
	[XmlAttribute ("name")] public string Name = "";
	[XmlAttribute ("stars")] public int Stars = 0;
	[XmlAttribute ("unblock")] public bool Unblock = false;
	[XmlAttribute ("puntaje")] public int Puntaje = 0;
	[XmlIgnore] public string SavePath = "";

	public TelemetriasXMLSaver ()
	{
		#if !RELEASE_VERSION
		Debug.LogWarning ("No olvides usar Init");
		#endif
	}
	//
	public TelemetriasXMLSaver Init (int id, string name)
	{
		Class = typeof(TelemetriasXMLSaver).ToString ();
		Id = id;
		Name = name;
		SavePath = Path.Combine (Application.persistentDataPath, Class + "_" + Id);
		return this;
	}
	//
	public override string ToString ()
	{
		return string.Format (
			"[TelemetriasXMLSaver] " +
			"Name {0} id:{1} Class:{2} Stars:{3} Unblock:{4} Puntaje:{5}\n" +
			"{6}",
			Name, Id, Class, Stars, Unblock, Puntaje,
			SavePath
		);
	}
}

[System.Serializable][XmlRoot ("Data")]
public class Usuario:XMLSaver
{
	[XmlAttribute ("name")] public string Name = "";
	[XmlAttribute ("pass")] public string Pass = "";

	public Usuario ()
	{
		#if !RELEASE_VERSION
		Debug.LogWarning ("No olvides usar Init");
		#endif
	}

	public Usuario Init (string name, string pass)
	{
		Name = name;
		Pass = pass;
		return this;
	}

	//TODO override ToString();
}
