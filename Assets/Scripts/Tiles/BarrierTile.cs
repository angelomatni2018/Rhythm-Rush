using UnityEngine;
using System.Collections;
using System;

public class BarrierTile : Tile
{
	public static event EventHandler<KillPlayerEventArgs> barrierDeath;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<PlayerController> () != null) {
			BarrierTile.FireDeath ("Game Over! The stage caught up to you.");
		}
	}

	public static void FireDeath(string deathText) {
		barrierDeath (null, new KillPlayerEventArgs (deathText));
	}
}

