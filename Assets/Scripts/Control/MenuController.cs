using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class MenuController : MonoBehaviour
{

	public GameObject menu;
	int activeMenu;

	private Vector3 targetPosition;
	public Transform targetPrefab;
	public Transform target;

	static Dictionary<int,Vector3> key_directions = new Dictionary<int, Vector3>
	{
		{0,new Vector3(0,1,0)},
		{1,new Vector3(0,-1,0)},
		{2,new Vector3(1,0,0)},
		{3,new Vector3(-1,0,0)}
	};

	void Awake() {
		activeMenu = 0;

		LevelController.pulsed += ReceivePulse;
	}

	internal void OnDestroy() {
		LevelController.pulsed -= ReceivePulse;
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

	public void ReceivePulse(object sender, PulseEventArgs pulseEvent) {
		if (pulseEvent.pulseValue == PulseEventArgs.PulseValue.Full) {
			int dir = Random.Range (0, 4);
			Func<int, Vector2> PosAt = i => (Vector2)(target.position + i * key_directions [dir]); 

			Vector2 next_pos = (Vector2)transform.position;
			for (int i = 1; i <= 1 + Random.Range(0, 2); i++) {
				next_pos = PosAt (i);
				//print (transform.position + "  " + next_pos + "  " + Physics2D.OverlapPoint (next_pos, LayerMask.GetMask(new string[] {"Barrier"})));
				Collider2D barrier = Physics2D.OverlapPoint (next_pos, LayerMask.GetMask (new string[] { "Barrier" }));
				if (barrier != null) {
					next_pos = PosAt (i - 1);
					break;
				}
				target.position = next_pos;
			}
		}
	}
}
