using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class Frisbee : Photon.PunBehaviour {

	private Vector3 position;
	private Vector3 velocity;
	private Transform owner;

	void Start () {
		position = transform.position;
	}

	void Update () {
		if (GetComponent<PhotonView>().isMine == false) return;
		if (owner) {
			transform.position = owner.position;
			transform.rotation = owner.rotation;
			velocity = (transform.position - position) / Time.deltaTime;
			position = transform.position;
		} else {
			velocity += new Vector3(0f, -1f * Time.deltaTime, 0f);  // gravity acceleration
			position += velocity * Time.deltaTime;
			transform.position = position;
			if (position.y <= 0f && velocity.y <= 0f) {  // hit ground
				velocity.y *= -0.5f;  // bounce back
				velocity.x *= 0.5f;  // slow down
				velocity.z *= 0.5f;  // slow down
				if (velocity.y < 0.01f) PhotonNetwork.Destroy(gameObject);
			}
			transform.Rotate(Vector3.up, 1800f * Time.deltaTime);
		}
	}

	public void SetOwner (Transform owner) {
		this.owner = owner;
	}
}
