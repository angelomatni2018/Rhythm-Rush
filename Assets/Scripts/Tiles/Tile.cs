using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	protected Collider2D playerCol;
	protected SpriteRenderer sRend;

	protected virtual void Start () {
		LevelController.pulsed += ReceivePulse;

		playerCol = GetComponent<Collider2D> ();
		sRend = GetComponent<SpriteRenderer> ();
	}

	protected virtual void OnDestroy() {
		LevelController.pulsed -= ReceivePulse;
	}

	public virtual void ReceivePulse(object sender, PulseEventArgs pulseEvent) {}
}
