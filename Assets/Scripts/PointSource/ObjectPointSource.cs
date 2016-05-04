using System.Collections;
using UnityEngine;

namespace TrailAdvanced.PointSource {

	public class ObjectPointSource : AbstractPointSource {
		public Transform objectToFollow;
		private Vector3 _lastPosition;

		protected override void Start() {
			base.Start();
			_lastPosition = objectToFollow.position;
		}

		protected override void Update() {
			base.Update();

			float distance = Vector3.SqrMagnitude(objectToFollow.position - _lastPosition);

			if (distance > 0.0001f) {
				points.AddToFront(objectToFollow.position);
			}
			_lastPosition = objectToFollow.position;
		}
	}
}
