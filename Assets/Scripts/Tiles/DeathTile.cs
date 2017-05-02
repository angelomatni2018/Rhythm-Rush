using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTile : Tile {

	Color deathColor;
	public bool startActive;
	public float pulseToggledAt = .25f;

	protected bool isActive;

	protected override void Start () {
		base.Start ();

		isActive = startActive;
		deathColor = sRend.color;
		UpdateTile ();
	}

	public override void ReceivePulse(object sender, PulseEventArgs pulseEvent) {
		if (pulseEvent.pulseValue == pulseToggledAt) {
			isActive = !isActive;
			UpdateTile ();
		}
	}

	void UpdateTile() {
		if (isActive) {
			sRend.color = deathColor;
		} else {
			sRend.color = LevelController.defaultColor;
		}
		playerCol.enabled = isActive;
	}
}
