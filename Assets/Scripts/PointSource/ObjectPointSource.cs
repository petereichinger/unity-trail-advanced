using System.Collections;
using UnityEngine;

namespace TrailAdvanced.PointSource {

	public class ObjectPointSource : AbstractPointSource {
		public Transform objectToFollow;

		protected override void Update() {
			base.Update();
			points.AddToBack(objectToFollow.position);
		}
	}
}