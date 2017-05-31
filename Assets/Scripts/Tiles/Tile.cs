using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	protected Collider2D col;
	protected SpriteRenderer sRend;

	public bool startActive;
	public PulseEventArgs.PulseValue pulseToggledAt = PulseEventArgs.PulseValue.Half;
	public int numPulsesToToggle;

	protected virtual void Start () {
		LevelController.pulsed += ReceivePulse;

		col = GetComponent<Collider2D> ();
		sRend = GetComponent<SpriteRenderer> ();
	}

	protected virtual void OnDestroy() {
		LevelController.pulsed -= ReceivePulse;
	}

	public virtual void ReceivePulse(object sender, PulseEventArgs pulseEvent) {}
}
