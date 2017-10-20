using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class ControllerQuad : Photon.PunBehaviour {

	private GameObject avatar;
	private int playerIndex;
	private const float RADIUS = 5.0f;
	private const float ANGLE =  0.4f * (2f * Mathf.PI);
	private GameObject frisbee;
	private bool frisbee_ismine = false;
	private Vector3 frisbee_position;
	private Vector3 frisbee_velocity;

	void Start () {
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings ("0.1");
	}
	
	void Update () {
		// create a new frisbee in your right hand when index button released
		if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger) && frisbee_ismine == false) {
			frisbee_ismine = true;
			frisbee = PhotonNetwork.Instantiate("Frisbee", Vector3.zero, Quaternion.identity, 0);
			foreach (Transform t in avatar.GetComponentsInChildren<Transform>()) {
				if (t.gameObject.name == "RightHandAnchor") {
					frisbee.transform.parent = t;
					frisbee.transform.localPosition = Vector3.zero;
					frisbee.transform.localRotation = Quaternion.identity;
					break;
				}
			}
			frisbee_position = frisbee.transform.position;
		}

		// record velocity of frisbee
		if (frisbee_ismine) {
			frisbee_velocity = (frisbee.transform.position - frisbee_position) / Time.deltaTime;
			frisbee_position = frisbee.transform.position;
		} else if (frisbee != null) {
			frisbee_velocity += new Vector3(0f, -1f * Time.deltaTime, 0f);  // gravity acceleration
			frisbee.transform.position += frisbee_velocity * Time.deltaTime;
			if (frisbee.transform.position.y < 0f) {
				frisbee_velocity = Vector3.zero;
			}
		}

		// release the frisbee when A button released
		if (OVRInput.GetUp(OVRInput.Button.One) && frisbee_ismine == true) {
			frisbee_ismine = false;
			frisbee.transform.parent = null;
		}

		// sync Avatar_obs position with camera
		if (avatar) {
			foreach (Transform t in avatar.GetComponentsInChildren<Transform>()) {
				if (t.gameObject.name == "Avatar_obs") {
					foreach (Transform s in avatar.GetComponentsInChildren<Transform>()) {
						if (s.gameObject.name == "CenterEyeAnchor") {
							t.position = s.position;
						}
					}
				}
			}
		}
	}

	public override void OnJoinedLobby () {
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.IsVisible = true;
		roomOptions.MaxPlayers = 5;
		PhotonNetwork.JoinOrCreateRoom("Quad", roomOptions, TypedLobby.Default);
	}

	public override void OnJoinedRoom () {
		playerIndex = PhotonNetwork.playerList.Length - 1;
		Vector3 position = new Vector3(RADIUS * Mathf.Cos(ANGLE * playerIndex), 0f, RADIUS * Mathf.Sin(ANGLE * playerIndex));
		Quaternion rotation = Quaternion.Euler(0f, ANGLE * playerIndex, 0f);
		avatar = PhotonNetwork.Instantiate("Avatar", position, rotation, 0);
	}
}
