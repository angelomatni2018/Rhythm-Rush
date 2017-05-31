using UnityEngine;
using System.Collections;

public class VanishingTile : Tile
{
	bool toggled;
	int togglePulses;
	public Color enabledColor, disabledColor;

	override protected void Start() {
		base.Start ();

		toggled = false;
		col = GetComponent<Collider2D> ();
		sRend = GetComponent<SpriteRenderer> ();
		togglePulses = 0;
	}

	public override void ReceivePulse(object sender, PulseEventArgs pulseEvent) {
		if (pulseEvent.pulseValue == pulseToggledAt) {
			togglePulses++;
			if (togglePulses >= numPulsesToToggle) {
				toggled = !toggled;
				UpdateTile ();
				togglePulses = 0;
			}
		}
	}

	void UpdateTile() {
		if (toggled) {
			sRend.color = enabledColor;
		} else {
			sRend.color = disabledColor;
		}
		col.enabled = toggled;
	}
}

