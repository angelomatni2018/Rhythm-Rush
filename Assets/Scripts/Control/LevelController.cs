using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class LevelController : MonoBehaviour {

	public enum Difficulty {
		Easy,
		Normal,
		Hard
	};
	private bool first_click = false;
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
	EscapeMenu escapeMenu;
	DeathMenu deathMenu;
	bool gameOver;

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

		GameObject menu = Resources.Load ("UI/EscapeMenu") as GameObject;

		escapeMenu = GameObject.Instantiate (menu).GetComponent<EscapeMenu> ();
		escapeMenu.gameObject.SetActive (false);
		menu = Resources.Load ("UI/DeathMenu") as GameObject;
		deathMenu = GameObject.Instantiate (menu).GetComponent<DeathMenu> ();
		deathMenu.gameObject.SetActive (false);
		gameOver = false;
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
		if (gameOver)
			return;
		timer = Time.time - startTime;
		if (timer > pulseActivations [0]) {
			pulseActivations [0] += debugPulse / 2;
			pulsed (null, new PulseEventArgs (PulseEventArgs.PulseValue.Half));
		}
		if (timer > pulseActivations [1]) {
			pulseActivations [1] += debugPulse;
			pulsed (null, new PulseEventArgs (PulseEventArgs.PulseValue.Full));
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			escapeMenu.gameObject.SetActive (!escapeMenu.gameObject.activeInHierarchy);
		} 
	}

	void DisappearCurrentRow() {
		Transform child = track.GetChild (rowIndex);
		if (child.gameObject.activeInHierarchy) {
			child.gameObject.SetActive (false);
			//barrier.transform.Translate (new Vector3 (0, tileScale, 0));
			barrier.transform.position = new Vector3 (0, child.transform.position.y, 0);
			rowIndex++;
		}
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
		if (pulseEvent.pulseValue == PulseEventArgs.PulseValue.Full) {
			clickPulses++;
			if (!first_click) {
				source.PlayOneShot (clickSound, 1F);
				first_click = true;
			}
		}
		if (clickPulses == pulsesPerClick) {
			source.PlayOneShot (clickSound, 1F);
			clickPulses = 0;
		}
	}

	public void LoadNextLevel(object sender, EventArgs e) {
		string sceneName = SceneManager.GetActiveScene ().name;
		SceneManager.LoadScene (GameController.GetNextSceneName(sceneName));
	}

	public void LoadMenu() {
		SceneManager.LoadScene (GameController.MainMenuScene);
	}

	public void ReloadLevel() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void KillPlayer(object sender, KillPlayerEventArgs e) {
		deathMenu.deathTextField.text = e.deathText;
		deathMenu.gameObject.SetActive (true);
		GameObject player = GameObject.Find ("Player");
		player.GetComponent<PlayerController> ().KillPlayer ();
		gameOver = true;
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

public class KillPlayerEventArgs : EventArgs {
	public string deathText;

	public KillPlayerEventArgs(string text) {
		deathText = text;
	}
}