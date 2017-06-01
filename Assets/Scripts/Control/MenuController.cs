using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

	public GameObject menu;
	int activeMenu;

	void Awake() {
		activeMenu = 0;
	}

	public void LoadMenuPage(int menuIndex) {
		menu.transform.GetChild (activeMenu).gameObject.SetActive (false);
		activeMenu = menuIndex;
		menu.transform.GetChild (activeMenu).gameObject.SetActive (true);
	}

	public void ExitGame() {
		Application.Quit ();
	}

	public void LoadScene(int index) {
		SceneManager.LoadScene (GameController.GetSceneName (index));
	}
}
