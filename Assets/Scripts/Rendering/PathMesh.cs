using System.Collections;
using System.Collections.Generic;
using TrailAdvanced.PointSource;
using UnityEngine;

namespace TrailAdvanced.Construction {

	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(AbstractPointSource))]
	public class PathMesh : MonoBehaviour {
		public float width = 2f;
		public Vector3 normals;

		private AbstractPointSource _source;

		private void Start() {
			_source = GetComponent<AbstractPointSource>();
		}

		//		[ContextMenu("Generate Mesh")]
		public void Update() {
			normals = normals.normalized;

			var mesh = GenerateMesh(_source.GetPoints(), width, normals);

			var filter = GetComponent<MeshFilter>();
			filter.sharedMesh = mesh;
		}

		/// <summary>
		/// Generate the mesh for the trail.
		/// </summary>
		/// <param name="points">Points to use.</param>
		/// <param name="width">Width of the trail.</param>
		/// <param name="normals">Normals for the path.</param>
		/// <returns>A mesh for the path.</returns>
		public static Mesh GenerateMesh(Vector3[] points, float width, Vector3 normals) {
			if (points.Length < 2)
				return null;
			Mesh m = new Mesh();
			Vector3[] verts = new Vector3[points.Length * 2];
			Vector3[] norms = new Vector3[points.Length * 2];

			for (int i = 0; i < points.Length; i++) {
				Vector3 perpendicularDirection;
				if (i == 0) {
					perpendicularDirection = Vector3.Cross(points[i + 1] - points[i], normals).normalized;
				} else if (i == points.Length - 1) {
					perpendicularDirection = Vector3.Cross(points[i] - points[i - 1], normals).normalized;
				} else {
					perpendicularDirection = Vector3.Cross(points[i + 1] - points[i - 1], normals).normalized;
				}
				verts[i * 2] = points[i] + perpendicularDirection * width;
				norms[i * 2] = normals;
				verts[i * 2 + 1] = points[i] - perpendicularDirection * width;
				norms[i * 2 + 1] = normals;
			}

			m.vertices = verts;
			m.normals = norms;

			int[] tris = new int[(points.Length - 1) * 6];

			for (int i = 0; i < points.Length - 1; i++) {
				tris[i * 6] = i * 2;
				tris[i * 6 + 1] = i * 2 + 2;
				tris[i * 6 + 2] = i * 2 + 1;

				tris[i * 6 + 3] = i * 2 + 1;
				tris[i * 6 + 4] = i * 2 + 2;
				tris[i * 6 + 5] = i * 2 + 3;
			}

			m.triangles = tris;

			m.name = "pathMesh";
			m.RecalculateNormals();
			m.RecalculateBounds();
			m.Optimize();
			return m;
		}
	}
}