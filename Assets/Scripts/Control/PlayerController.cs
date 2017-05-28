using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	float lastPulse;

	public Rigidbody2D rb2d;
	public float speed;
	private Vector2 offset;

	//enum Direction {n, s, e, w, none};
	//private Direction dir;
	//private Direction last_input;
	float input_buffer_threshold;
	float input_accuracy_threshold;
	KeyCode last_input;
	float last_input_time;
	int current_scale;

	private bool seekingInput = true;
	private double threshold = 0.3;
	private Vector3 target;
	public Transform targetPrefab;

	// Determines how close you have to input to the beat of the music
	public float pulseThreshold;

	public int numPulsesPerInput = 4;
	public PulseEventArgs.PulseValue pulseToggledAt = PulseEventArgs.PulseValue.Half;
	int numPulses;

	static Dictionary<KeyCode,Vector3> key_directions = new Dictionary<KeyCode, Vector3>
	{
		{KeyCode.W,new Vector3(0,1,0)},
		{KeyCode.S,new Vector3(0,-1,0)},
		{KeyCode.D,new Vector3(1,0,0)},
		{KeyCode.A,new Vector3(-1,0,0)}
	};

	void Start ()
	{
		//last_input = Direction.none;
		targetPrefab = Instantiate(targetPrefab, target, Quaternion.identity);

		input_buffer_threshold = LevelController.quarterPulse / 4;
		input_accuracy_threshold = LevelController.quarterPulse / 8;
		last_input_time = Time.time;
		last_input = KeyCode.None;
		current_scale = 1;
	}

	internal void Awake () {
		numPulses = 0;
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
/*
		if (Mathf.Round (transform.position.x) == Mathf.Round (target [0]) &
			Mathf.Round (transform.position.y) == Mathf.Round (target [1])) {
			return true;
		}
*/
	}

	void FixedUpdate () {
		if (PlayerAtTilePos (target)) {
			rb2d.velocity = Vector3.zero;
		}
		if (Time.time - last_input_time > input_buffer_threshold) {
			foreach (KeyValuePair<KeyCode,Vector3> pair in key_directions) {
				if (Input.GetKeyDown (pair.Key)) {
					last_input = pair.Key;
					last_input_time = Time.time;
				}
			}
		}
		/*
		if (Input.GetKeyDown ("w")) {
			last_input = Direction.n;
		} else if (Input.GetKeyDown ("a")) {
			last_input = Direction.w;
		} else if (Input.GetKeyDown ("d")) {
			last_input = Direction.e;
		} else if (Input.GetKeyDown ("s")) {
			last_input = Direction.s;
		}
		*/
//		transform.Translate ((target - transform.position));
//		rb2d.MovePosition (rb2d.position + offset);
	}

	float getDistance() {
//		float num_tiles = (Mathf.Abs ((Time.time - lastPulse)) * 4);
//		num_tiles -= (num_tiles % 1 - 1);
//		float distance = num_tiles * LevelController.tileScale;
//		return distance;
		return 1;
	}

	public void KillPlayersAt(float height) {
		GameObject.Destroy (gameObject);
	}

	public void ReceivePulse(object sender, PulseEventArgs pulseEvent) {
		if (pulseEvent.pulseValue == pulseToggledAt) {
			numPulses++;
			clipToGrid ();
			if (last_input != KeyCode.None)
				setTarget ();
			//last_input = Direction.none;
			if (numPulses == numPulsesPerInput) {
				lastPulse = LevelController.GetPulseActivation((int)pulseToggledAt);
				numPulses = 0;
			}
		}
	}

	private void setTarget() {
		float distance = getDistance();
		speed = 2 * distance / (LevelController.quarterPulse);
		//print (distance);

		float accuracy = Time.time - last_input_time;
		if (accuracy > input_accuracy_threshold) {
			print (accuracy);
			rb2d.velocity = current_scale * key_directions [last_input] * speed;
			target = transform.position + current_scale * distance * key_directions [last_input];
			// Upgrade speed factor
			current_scale = Mathf.Min(3,current_scale + 1);
		} else {
			// Downgrade speed factor
			if (current_scale == 1) {
				//Punish player
			} else {
				current_scale--;
			}
		}

		drawTarget ();
		last_input = KeyCode.None;
		/*
		dir = last_input;
		if (dir == Direction.n) {
			target = new Vector3 (Mathf.Round(transform.position.x), Mathf.Round(transform.position.y + distance), 0);
			rb2d.velocity = new Vector3 (0, speed, 0);
		} else if (dir == Direction.s) {
			target = new Vector3 (Mathf.Round(transform.position.x), Mathf.Round(transform.position.y - distance), 0);
			rb2d.velocity = new Vector3 (0, -speed, 0);
		} else if (dir == Direction.e) {
			rb2d.velocity = new Vector3 (speed, 0, 0);
			target = new Vector3 (Mathf.Round(transform.position.x + distance), Mathf.Round(transform.position.y), 0);
		} else if (dir == Direction.w) {
			rb2d.velocity = new Vector3 (-speed, 0, 0);
			target = new Vector3 (Mathf.Round(transform.position.x - distance), Mathf.Round(transform.position.y), 0);
		} else {
			offset = new Vector3 (0, 0, 0);
		}
		*/
	}


	private void drawTarget() {
		targetPrefab.transform.localPosition = target;
	}

	private void clipToGrid() {
		transform.position = new Vector2(Mathf.Round (transform.position.x), Mathf.Round (transform.position.y));
	}
}
