using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class Avatar : Photon.PunBehaviour {

	private GameObject frisbee;
	private Transform right_hand;

	void Start () {
		foreach (Transform t in GetComponentsInChildren<Transform>()) {
			if (t.gameObject.name == "RightHandAnchor") {
				right_hand = t;
				break;
			}
		}
	}
	
	void Update () {
		// create a new frisbee in your right hand when index button released
		if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger) && !frisbee) {
			frisbee = PhotonNetwork.Instantiate("Frisbee", Vector3.zero, Quaternion.identity, 0);
			frisbee.GetComponent<Frisbee>().SetOwner(right_hand);
		}

		// release the frisbee when A button released
		if (OVRInput.GetUp(OVRInput.Button.One) && frisbee) {
			frisbee.GetComponent<Frisbee>().SetOwner(null);
			frisbee = null;
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.name == "Frisbee(Clone)" && !frisbee) {
			frisbee = other.gameObject;
			frisbee.GetComponent<Frisbee>().SetOwner(right_hand);
			frisbee.GetComponent<PhotonView>().RequestOwnership();
		}
	}
}
