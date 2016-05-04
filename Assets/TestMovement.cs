using System.Collections;
using UnityEngine;

public class TestMovement : MonoBehaviour {
	public float speed;
	private Transform _t;

	private void Start() {
		_t = transform;
	}

	// Update is called once per frame
	private void Update() {
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		_t.position += new Vector3(horizontal * speed * Time.deltaTime, vertical * speed * Time.deltaTime);
	}
}
