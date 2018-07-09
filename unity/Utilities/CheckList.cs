using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrizEngine.Utilities
{
	public class CheckList
	{
		#region Class members

		#endregion

		#region Class Accesors

		public static bool Check (object obj)
		{
			return(obj != null && obj.ToString () != "null");
		}

		#endregion

		#region MonoBehaviour overrides

		///Initialization
		//void Reset(){}
		//void Awake(){}
		//void OnEnable(){}
		//void Start () {}

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
