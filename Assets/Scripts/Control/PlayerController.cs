using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	static List<Player> players;

	void Awake () {
		Transform playerHolder = GameObject.Find ("Players").transform;
		players = new List<Player> ();
		for (int i = 0; i < playerHolder.childCount; i++) {
			players.Add(playerHolder.GetChild (i).GetComponent<Player> ());
		}
	}
	
	void Update () {
		Vector3 move = new Vector3();
		if (Input.GetKeyDown (KeyCode.W)) {
			move = new Vector3 (0, LevelController.tileScale, 0);
		} else if (Input.GetKeyDown (KeyCode.S)) {
			move = new Vector3 (0, -LevelController.tileScale, 0);
		} else if (Input.GetKeyDown (KeyCode.D)) {
			move = new Vector3 (LevelController.tileScale, 0, 0);
		} else if (Input.GetKeyDown (KeyCode.A)) {
			move = new Vector3 (-LevelController.tileScale, 0, 0);
		}
		for (int i = 0; i < players.Count; i++) {
			players [i].transform.Translate (move);
		}
	}

	public static void KillPlayersAt(float height) {
		for (int i = 0; i < players.Count; i++) {
			if (players [i].transform.position.y - height < .001f)
				players [i].Kill ();
		}
	}
}
