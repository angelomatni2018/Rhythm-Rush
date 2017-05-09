using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	float lastPulse;

	// Determines how close you have to input to the beat of the music
	public float[] pulseThreshold;

	public static List<Player> players;

	static Dictionary<KeyCode,Vector3> key_directions = new Dictionary<KeyCode, Vector3>
	{
		{KeyCode.W,new Vector3(0,1,0)},
		{KeyCode.S,new Vector3(0,-1,0)},
		{KeyCode.D,new Vector3(1,0,0)},
		{KeyCode.A,new Vector3(-1,0,0)}
	};

	internal void Awake () {
		Transform playerHolder = GameObject.Find ("Players").transform;
		players = new List<Player> ();
		for (int i = 0; i < playerHolder.childCount; i++) {
			players.Add(playerHolder.GetChild (i).GetComponent<Player> ());
		}

		LevelController.pulsed += ReceivePulse;
	}

	internal void OnDestroy() {
		LevelController.pulsed -= ReceivePulse;
	}

	void Update () {
		Vector3 move = new Vector3();
		foreach (KeyValuePair<KeyCode,Vector3> pair in key_directions) {
			if (Input.GetKeyDown (pair.Key)) {
				move = ScaledMove(pair.Value);
			}
		}
		for (int i = 0; i < players.Count; i++) {
			players [i].transform.Translate (move);
		}
	}

	Vector3 ScaledMove(Vector3 dir) {
		Vector3 maxMove = dir * LevelController.tileScale;
		float modifier = Mathf.Exp (-(Time.time - lastPulse) * pulseThreshold[(int)LevelController.levelDifficulty]);
		return modifier * maxMove;
	}

	public static void KillPlayersAt(float height) {
		for (int i = 0; i < players.Count; i++) {
			if (players [i].transform.position.y - height < .001f)
				players [i].Kill ();
		}
	}

	public void ReceivePulse(object sender, PulseEventArgs pulseEvent) {
		lastPulse = Time.time;
	}
}
