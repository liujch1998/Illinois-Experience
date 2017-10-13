using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class ControllerQuad : Photon.PunBehaviour {

	private GameObject avatar;
	private int playerIndex;
	private const float RADIUS = 5.0f;
	private const float ANGLE =  0.4f * (2f * Mathf.PI);

	void Start () {
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings ("0.1");
	}
	
	void Update () {
		if (avatar) {
			GameObject.Find("CanvasQuad").transform.position = avatar.transform.position;
		}
	}

	public override void OnJoinedLobby () {
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.IsVisible = true;
		roomOptions.MaxPlayers = 5;
		PhotonNetwork.JoinOrCreateRoom("Quad", roomOptions, TypedLobby.Default);
	/*	PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonJoinFailed () {
		PhotonNetwork.CreateRoom("Quad");
		PhotonNetwork.CreateRoom(null);
	*/
	}

	public override void OnJoinedRoom () {
		playerIndex = PhotonNetwork.playerList.Length - 1;
		Vector3 position = new Vector3(RADIUS * Mathf.Cos(ANGLE * playerIndex), 0f, RADIUS * Mathf.Sin(ANGLE * playerIndex));
		Quaternion rotation = Quaternion.Euler(0f, ANGLE * playerIndex, 0f);
		avatar = PhotonNetwork.Instantiate("Avatar", position, rotation, 0);
	//	GameObject.FindGameObjectWithTag("MainCamera").transform.SetParent(avatar.transform);
	//	GameObject.Find("CanvasQuad").transform.SetParent(avatar.transform);
	}
}
