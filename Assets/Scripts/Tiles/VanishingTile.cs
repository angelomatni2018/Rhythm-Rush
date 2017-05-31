using UnityEngine;
using System.Collections;

public class VanishingTile : Tile
{
	bool isActive;
	int togglePulses;
	public Color enabledColor, disabledColor;

	override protected void Start() {
		base.Start ();

		isActive = false;
		col = GetComponent<Collider2D> ();
		sRend = GetComponent<SpriteRenderer> ();
		togglePulses = 0;
	}

	public override void ReceivePulse(object sender, PulseEventArgs pulseEvent) {
		if (pulseEvent.pulseValue == pulseToggledAt) {
			togglePulses++;
			if (togglePulses >= numPulsesToToggle) {
				isActive = !isActive;
				UpdateTile ();
				togglePulses = 0;
			}
		}
	}

	void UpdateTile() {
		if (isActive) {
			sRend.color = enabledColor;
		} else {
			sRend.color = disabledColor;
		}
		col.enabled = !isActive;
	}

	public override bool PlayerContinueMove() { 
		return isActive;
	}

}

