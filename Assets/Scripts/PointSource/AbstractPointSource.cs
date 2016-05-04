using Nito;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;

namespace TrailAdvanced.PointSource {

	public abstract class AbstractPointSource : MonoBehaviour {
		public int maxPoints = 1000;

		protected Deque<Vector3> points;
		protected int minimumFreeCapacity = 10;

		public Deque<Vector3> GetPoints() {
			return points;
		}

		protected virtual void Start() {
			points = new Deque<Vector3>(maxPoints);
		}

		protected virtual void Update() {
			int count = points.Count - (points.Capacity - minimumFreeCapacity);
			if (count > 0) {
				points.RemoveRange(points.Count - count, count);
			}
		}
	}
}
