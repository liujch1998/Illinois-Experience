using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

[RequireComponent(typeof(AudioSource))]
public class Avatar : Photon.PunBehaviour {

	private GameObject frisbee;
	private Transform right_hand;

	public AudioClip audio_creat;
	public AudioClip audio_throw;
	public AudioClip audio_catch;

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
			AudioSource.PlayClipAtPoint(audio_creat, right_hand.position);
		}

		// release the frisbee when A button released
		if (OVRInput.GetUp(OVRInput.Button.One) && frisbee) {
			frisbee.GetComponent<Frisbee>().SetOwner(null);
			frisbee = null;
			AudioSource.PlayClipAtPoint(audio_throw, right_hand.position);
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.name == "Frisbee(Clone)" && !frisbee) {
			frisbee = other.gameObject;
			frisbee.GetComponent<Frisbee>().SetOwner(right_hand);
			frisbee.GetComponent<PhotonView>().RequestOwnership();
			AudioSource.PlayClipAtPoint(audio_catch, right_hand.position);
		}
	}
}
