using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientWorldController : MonoBehaviour {

	public GameObject prefab_avatar;

	// Use this for initialization
	void Start () {
		string server_ip = "";
		int server_port = 8888;
		Network.Connect(server_ip, server_port);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
