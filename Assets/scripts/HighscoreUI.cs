using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HighscoreUI : MonoBehaviour {
    [SerializeField] GameObject panel;
    [SerializeField] GameObject highscoreUIElementPrefab;
	[SerializeField] GameObject highscoreUIElementPrefabSmallDisplay;
	[SerializeField] Transform elementWrapper;
	[SerializeField] Transform elementWrapperSmallDisplay;
	[SerializeField] GameObject setHighScorePanel;
	[SerializeField] HighscoreHandler highscoreHandler;
	[SerializeField] serial serial;
	[SerializeField] private TextMeshProUGUI gameName;
	[SerializeField] private TextMeshProUGUI gameNameSmallDisplay;
	[SerializeField] keypress keypress;
	Coroutine blinkHs = null;
	TextMeshProUGUI[] texts;
	TextMeshProUGUI[] texts2;

	public delegate void PlayWinner();
	public static event PlayWinner PlayWinnerSound;

	public delegate void PlayDraw();
	public static event PlayDraw PlayDrawSound;

	//[SerializeField] keypress enterHighscore;

	List<GameObject> uiElements = new List<GameObject> ();
	List<GameObject> uiElementsSmall = new List<GameObject>();

	private void OnEnable () {
        HighscoreHandler.onHighscoreListChanged += UpdateUI;
		serial.GameIsOver += GameOver;
		serial.GameChanged += GameChanged;
		gameName.text = serial.DisplayCurrentGameName;//serial.CurrentGame;
		gameNameSmallDisplay.text = serial.DisplayCurrentGameName;//serial.CurrentGame;
		//print(elementWrapper.GetChild(0).GetType());
		//Destroy(elementWrapper.GetChild(0));
		//foreach(Transform t in elementWrapper.transform) {
		//	Destroy(t.gameObject);
		//}

	}

    private void OnDisable () {
        HighscoreHandler.onHighscoreListChanged -= UpdateUI;
		serial.GameIsOver -= GameOver;
		serial.GameChanged -= GameChanged;
		
	}
	public void GameChanged() {
		gameName.text = serial.DisplayCurrentGameName;
		gameNameSmallDisplay.text = serial.DisplayCurrentGameName;
		//for(int i = 0; i < uiElements.Count; i++) {
		//print(elementWrapper.childCount);

		//uiElements.RemoveRange(0, uiElements.Count);
		//}
		if(blinkHs != null) {
			StopCoroutine(blinkHs);
			HsEnableAfterStop();
			blinkHs = null;
		}
		foreach(Transform t in elementWrapper.transform) {
			Destroy(t.gameObject);
		}
		foreach(Transform t in elementWrapperSmallDisplay.transform) {
			Destroy(t.gameObject);
		}
		uiElements.RemoveRange(0, uiElements.Count);
		uiElementsSmall.Clear();
	}
	public void GameOver() {
		//if(setHighScorePanel.activeSelf == false) {
		//	panel.SetActive(true);
		//}
		if(highscoreHandler.IfHighscore2(serial.Points, serial.Throws)) {
			//keypress.enabled = true;
			setHighScorePanel.SetActive(true);
			if(PlayWinnerSound != null) {
				PlayWinnerSound();
			}
			//print("true");
			//highscoreHandler.AddHighscoreIfPossible(new HighscoreElement("ABC", serial.Points));
		} else {
			panel.SetActive(true);
			if(PlayDrawSound != null) {
				PlayDrawSound();
			}
		}
	}


	public void ShowPanel () {
        panel.SetActive (true);
    }

    public void ClosePanel () {
        panel.SetActive (false);
    }
	private IEnumerator BlinkNewHighScore(float blinkInterval) {
		texts = uiElements[highscoreHandler.highScoreNr].GetComponentsInChildren<TMPro.TextMeshProUGUI>();
		texts2 = uiElementsSmall[highscoreHandler.highScoreNr].GetComponentsInChildren<TMPro.TextMeshProUGUI>();
		while(true) {
			for(int i = 1; i < texts.Length; i++) {
				texts[i].enabled = !texts[i].enabled;
			}
			for(int i = 1; i < texts2.Length; i++) {
				texts2[i].enabled = !texts2[i].enabled;
			}
			yield return new WaitForSeconds(blinkInterval);
		}

	}
	private void HsEnableAfterStop() {
		//texts = uiElements[highscoreHandler.highScoreNr].GetComponentsInChildren<TMPro.TextMeshProUGUI>();
		//texts2 = uiElementsSmall[highscoreHandler.highScoreNr].GetComponentsInChildren<TMPro.TextMeshProUGUI>();
		for(int i = 1; i < texts.Length; i++) {
			texts[i].enabled = true;
		}
		for(int i = 1; i < texts2.Length; i++) {
			texts2[i].enabled = true;
		}
	}
	private void UpdateUI (List<HighscoreElement> list) {
		//foreach(Transform t in elementWrapper.transform) {
		//	Destroy(t.gameObject);
		//}
		//uiElements.RemoveRange(0, uiElements.Count);
		if(blinkHs != null) {
			StopCoroutine(blinkHs);
			HsEnableAfterStop();
			blinkHs = null;
		}
		for (int i = 0; i < list.Count; i++) {
			//print(list[i].playerName);
            HighscoreElement el = list[i];
            if (el != null && el.points > 0) {
                if (i >= uiElements.Count) {
                    // instantiate new entry
                    var inst = Instantiate (highscoreUIElementPrefab, Vector3.zero, Quaternion.identity);
                    inst.transform.SetParent (elementWrapper, false);
					uiElements.Add(inst);
					//inst.transform.SetParent(elementWrapperSmallDisplay, false);
					//uiElementsSmall.Add(inst);


					inst = Instantiate(highscoreUIElementPrefabSmallDisplay, Vector3.zero, Quaternion.identity);
					inst.transform.SetParent(elementWrapperSmallDisplay, false);
					uiElementsSmall.Add(inst);
					//uiElements.Remove(inst);
				}

				// write or overwrite name & points
				//var texts = uiElements[i].GetComponentsInChildren<Text> ();
				texts = uiElements[i].GetComponentsInChildren<TMPro.TextMeshProUGUI>();
				texts[0].text = (i + 1).ToString();
				texts[1].text = el.playerName;
				texts[2].text = el.points.ToString();
				texts[3].text = el.throws.ToString();
				texts = uiElementsSmall[i].GetComponentsInChildren<TMPro.TextMeshProUGUI>();
				texts[0].text = (i+1).ToString();
				texts[1].text = el.playerName;
				texts[2].text = el.points.ToString();
				texts[3].text = el.throws.ToString();
				//if(highscoreHandler.highScoreNr == i) {
				//	texts[2].text = "99993";
				//}
				

			}
        }
		if(highscoreHandler.highScoreNr != -1) {
			blinkHs = StartCoroutine(BlinkNewHighScore(0.5f));
			highscoreHandler.highScoreNr = -1;
		}
	}

}