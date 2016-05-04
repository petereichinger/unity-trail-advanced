using Nito;
using System.Collections;
using System.Collections.Generic;
using TrailAdvanced.PointSource;
using UnityEngine;

namespace TrailAdvanced.Construction {

	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(AbstractPointSource))]
	public class PathMesh : MonoBehaviour {
		public Gradient gradient;

		public float width = 2f;
		public Vector3 normals;

		private MeshFilter _filter;
		private AbstractPointSource _source;

		private void Start() {
			_filter = GetComponent<MeshFilter>();
			_source = GetComponent<AbstractPointSource>();
		}

		public void Update() {
			Destroy(_filter.sharedMesh);
			_filter.sharedMesh = GenerateMesh(_source.GetPoints(), width, normals, gradient);
		}

		/// <summary>
		/// Generate the mesh for the trail.
		/// </summary>
		/// <param name="points">Points to use.</param>
		/// <param name="width">Width of the trail.</param>
		/// <param name="normals">Normals for the path.</param>
		/// <returns>A mesh for the path.</returns>
		public static Mesh GenerateMesh(Deque<Vector3> points, float width, Vector3 normals, Gradient gradient = null) {
			if (points.Count < 2)
				return null;
			Mesh m = new Mesh();
			var vertexCount = points.Count * 2;
			Vector3[] verts = new Vector3[vertexCount];
			Vector3[] norms = new Vector3[vertexCount];
			Vector2[] uvs = new Vector2[vertexCount];
			Color32[] colors = new Color32[vertexCount];
			for (int i = 0; i < points.Count; i++) {
				var percentage = i / (points.Count - 1f);

				Vector3 perpendicularDirection;
				if (i == 0) {
					perpendicularDirection = Vector3.Cross(points[i + 1] - points[i], normals).normalized;
				} else if (i == points.Count - 1) {
					perpendicularDirection = Vector3.Cross(points[i] - points[i - 1], normals).normalized;
				} else {
					perpendicularDirection = Vector3.Cross(points[i + 1] - points[i - 1], normals).normalized;
				}
				var vertexIndex = i * 2;
				verts[vertexIndex] = points[i] + perpendicularDirection * width;
				norms[vertexIndex] = normals;
				uvs[vertexIndex] = new Vector2(percentage, 0);
				verts[vertexIndex + 1] = points[i] - perpendicularDirection * width;
				norms[vertexIndex + 1] = normals;
				uvs[vertexIndex + 1] = new Vector2(percentage, 1);

				if (gradient != null) {
					colors[vertexIndex] = colors[vertexIndex + 1] = gradient.Evaluate(percentage);
				}
			}
			int[] tris = new int[(points.Count - 1) * 6];

			for (int i = 0; i < points.Count - 1; i++) {
				tris[i * 6] = i * 2;
				tris[i * 6 + 1] = i * 2 + 2;
				tris[i * 6 + 2] = i * 2 + 1;

				tris[i * 6 + 3] = i * 2 + 1;
				tris[i * 6 + 4] = i * 2 + 2;
				tris[i * 6 + 5] = i * 2 + 3;
			}

			m.vertices = verts;
			m.normals = norms;
			m.uv = uvs;
			m.triangles = tris;
			if (gradient != null) {
				m.colors32 = colors;
			}
			return m;
		}
	}
}
