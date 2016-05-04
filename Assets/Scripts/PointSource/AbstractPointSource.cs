using UnityEngine;
using System.Collections;

namespace TrailAdvanced.PointSource {
	public abstract class AbstractPointSource : MonoBehaviour {
		public abstract Vector3[] GetPoints();
	}
}