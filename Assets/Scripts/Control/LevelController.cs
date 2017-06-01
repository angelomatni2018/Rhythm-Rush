using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelController : MonoBehaviour {

	public enum Difficulty {
		Easy,
		Normal,
		Hard
	};
	public static LevelController currentLevel;
	public static Difficulty levelDifficulty;
	public static float quarterPulse;
	public static float tileScale;
	public static Color defaultColor;

	public float debugPulse;
	public float levelTileScaling;
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

	GameObject barrier;

	void Awake() {
		currentLevel = this;

		track = GameObject.Find ("Track").transform;
		rowIndex = 0;
		timer = 0;
		tileScale = 1;
		disappearPulses = 0;
		clickPulses = 0;

		levelDifficulty = (Difficulty)debugDifficulty;
		defaultColor = Color.white;
		quarterPulse = debugPulse;
		tileScale = levelTileScaling;
	
		source = GetComponent<AudioSource> ();
		startTime = Time.time;
		pulseActivations = new List<float> { debugPulse / 2, debugPulse };

		LevelController.pulsed += DisappearRow;
		LevelController.pulsed += PlayBeat;
		FinishTile.finishedLevel += LoadNextLevel;

		BarrierTile.barrierDeath += KillPlayer;
		barrier = GameObject.Find ("BarrierCollider");
	}

	void OnDestroy() {
		LevelController.pulsed -= DisappearRow;
		LevelController.pulsed -= PlayBeat;
		FinishTile.finishedLevel -= LoadNextLevel;

		BarrierTile.barrierDeath -= KillPlayer;
	}

	public static float NextQuarterPulse() {
		return pulseActivations [1];
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

	void DisappearCurrentRow() {
		Transform child = track.GetChild (rowIndex);
		child.gameObject.SetActive (false);
		barrier.transform.Translate (new Vector3 (0, tileScale, 0));
		rowIndex++;
	}

	public void DisappearRowsTo(int index) {
		while (rowIndex < index) {
			DisappearCurrentRow ();
		}
	}

	void DisappearRow(object sender, PulseEventArgs pulseEvent) {
		if (pulseEvent.pulseValue == PulseEventArgs.PulseValue.Full)
			disappearPulses++;
		if (disappearPulses >= pulsesPerRowDissapear) {
			DisappearCurrentRow ();
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

	public void LoadNextLevel(object sender, EventArgs e) {
		SceneManager.LoadScene ("TestScene");
	}

	public void KillPlayer(object sender, EventArgs e) {
		SceneManager.LoadScene ("TestScene");
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
