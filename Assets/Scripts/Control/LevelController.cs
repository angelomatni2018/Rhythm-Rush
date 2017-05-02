using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelController : MonoBehaviour {

	public static float tileScale;
	public static Color defaultColor;

	public float debugPulse;
	float timer;
	List<float> pulseActivations;

	Transform track;
	int rowIndex;

	public static event EventHandler<PulseEventArgs> pulsed;

	void Awake() {
		track = GameObject.Find ("Track").transform;
		rowIndex = 0;
		timer = 0;
		defaultColor = Color.white;
		tileScale = 1;

		pulseActivations = new List<float> { 0, 0 };
	}

	void Start () {
	}
	
	void Update () {
		timer += Time.deltaTime;
		if (timer % debugPulse < debugPulse / 2 && (timer - pulseActivations [0]) > debugPulse / 2) {
			print ("Half pulse: " + timer);
			pulseActivations [0] = timer;
			pulsed (null, new PulseEventArgs (.25f));
		}
		if (timer % (2*debugPulse) < debugPulse && (timer - pulseActivations [1]) > debugPulse) {
			print ("Full pulse: " + timer);
			pulseActivations [1] = timer;
			pulsed (null, new PulseEventArgs (.5f));
			DisappearRow ();
		}
	}

	void DisappearRow() {
		Transform child = track.GetChild (rowIndex);
		PlayerController.KillPlayersAt (child.transform.position.y);
		child.gameObject.SetActive (false);
		rowIndex++;
	}
}

public class PulseEventArgs : EventArgs {
	public float pulseValue;

	public PulseEventArgs(float val) {
		pulseValue = val;
	}
}
