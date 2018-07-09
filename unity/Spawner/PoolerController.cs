using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrizEngine.Spawner
{
	public class PoolerController : MonoBehaviour
	{

		[SerializeField] SecuenciaModel[] secuencias = new SecuenciaModel[0];
		[SerializeField] Pool[] pools = new Pool[0];

		#region Class members

		#endregion

		#region Class Accessors

		public void SetSecuency (string[] secuency, int[] prob)
		{
			secuencias = new SecuenciaModel[secuency.Length];
			for (int i = 0, secuencyLength = secuency.Length; i < secuencyLength; i++) {
				string item = secuency [i];
				secuencias [i] = new SecuenciaModel (item, prob [i]);
			}
		}

		public GameObject Spawn ()
		{
			string secuencia = GetSecuencia ();
			GameObject obj = GetSpawn (secuencia);
			return obj;
		}

		public GameObject GetSpawn (string id)
		{
			GameObject item = null;
			foreach (Pool pool in pools) {
				if (pool.id == id) {
					item = pool.GetGameObject ();
					break;
				}
			}
			return item;
		}

		public string GetSecuencia ()
		{
			int sum = 0;
			foreach (SecuenciaModel item in secuencias) {
				sum += item.prob;
			}
			int selected = Random.Range (0, sum + 1);
			sum = 0;
			foreach (SecuenciaModel item in secuencias) {
				sum += item.prob;
				if (selected <= sum && item.prob != 0)
					return item.code;
			}
			return null;
		}

		public void SetActive (bool b)
		{
			foreach (var pool in pools) {
				pool.SetActive (b);
			}
		}

		public void Clear ()
		{
			foreach (var pool in pools) {
				pool.Clear ();
			}
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
	