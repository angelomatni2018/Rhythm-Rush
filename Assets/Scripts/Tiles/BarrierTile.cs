using UnityEngine;
using System.Collections;
using System;

public class BarrierTile : Tile
{
	public static event EventHandler<EventArgs> barrierDeath;

	void OnTriggerEnter2D(Collider2D other) {
		barrierDeath(null, new EventArgs());
	}
}

