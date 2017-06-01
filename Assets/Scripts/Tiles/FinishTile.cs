using UnityEngine;
using System.Collections;
using System;

public class FinishTile : Tile
{
	public static event EventHandler<EventArgs> finishedLevel;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<PlayerController>() != null)
			finishedLevel(null, new EventArgs());
	}

}

