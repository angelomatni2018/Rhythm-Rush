using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
	public Text deathTextField;

	public void GoToMenu() {
		LevelController.currentLevel.LoadMenu ();	
	}

	public void ReloadLevel() {
		LevelController.currentLevel.ReloadLevel ();
	}


}

