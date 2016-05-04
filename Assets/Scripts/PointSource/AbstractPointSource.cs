using System.Collections;
using UnityEngine;

namespace TrailAdvanced.PointSource {

	public abstract class AbstractPointSource : MonoBehaviour {

		public abstract Vector3[] GetPoints();
	}
}