using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseUI : MonoBehaviour {

	Image pulseIndicator;
	public PulseEventArgs.PulseValue pulseToggledAt;
	public int numPulsesPerToggle;

	int numPulses;

	// Use this for initialization
	void Awake () {
		pulseIndicator = GetComponent<Image> ();
		LevelController.pulsed += ReceivePulse;
		numPulses = 0;
	}

	void OnDestroy() {
		LevelController.pulsed -= ReceivePulse;
	}
	
	// Update is called once per frame
	void Update () {
		SetPulserAlpha (pulseIndicator.color.a - (Time.deltaTime / 2) / LevelController.quarterPulse);
	}

	public void ReceivePulse(object sender, PulseEventArgs pulseEvent) {
		if (pulseEvent.pulseValue == pulseToggledAt) {
			numPulses++;
			if (numPulses == numPulsesPerToggle) {
				SetPulserAlpha (1);
				numPulses = 0;
			}
		}
	}

	void SetPulserAlpha(float newAlpha) {
		pulseIndicator.color = new Color (pulseIndicator.color.r, pulseIndicator.color.g, pulseIndicator.color.b, newAlpha);
	}

}
