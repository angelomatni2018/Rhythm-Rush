using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	static List<Player> players;

	static Dictionary<KeyCode,Vector3> key_directions = new Dictionary<KeyCode, Vector3>
	{
		{KeyCode.W,new Vector3(0,1,0)},
		{KeyCode.S,new Vector3(0,-1,0)},
		{KeyCode.D,new Vector3(1,0,0)},
		{KeyCode.A,new Vector3(-1,0,0)}
	};

	void Awake () {
		Transform playerHolder = GameObject.Find ("Players").transform;
		players = new List<Player> ();
		for (int i = 0; i < playerHolder.childCount; i++) {
			players.Add(playerHolder.GetChild (i).GetComponent<Player> ());
		}
	}
	
	void Update () {
		Vector3 move = new Vector3();
		foreach (KeyValuePair<KeyCode,Vector3> pair in key_directions) {
			if (Input.GetKeyDown (pair.Key))
				move = pair.Value * LevelController.tileScale;
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
