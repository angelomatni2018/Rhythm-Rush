using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTile : Tile {

	protected bool isActive;
	Color deathColor;
	int togglePulses;

	protected override void Start () {
		base.Start ();

		isActive = startActive;
		deathColor = sRend.color;
		UpdateTile ();
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
			sRend.color = deathColor;
		} else {
			sRend.color = LevelController.defaultColor;
		}
		col.enabled = isActive;
	}
}
