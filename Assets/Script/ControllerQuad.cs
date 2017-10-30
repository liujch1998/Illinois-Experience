using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class ControllerQuad : Photon.PunBehaviour {

	public Transform avatar_prefab;
	private GameObject avatar;
	private GameObject avatar_obs;
	private Transform center_eye;

	private int playerIndex;
	private const float RADIUS = 5.0f;
	private const float ANGLE =  0.4f * (2f * Mathf.PI);

	void Start () {
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings ("0.1");
	}

	void Update () {
		// sync Avatar_obs position with camera
		if (avatar_obs && center_eye) {
			avatar_obs.transform.position = center_eye.position;
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
		Vector3 position = new Vector3(RADIUS * Mathf.Cos(ANGLE * playerIndex), 1f, RADIUS * Mathf.Sin(ANGLE * playerIndex));
		Quaternion rotation = Quaternion.Euler(0f, ANGLE * playerIndex, 0f);
		avatar = Instantiate(avatar_prefab, position, rotation).gameObject;
		avatar_obs = PhotonNetwork.Instantiate("Avatar_obs", position, rotation, 0);
		foreach (Transform t in avatar.GetComponentsInChildren<Transform>()) {
			if (t.gameObject.name == "CenterEyeAnchor") {
				center_eye = t;
				break;
			}
		}
	}
}
