using UnityEngine;
using System.Collections;
using System;

public class FinishTile : Tile
{
	public static event EventHandler<EventArgs> finishedLevel;

	void OnTriggerEnter2D(Collider2D other) {
		finishedLevel(null, new EventArgs());
	}

}

