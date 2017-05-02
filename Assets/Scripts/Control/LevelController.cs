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

	public int pulsesPerRowDissapear;
	int disappearPulses;

	Transform track;
	int rowIndex;

	public static event EventHandler<PulseEventArgs> pulsed;

	void Awake() {
		track = GameObject.Find ("Track").transform;
		rowIndex = 0;
		timer = 0;
		defaultColor = Color.white;
		tileScale = 1;
		disappearPulses = 0;

		pulseActivations = new List<float> { 0, 0 };

		LevelController.pulsed += DisappearRow;
	}

	void OnDestroy() {
		LevelController.pulsed -= DisappearRow;
	}

	void Start () {
	}
	
	void Update () {
		timer += Time.deltaTime;
		if (timer % debugPulse < debugPulse / 2 && (timer - pulseActivations [0]) > debugPulse / 2) {
			//print ("Half pulse: " + timer);
			pulseActivations [0] = timer;
			pulsed (null, new PulseEventArgs (PulseEventArgs.PulseValue.Half));
		}
		if (timer % (2*debugPulse) < debugPulse && (timer - pulseActivations [1]) > debugPulse) {
			//print ("Full pulse: " + timer);
			pulseActivations [1] = timer;
			pulsed (null, new PulseEventArgs (PulseEventArgs.PulseValue.Full));
		}
	}

	void DisappearRow(object sender, PulseEventArgs pulseEvent) {
		if (pulseEvent.pulseValue == PulseEventArgs.PulseValue.Full)
			disappearPulses++;
		if (disappearPulses >= pulsesPerRowDissapear) {
			Transform child = track.GetChild (rowIndex);
			PlayerController.KillPlayersAt (child.transform.position.y);
			child.gameObject.SetActive (false);
			rowIndex++;
			disappearPulses = 0;
		}
	}
}

public class PulseEventArgs : EventArgs {
	public enum PulseValue {
		Half,
		Full
	};

	public PulseValue pulseValue;

	public PulseEventArgs(PulseValue val) {
		pulseValue = val;
	}
}
