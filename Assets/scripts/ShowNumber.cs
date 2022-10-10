using TMPro;
using UnityEngine;

public class ShowNumber : MonoBehaviour {

	private float timer;

	[SerializeField]
	private TextMeshProUGUI firstDigit;
	[SerializeField]
	private TextMeshProUGUI secondDigit;
	[SerializeField]
	private TextMeshProUGUI thirdDigit;
	

	// Start is called before the first frame update
	void Start() {
		ResetTimer();

	}

	// Update is called once per frame
	void Update() {
		if(timer > 0) {
			timer -= Time.deltaTime;
			//UpdateTimerDisplay(timer);
		} else {
			Flash();
		}
	}
	private void ResetTimer() {
		//timer = timeDuration;


	}
	private void UpdateTimerDisplay(int point) {
		//float minutes = Mathf.FloorToInt(time / 60);
		string score = string.Format("{0:000}", point);
		firstDigit.text = score[0].ToString();
		secondDigit.text = score[1].ToString();
		thirdDigit.text = score[2].ToString();

	}
	private void Flash() {

	}
}
