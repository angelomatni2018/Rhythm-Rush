using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelController : MonoBehaviour {

	public enum Difficulty {
		Easy,
		Normal,
		Hard
	};
	public static Difficulty levelDifficulty;
	public static float quarterPulse;
	public static float tileScale;
	public static Color defaultColor;

	public float debugPulse;
	float startTime;
	float timer;

	public int pulsesPerRowDissapear;
	public int pulsesPerClick;
	int disappearPulses;
	int clickPulses;

	Transform track;
	int rowIndex;

	public int debugDifficulty;

	private AudioSource source;
	public AudioClip clickSound;

	static List<float> pulseActivations;
	public static event EventHandler<PulseEventArgs> pulsed;

	void Awake() {
		track = GameObject.Find ("Track").transform;
		rowIndex = 0;
		timer = 0;
		defaultColor = Color.white;
		tileScale = 1;
		disappearPulses = 0;
		clickPulses = 0;
		levelDifficulty = (Difficulty)debugDifficulty;
		quarterPulse = debugPulse;
	
		source = GetComponent<AudioSource> ();
		startTime = Time.time;
		pulseActivations = new List<float> { debugPulse / 2, debugPulse };

		LevelController.pulsed += DisappearRow;
		LevelController.pulsed += PlayBeat;
	}

	void OnDestroy() {
		LevelController.pulsed -= DisappearRow;
		LevelController.pulsed -= PlayBeat;
	}
	
	void Update () {
		timer = Time.time - startTime;
		if (timer > pulseActivations [0]) {
			pulseActivations [0] += debugPulse / 2;
			pulsed (null, new PulseEventArgs (PulseEventArgs.PulseValue.Half));
		}
		if (timer > pulseActivations [1]) {
			pulseActivations [1] += debugPulse;
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

	void PlayBeat(object sender, PulseEventArgs pulseEvent) {
		if (pulseEvent.pulseValue == PulseEventArgs.PulseValue.Full)
			clickPulses++;
		if (clickPulses >= pulsesPerClick) {
			source.PlayOneShot (clickSound, 1F);
			clickPulses = 0;
		}
	}

	public static float GetPulseActivation(int index) {
		return pulseActivations[index];
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
