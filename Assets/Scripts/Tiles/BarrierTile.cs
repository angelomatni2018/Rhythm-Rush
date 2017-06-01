using UnityEngine;
using System.Collections;
using System;

public class BarrierTile : Tile
{
	public static event EventHandler<EventArgs> barrierDeath;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<PlayerController> () != null) {
			barrierDeath (null, new EventArgs ());
		}
	}
}

