using UnityEngine;
using System.Collections;
using System;

public class VanishingTile : BarrierTile
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
//		col.enabled = isActive;
	}

	public override void AffectPlayer(PlayerController pc) {}

	public override bool PlayerContinueMove() { 
		return isActive;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<PlayerController> () != null) {
			BarrierTile.FireDeath ("Game Over! You were killed by the vanishing tile.");
		}
	}
}

