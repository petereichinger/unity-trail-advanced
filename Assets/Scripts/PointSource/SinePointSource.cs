using Nito;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrailAdvanced.PointSource {

	public class SinePointSource : AbstractPointSource {
		public float frequency = 1f;
		public float amplitude = 1f;
		public int quality = 1;

		private float _time;

		protected override void Update() {
			base.Update();
			float newTime = _time + Time.deltaTime;
			while (points.Count >= maxPoints + quality) {
				points.RemoveFromFront();
			}
			for (int i = 0; i <= quality; i++) {
				float t = Mathf.Lerp(_time, newTime, (float)i / quality);
				points.AddToBack(new Vector3(t, amplitude * Mathf.Sin(frequency * t), 0));
			}

			_time = newTime;
		}
	}
}
