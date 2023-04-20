using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlinkArrow : MonoBehaviour
{
	[SerializeField] Image Arrow;
	[SerializeField] serial serial;
	// Start is called before the first frame update
	void Start()
    {
        
    }

	private void OnEnable() {
		if(serial.gameMode == 7) {
			StartCoroutine(BlinkSegments(Arrow, 0.5f));
		} else {
			Arrow.enabled = false;
		}
	}
	private void OnDisable() {
		StopCoroutine("BlinkSegments");
	}
	private IEnumerator BlinkSegments(Image img, float blinkInterval) {
		while(true) {

			img.enabled = !img.enabled;
			yield return new WaitForSeconds(blinkInterval);
		}

	}
}
