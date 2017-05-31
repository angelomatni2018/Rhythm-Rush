using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	float lastPulse;

	public Rigidbody2D rb2d;
	public float speed;
	private Vector2 offset;
	enum Direction {n, s, e, w, none};
	private Direction dir;
	private Direction last_input;
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
		last_input = Direction.none;
		targetPrefab = Instantiate(targetPrefab, target, Quaternion.identity);
	}

	internal void Awake () {
		numPulses = 0;
		LevelController.pulsed += ReceivePulse;
	}

	internal void OnDestroy() {
		LevelController.pulsed -= ReceivePulse;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "Wall")
			last_input = Direction.none;
	}

	void FixedUpdate () {
		if (Mathf.Round (transform.position.x) == Mathf.Round (target [0]) &
		    Mathf.Round (transform.position.y) == Mathf.Round (target [1])) {
			rb2d.velocity = Vector3.zero;
		}
		if (Input.GetKeyDown ("w")) {
			last_input = Direction.n;
		} else if (Input.GetKeyDown ("a")) {
			last_input = Direction.w;
		} else if (Input.GetKeyDown ("d")) {
			last_input = Direction.e;
		} else if (Input.GetKeyDown ("s")) {
			last_input = Direction.s;
		}
//		transform.Translate ((target - transform.position));
//		rb2d.MovePosition (rb2d.position + offset);
	}

	float getDistance() {
//		float num_tiles = (Mathf.Abs ((Time.time - lastPulse)) * 4);
//		num_tiles -= (num_tiles % 1 - 1);
//		float distance = num_tiles * LevelController.tileScale;
//		return distance;
		return 2;
	}

	public static void KillPlayersAt(float height) {
//		for (int i = 0; i < players.Count; i++) {
//			if (players [i].transform.position.y - height < .001f)
//				players [i].Kill ();
//		}
	}

	public void ReceivePulse(object sender, PulseEventArgs pulseEvent) {
		if (pulseEvent.pulseValue == pulseToggledAt) {
			numPulses++;
			clipToGrid ();
			setTarget ();
			last_input = Direction.none;
			if (numPulses == numPulsesPerInput) {
				lastPulse = LevelController.GetPulseActivation((int)pulseToggledAt);
				numPulses = 0;
			}
		}
	}

	private void setTarget() {
		float distance = getDistance();
		speed = distance / (LevelController.quarterPulse);
		print (distance);
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
		drawTarget ();
	}


	private void drawTarget() {
		targetPrefab.transform.localPosition = target;
	}

	private void clipToGrid() {
		transform.position = new Vector2(Mathf.Round (transform.position.x), Mathf.Round (transform.position.y));
	}
}
