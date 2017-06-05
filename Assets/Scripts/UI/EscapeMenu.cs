using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{

	public void GoToMenu() {
		LevelController.currentLevel.LoadMenu ();	
	}

	public void ReloadLevel() {
		LevelController.currentLevel.ReloadLevel ();
	}

}

