using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class Frisbee : Photon.PunBehaviour {

	public Transform frisbee_obs_prefab;
	private Transform frisbee_obs;

	private Vector3 position;
	private Vector3 velocity;
	private Transform owner;

	void Start () {
		frisbee_obs = (Transform)Instantiate(frisbee_obs_prefab, Vector3.zero, Quaternion.identity);
		position = transform.position;
	}

	void Update () {
		if (GetComponent<PhotonView>().isMine) {
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
				transform.Rotate(Vector3.up, 900f * Time.deltaTime);
			}
		}

		UpdateFrisbeeObs();
	}

	void UpdateFrisbeeObs () {
		Transform player = GameObject.Find("CenterEyeAnchor").transform;
		if (owner || transform.position.y + 0.2f >= player.position.y) {
			frisbee_obs.position = transform.position;
			frisbee_obs.rotation = transform.rotation;
			frisbee_obs.localScale = transform.localScale;
		} else {
			float R = 40f;
			float H_v = player.position.y;
			float H_o = transform.position.y;
			float dx = transform.position.x - player.position.x;
			float dz = transform.position.z - player.position.z;
			float r = Mathf.Sqrt(dx * dx + dz * dz);
			float theta = Mathf.Atan(r / (H_v - H_o));
			float r_ = R * Mathf.Sin (theta) * (H_v - H_o) / H_v;

			frisbee_obs.position = new Vector3(
				r_ / r * dx + player.position.x, 
				H_v - R * Mathf.Cos (theta) * (H_v - H_o) / H_v, 
				r_ / r * dz + player.position.z);
			frisbee_obs.rotation = transform.rotation;
			frisbee_obs.localScale = transform.localScale * R * Mathf.Cos (theta) / H_v;
		}
	}

	public void SetOwner (Transform owner) {
		this.owner = owner;
	}

	void OnDestroy () {
		Destroy(frisbee_obs.gameObject); 
	}
}
