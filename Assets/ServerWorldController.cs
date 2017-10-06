using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerWorldController : MonoBehaviour {

	public GameObject prefab_frisbee;

	// Use this for initialization
	void Start () {
		int client_max = 8;
		int server_port = 8888;
		bool use_nat = false;
		Network.InitializeServer(client_max, server_port, use_nat);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
