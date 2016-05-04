using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrailAdvanced.PointSource {

	public class SinePointSource : AbstractPointSource {
		public float frequency = 1f;
		public float amplitude = 1f;
		public int quality = 1;

		private List<Vector3> points;
		private float _time;

		private void Start() {
			points = new List<Vector3>();
		}

		private void Update() {
			float newTime = _time + Time.deltaTime;
			for (int i = 0; i <= quality; i++) {
				float t = Mathf.Lerp(_time, newTime, (float)i / quality);
				points.Add(new Vector3(t, amplitude * Mathf.Sin(frequency * t), 0));
			}
			_time = newTime;
		}

		public override Vector3[] GetPoints() {
			return points.ToArray();
		}
	}
}