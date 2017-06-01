using UnityEngine;
using System.Collections;

public class RowDisappearTrigger : MonoBehaviour
{
	public int rowIndex;

	public void OnTriggerEnter2D(Collider2D col) {
		if (col.GetComponent<PlayerController> () != null) {
			LevelController.currentLevel.DisappearRowsTo (rowIndex);
			GetComponent<Collider2D> ().enabled = false;
		}
	}
}

