using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class ControllerQuad : Photon.PunBehaviour {

	private GameObject avatar;

	void Start () {
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings ("0.1");
	}
	
	void Update () {
		
	}

	public override void OnJoinedLobby () {
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed () {
		Debug.Log("Can't join random room!");
		PhotonNetwork.CreateRoom(null);
	}

	public override void OnJoinedRoom () {
		avatar = PhotonNetwork.Instantiate("Avatar", Vector3.zero, Quaternion.identity, 0);
		GameObject.FindGameObjectWithTag("MainCamera").transform.SetParent(avatar.transform);
		GameObject.Find("CanvasQuad").transform.SetParent(avatar.transform);
	}
}
