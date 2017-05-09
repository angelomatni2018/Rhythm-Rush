using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseUI : MonoBehaviour {

	Image pulseIndicator;
	bool fadeIn;

	// Use this for initialization
	void Awake () {
		pulseIndicator = GetComponent<Image> ();
		fadeIn = false;
	}
	
	// Update is called once per frame
	void Update () {
		float toggle = fadeIn ? 1 : -1;
		float newA = pulseIndicator.color.a + toggle * Time.deltaTime / LevelController.quarterPulse;
		if (newA > 1) {
			newA = 1;
			fadeIn = false;
		} else if (newA < 0) {
			newA = 0;
			fadeIn = true;
		}
		pulseIndicator.color = new Color (pulseIndicator.color.r, pulseIndicator.color.g, pulseIndicator.color.b, newA);
	}
}
