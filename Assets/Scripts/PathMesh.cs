using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PathMesh : MonoBehaviour {
	public float width = 2f;
	public Vector3 normals;

	[ContextMenu("Generate Mesh")]
	public void GenerateMesh() {
		normals = normals.normalized;
		var points = new List<Vector3>();
		foreach (Transform child in transform) {
			points.Add(child.position);
		}

		var mesh = GenerateMesh(points.ToArray(), width, normals);

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
			Debug.Log(perpendicularDirection);
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
		for (int i = 0; i < verts.Length; i++) {
			Debug.LogWarning(verts[i]);
		}
		for (int i = 0; i < tris.Length; i++) {
			Debug.Log(tris[i]);
		}

		m.triangles = tris;

		m.name = "pathMesh";
		//		m.RecalculateNormals();
		//		m.RecalculateBounds();
		//		m.Optimize();
		return m;
	}
}