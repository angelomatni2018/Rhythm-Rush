using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	Camera cam;
	Player currentPlayer;

	void Awake() {
		cam = GetComponent<Camera> ();
	}

	void Start () {
		currentPlayer = PlayerController.players [0];
	}
	
	// Update is called once per frame
	void Update () {
		cam.transform.position = new Vector3(cam.transform.position.x,currentPlayer.transform.position.y,cam.transform.position.z);
	}
}
