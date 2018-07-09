using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FrizEngine.Spawner
{
	[System.Serializable]
	public class Pool
	{
		public string id;
		public int weight;
		public GameObject prefab;
		public List<GameObject> pool;

		public Pool (string code, GameObject prefab, int weight)
		{
			this.id = code;
			this.prefab = prefab;
			this.weight = weight;
			pool = new List<GameObject> ();
		}

		public void Clean ()
		{
			pool.Clear ();
			pool = new List<GameObject> ();
		}

		public void Clear ()
		{
			foreach (GameObject item in pool) {
				GameObject.Destroy (item);
			}
			pool.Clear ();
		}

		public void SetActive (bool b)
		{
			foreach (GameObject item in pool) {
				if (item != null)
					item.SetActive (b);
			}
		}

		public GameObject GetGameObject ()
		{
			//si esta vacio lo crea
			if (pool == null)
				pool = new List<GameObject> ();
			//si no tiene nada le agrega un objeto
			if (pool.Count == 0) {
				pool.Add (GameObject.Instantiate (prefab));
				return pool [0];
			}
			/// si tiene un objecto inactivo, lo activa y lo devuelve
			/// si no, crea uno y lo devuelve
			foreach (GameObject item in pool) {
				if (item == null || item.ToString () == "null")
					continue;
				if (!item.activeInHierarchy) {
					item.SetActive (true);
					item.transform.SetParent (null);
					item.transform.localPosition = Vector3.zero;
					item.transform.position = prefab.transform.position;
					item.transform.rotation = prefab.transform.rotation;
					item.transform.localScale = prefab.transform.localScale;
					return item;
				}
			}
			pool.Add (GameObject.Instantiate (prefab));
			return pool [pool.Count - 1];

		}
	}
	//TODO a struct with prefab, id, prob and stack
	//TODO a loadScene function that change prefabs to select
}
	