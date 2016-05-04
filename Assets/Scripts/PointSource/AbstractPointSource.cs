using Nito;
using System.Collections;
using UnityEngine;

namespace TrailAdvanced.PointSource {

	public abstract class AbstractPointSource : MonoBehaviour {
		public int maxPoints = 1000;

		protected Deque<Vector3> points;

		public Deque<Vector3> GetPoints() {
			return points;
		}

		protected virtual void Start() {
			points = new Deque<Vector3>(maxPoints);
		}
	}
}