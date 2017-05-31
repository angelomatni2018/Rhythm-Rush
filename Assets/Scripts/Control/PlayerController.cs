using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour {

	public enum PlayerState { Dead, Stunned, Normal };

	public Rigidbody2D rb2d;
	public float speed;
	private Vector2 offset;

	float input_buffer_threshold;
	float input_accuracy_threshold;
	KeyCode last_input;
	float last_input_time;
	int current_scale;

	private Vector3[] targetPositions;
	public Transform targetPrefab;
	private Transform[] targets;

	// Determines how close you have to input to the beat of the music
	public float pulseThreshold;

	float startTime;
	float timer;

	float stunnedUntil;
	public Color stunnedColor;
	Color normalColor;

	public int numPulsesPerInput = 4;
	public PulseEventArgs.PulseValue pulseToggledAt = PulseEventArgs.PulseValue.Half;

	static KeyCode[] input_keys = new KeyCode[4]
	{
		KeyCode.W,KeyCode.S,KeyCode.D,KeyCode.A
	};
		
	static Dictionary<KeyCode,Vector3> key_directions = new Dictionary<KeyCode, Vector3>
	{
		{KeyCode.W,new Vector3(0,1,0)},
		{KeyCode.S,new Vector3(0,-1,0)},
		{KeyCode.D,new Vector3(1,0,0)},
		{KeyCode.A,new Vector3(-1,0,0)}
	};

	void InitTargetHighlighting() {
		targets = new Transform[4];
		targetPositions = new Vector3[4];
		for (int i = 0; i < 4; i++) {
			targetPositions [i] = transform.position + current_scale * key_directions [input_keys[i]];
			targets [i] = Instantiate (targetPrefab, targetPositions [i], Quaternion.identity);
		}
	}

	void Start () {
		input_buffer_threshold = LevelController.quarterPulse * 2 * pulseThreshold;
		input_accuracy_threshold = LevelController.quarterPulse * pulseThreshold;
		current_scale = 1;

		startTime = Time.time;
		timer = 0;
		stunnedUntil = 0;
		last_input_time = 0;
		last_input = KeyCode.None;

		normalColor = GetComponent<SpriteRenderer> ().color;

		InitTargetHighlighting ();
	}

	internal void Awake () {
		LevelController.pulsed += ReceivePulse;
	}

	internal void OnDestroy() {
		LevelController.pulsed -= ReceivePulse;
	}

	void OnCollisionEnter(Collision collision) {
		//if (collision.collider.tag == "Wall")
		//	last_input = Direction.none;
	}

	bool PlayerAtTilePos(Vector3 target) {
		//print (Vector3.Magnitude (target - transform.position));
		//print (.05 * LevelController.tileScale);
		return Vector3.Magnitude(target - transform.position) < .05 * LevelController.tileScale;
	}

	// Deprecated: Physics was causing clipping, so instead movement is now instantaneous
	// See SnapToNextTile
	/*void MoveToNextTile(int distance, float speed) {
		rb2d.velocity = current_scale * key_directions [last_input] * speed;
		target = transform.position + current_scale * distance * key_directions [last_input];
	}*/

	bool SnapToNextTile(int distance) {

		Func<int, Vector2> PosAt = i => (Vector2)(transform.position + i * key_directions [last_input]); 

		bool stun = false;
		Vector2 next_pos = (Vector2)transform.position;
		for (int i = 1; i <= current_scale * distance; i++) {
			next_pos = PosAt(i);
			//print (transform.position + "  " + next_pos + "  " + Physics2D.OverlapPoint (next_pos, LayerMask.GetMask(new string[] {"Barrier"})));
			Collider2D barrier = Physics2D.OverlapPoint (next_pos, LayerMask.GetMask(new string[] {"Barrier"}));
			if (barrier != null) {
				next_pos = PosAt(i - 1);
				stun = true;
				break;
			}
			Collider2D tileCol = Physics2D.OverlapPoint (next_pos, LayerMask.GetMask(new string[] {"InteractiveTile"}));
			if (tileCol != null) {
				Tile tile = tileCol.GetComponent<Tile> ();
				tile.AffectPlayer (this);
				if (!tile.PlayerContinueMove ()) {
					break;
				}
			}
		}
		//print (next_pos + "  " + stun);
		//rb2d.MovePosition (next_pos);
		transform.position = next_pos;
		return stun;
	}

	// Stunned until prevents inputs from being read until timer is past value
	// Value is therefore last stunned beat + input accuracy so next input can be received next beat
	void StunFor(int pulses) {
		stunnedUntil = LevelController.NextQuarterPulse () + (pulses - 1) * LevelController.quarterPulse - input_accuracy_threshold;
		//print ("Stunned from : " + timer + "  And until: " + stunnedUntil);
	}

	void Update() {
		if (timer > stunnedUntil) {
			GetComponent<SpriteRenderer> ().color = normalColor;
		} else {
			GetComponent<SpriteRenderer> ().color = stunnedColor;
		}
	}

	void FixedUpdate () {
		timer = Time.time - startTime;
		/*if (PlayerAtTilePos (target)) {
			rb2d.velocity = Vector3.zero;
		}*/
		if (timer > stunnedUntil) {
			if (timer - last_input_time > input_buffer_threshold) {
				foreach (KeyValuePair<KeyCode,Vector3> pair in key_directions) {
					if (Input.GetKeyDown (pair.Key)) {
						last_input = pair.Key;
						last_input_time = timer;
					}
				}
			}
			if (last_input != KeyCode.None)
				SetTarget ();
		} else {
			//print (stunnedUntil + "  " + timer);
		}
	}

	int GetDistance() {
		return 1;
	}

	public void KillPlayersAt(float height) {
		GameObject.Destroy (gameObject);
	}

	public void ReceivePulse(object sender, PulseEventArgs pulseEvent) {
	}

	private void SetTarget() {
		int distance = GetDistance();
		speed = 2f * distance / (LevelController.quarterPulse);

		float accuracy = Mathf.Abs (LevelController.NextQuarterPulse() - last_input_time);
		if (accuracy < input_accuracy_threshold || accuracy > LevelController.quarterPulse - input_accuracy_threshold) {
			//MoveToNextTile (distance, speed);
			bool stun = SnapToNextTile(distance);
			if (stun) {
				//print ("Hit something! " + accuracy + "  " + current_scale);
				StunFor (1);
				current_scale = Mathf.Max (1, current_scale - 1);
			} else {
				//print ("Correct! " + accuracy + "  " + current_scale);
				// Upgrade speed factor
				current_scale = Mathf.Min (3, current_scale + 1);
			}
		} else {
			// Downgrade speed factor
			if (current_scale == 1) {
				//Punish player
				StunFor(1);
			} else {
				current_scale--;
			}
			//print ("Wrong! " + accuracy + "  " + current_scale);
		}

		ClipToGrid ();
		UpdateTargetHighlighting ();
		last_input = KeyCode.None;
	}

	void UpdateTargetHighlighting() {
		for (int i = 0; i < 4; i++) {
			targetPositions [i] = transform.position + current_scale * key_directions [input_keys[i]];
			targets [i].position = targetPositions [i];
		}
	}

	private void ClipToGrid() {
		transform.position = new Vector2(Mathf.Round (transform.position.x), Mathf.Round (transform.position.y));
	}
}
