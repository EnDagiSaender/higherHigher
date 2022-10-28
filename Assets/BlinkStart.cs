using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkStart : MonoBehaviour {
	[SerializeField] TextMeshProUGUI insertCoinText;
	// Start is called before the first frame update
	void Start() {

	}


	private void OnEnable() {
		StartCoroutine(BlinkSegments(insertCoinText, 0.5f));
	}
	private void OnDisable() {
		StopCoroutine("BlinkSegments");
	}
	private IEnumerator BlinkSegments(TextMeshProUGUI segment, float blinkInterval) {
		while(true) {

			segment.enabled = !segment.enabled;
			yield return new WaitForSeconds(blinkInterval);
		}

	}
}
