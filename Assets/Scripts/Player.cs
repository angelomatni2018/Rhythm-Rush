using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public Slider healthSlider;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Kill() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	void OnTriggerEnter2D(Collider2D col) {
		healthSlider.value -= 0.11f;
//		Kill ();
	}
}
