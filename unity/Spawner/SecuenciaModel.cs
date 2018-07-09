using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrizEngine.Spawner
{

	[System.Serializable]
	public struct SecuenciaModel
	{
		public string code;
		public int prob;

		public SecuenciaModel (string code, int prob)
		{
			this.code = code;
			this.prob = prob;
		}
	}
}
	