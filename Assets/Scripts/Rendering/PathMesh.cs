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
		public float width = 2f;
		public Vector3 normals;

		private MeshFilter _filter;
		private AbstractPointSource _source;

		private void Start() {
			_filter = GetComponent<MeshFilter>();
			_source = GetComponent<AbstractPointSource>();
		}

		//		[ContextMenu("Generate Mesh")]
		public void Update() {
			Destroy(_filter.sharedMesh);
			_filter.sharedMesh = GenerateMesh(_source.GetPoints(), width, normals);
		}

		/// <summary>
		/// Generate the mesh for the trail.
		/// </summary>
		/// <param name="points">Points to use.</param>
		/// <param name="width">Width of the trail.</param>
		/// <param name="normals">Normals for the path.</param>
		/// <returns>A mesh for the path.</returns>
		public static Mesh GenerateMesh(Deque<Vector3> points, float width, Vector3 normals) {
			if (points.Count < 2)
				return null;
			Mesh m = new Mesh();
			Vector3[] verts = new Vector3[points.Count * 2];
			Vector3[] norms = new Vector3[points.Count * 2];
			Vector2[] uvs = new Vector2[points.Count * 2];
			for (int i = 0; i < points.Count; i++) {
				Vector3 perpendicularDirection;
				if (i == 0) {
					perpendicularDirection = Vector3.Cross(points[i + 1] - points[i], normals).normalized;
				} else if (i == points.Count - 1) {
					perpendicularDirection = Vector3.Cross(points[i] - points[i - 1], normals).normalized;
				} else {
					perpendicularDirection = Vector3.Cross(points[i + 1] - points[i - 1], normals).normalized;
				}
				verts[i * 2] = points[i] + perpendicularDirection * width;
				norms[i * 2] = normals;
				verts[i * 2 + 1] = points[i] - perpendicularDirection * width;
				norms[i * 2 + 1] = normals;
				uvs[i * 2] = new Vector2(i / (points.Count - 1f), 1);
				uvs[i * 2 + 1] = new Vector2(i / (points.Count - 1f), 0);
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

			//			m.name = "pathMesh";
			//			m.RecalculateNormals();
			//			m.RecalculateBounds();
			//			m.Optimize();
			return m;
		}
	}
}