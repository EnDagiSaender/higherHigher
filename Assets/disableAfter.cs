using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableAfter : MonoBehaviour
{
	[SerializeField] GameObject CreditsCanvas;
	[SerializeField] GameObject HighScoreCanvas;
	// Start is called before the first frame update
	void OnEnable()
    {
		Invoke("StopShowHs", 10f);
    }

	private void StopShowHs() {
		if(HighScoreCanvas.activeSelf) {
			CreditsCanvas.SetActive(true);
			HighScoreCanvas.SetActive(false);
		}
	}
}
