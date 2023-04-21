using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableAfter : MonoBehaviour
{
	[SerializeField] GameObject CreditsCanvas;
	[SerializeField] GameObject HighScoreCanvas;
	[SerializeField] serial serial;
	// Start is called before the first frame update
	void OnEnable()
    {
		Invoke("StopShowHs", 10f);
    }

	private void StopShowHs() {
		if(HighScoreCanvas.activeSelf) {
			serial.DisplayCredits();
			//CreditsCanvas.SetActive(true);
			//HighScoreCanvas.SetActive(false);
		}
	}
}
