using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HighscoreUI : MonoBehaviour {
    [SerializeField] GameObject panel;
    [SerializeField] GameObject highscoreUIElementPrefab;
    [SerializeField] Transform elementWrapper;
	[SerializeField] GameObject setHighScorePanel;
	[SerializeField] HighscoreHandler highscoreHandler;
	[SerializeField] serial serial;
	[SerializeField] private TextMeshProUGUI gameName;
	[SerializeField] keypress keypress;

	//[SerializeField] keypress enterHighscore;

	List<GameObject> uiElements = new List<GameObject> ();

    private void OnEnable () {
        HighscoreHandler.onHighscoreListChanged += UpdateUI;
		serial.GameIsOver += GameOver;
		serial.GameChanged += GameChanged;
		gameName.text = serial.CurrentGame;
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
		gameName.text = serial.CurrentGame;
		//for(int i = 0; i < uiElements.Count; i++) {
		//print(elementWrapper.childCount);

		//uiElements.RemoveRange(0, uiElements.Count);
		//}
		foreach(Transform t in elementWrapper.transform) {
			Destroy(t.gameObject);
		}
		uiElements.RemoveRange(0, uiElements.Count);
	}
	public void GameOver() {
		//if(setHighScorePanel.activeSelf == false) {
		//	panel.SetActive(true);
		//}
		if(highscoreHandler.IfHighscore2(serial.Points, serial.Throws)) {
			//keypress.enabled = true;
			setHighScorePanel.SetActive(true);
			//print("true");
			//highscoreHandler.AddHighscoreIfPossible(new HighscoreElement("ABC", serial.Points));
		} else {
			panel.SetActive(true);
		}
	}


	public void ShowPanel () {
        panel.SetActive (true);
    }

    public void ClosePanel () {
        panel.SetActive (false);
    }

    private void UpdateUI (List<HighscoreElement> list) {
		//foreach(Transform t in elementWrapper.transform) {
		//	Destroy(t.gameObject);
		//}
		//uiElements.RemoveRange(0, uiElements.Count);
		for (int i = 0; i < list.Count; i++) {
			//print(list[i].playerName);
            HighscoreElement el = list[i];
            if (el != null && el.points > 0) {
                if (i >= uiElements.Count) {
                    // instantiate new entry
                    var inst = Instantiate (highscoreUIElementPrefab, Vector3.zero, Quaternion.identity);
                    inst.transform.SetParent (elementWrapper, false);

                    uiElements.Add (inst);
					//uiElements.Remove(inst);
				}

				// write or overwrite name & points
				//var texts = uiElements[i].GetComponentsInChildren<Text> ();
				var texts = uiElements[i].GetComponentsInChildren<TMPro.TextMeshProUGUI>();
				texts[0].text = el.playerName;
                texts[1].text = el.points.ToString ();
				texts[2].text = el.throws.ToString();
			}
        }
    }

}