using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO.Ports;



public class serial : MonoBehaviour
{

	public delegate void GameRunning();
	public static event GameRunning GameIsOver;

	public delegate void GameIsChanged();
	public static event GameIsChanged GameChanged;
	
	[SerializeField] GameObject highScorePanel;
	[SerializeField] GameObject setHighScorePanel;
	[SerializeField] EventManager EventManager;
	[SerializeField] Transform MainCanvas;

	[SerializeField] GameObject scoreUIElementPrefab;
	[SerializeField] GameObject BigScoreUIElementPrefab;
	[SerializeField] GameObject SmallScoreUIElementPrefab;
	[SerializeField] GameObject TotalScoreUIElementPrefab;
	[SerializeField] GameObject TotalLivesUIElementPrefab;
	[SerializeField] GameObject TotalThrowsUIElementPrefab;
	[SerializeField] Transform elementWrapper;
	List<GameObject> uiElements = new List<GameObject>();

	[SerializeField] Sprite Sprite1;
	[SerializeField] Sprite Sprite2;

	[SerializeField] GameObject lifePanel;
	[SerializeField] GameObject throwPanel;
	[SerializeField] GameObject totalScorePanel;
	[SerializeField] GameObject scorePanel1;
	[SerializeField] GameObject scorePanel2;
	[SerializeField] GameObject scorePanel3;
	[SerializeField] GameObject scorePanel4;
	[SerializeField] GameObject scorePanel5;

	[SerializeField] TextMeshProUGUI GameName;

	private TextMeshProUGUI[] totalLives;
	private TextMeshProUGUI[] totalthrows;
	private TextMeshProUGUI[] totalScores;
	private TextMeshProUGUI[] score1;
	private TextMeshProUGUI[] score2;
	private TextMeshProUGUI[] score3;
	private TextMeshProUGUI[] score4;
	private TextMeshProUGUI[] score5;



	private TextMeshProUGUI[] testText;
	private Transform[] tempTransform;
	private TextMeshProUGUI[] tempText1;
	private Transform[] testTransform;
	private Transform[] testTransform2;
	private Component[] testComponet;
	private GameObject[] testObjekt;

	private int totalLifes = 0;
	private bool lostLife = false;
	private bool skipMoveDown = false;
	private int oldScore = 0;
	private int oldScore2 = 0;
	private int nrOfBlink;
	private float timer = 0;
	private float flashTimer = 0;
	private float deltaTime;
	private bool even = false;
	private bool blinkOn = true;
	private int totalScore = 0;
	private int totalThrows = 0;
	private bool gameOver = false;
	private int gameMode = 3;
	private int gameModeMultiplyer = 1;
	private string score = "   ";
	private string totScore = "    ";
	private int gameModeMax = 4;


	private void AddScorePannel() {
		var inst = Instantiate(scoreUIElementPrefab, Vector3.zero, Quaternion.identity);
		inst.transform.SetParent(elementWrapper, false);
		uiElements.Add(inst);
		//print(uiElements.Count);
	}
	private TextMeshProUGUI[] GetChildText(GameObject prefab, Vector3 position) {
		var inst = Instantiate(prefab, position, Quaternion.identity);
		inst.transform.SetParent(MainCanvas, false);
		uiElements.Add(inst);
		tempText1 = inst.GetComponentsInChildren<TextMeshProUGUI>();
		TextMeshProUGUI[] tempText2 = new TextMeshProUGUI[tempText1.Length / 2];
		//uiElements[uiElements.Count -1].transform.localPosition = position;
		int j = 0;
		for(int i = 1; i < tempText1.Length; i++) {
			//tempText1[i].fontSize = size;
			if(i % 2 == 1) {
				tempText2[j] = tempText1[i];
				j += 1;
			}
		}
		return tempText2;
	}
	private TextMeshProUGUI[] GetChildText(GameObject panel) {
		tempText1 = panel.GetComponentsInChildren<TextMeshProUGUI>();
		TextMeshProUGUI[] tempText2 = new TextMeshProUGUI[tempText1.Length/2];
		int j = 0;
		for(int i = 1; i < tempText1.Length; i += 2) {
			tempText2[j] = tempText1[i];
			j += 1;
		}
		return tempText2;
	}
	private void MoveText(TextMeshProUGUI[] from, TextMeshProUGUI[] to) {
		for(int i = 0; i < from.Length; i++) {
			to[i].text  = from[i].text;
		}
	}
	private void RemoveAllListObjects() {
		foreach(GameObject t in uiElements) {
			Destroy(t);
		}
		uiElements.RemoveRange(0, uiElements.Count);
	}
	private void ResetScore(TextMeshProUGUI[] text, bool dot, bool zero) {
		for(int i = 0; i < text.Length; i++) {

			if(dot && i == (text.Length - 1)) {
				text[i].text = ".";
			} else if(dot && zero && (i == text.Length - 2 || i == text.Length - 3)) {
				text[i].text = "0";
			} else if(zero && i == (text.Length - 1)) {
				text[i].text = "0";
			} else {
				text[i].text = "";
			}
		}
	}
	private void ResetScore(TextMeshProUGUI[] text, bool dot) {
		for(int i = 0; i < text.Length; i++) {
			if(dot && i == (text.Length - 1)) {
				text[i].text = ".";
			} else {
				text[i].text = "";
			}
		}
	}
	private void ResetScore(TextMeshProUGUI[] text) {
		for(int i = 0; i < text.Length; i++) {
			text[i].text = "";
		}
	}
	private void OnEnable() {
		EventManager.NewGame += newGame;
		EventManager.ChangedDir += changeGame;
		EventManager.NewScore += updateScore;
		MainCanvas.GetComponent<UnityEngine.UI.Image>().sprite = Sprite1;
		//lifePanel.transform.localPosition = new Vector3(326, 385, 0);
		//score1 = GetChildText(scorePanel1);
		//AddScorePannel();
		//score2 = GetChildText(uiElements[0]);
		//AddScorePannel();
		//score3 = GetChildText(uiElements[1]);
		//AddScorePannel();
		//score4 = GetChildText(uiElements[2]);
		//AddScorePannel();
		//score5 = GetChildText(uiElements[3]);
		score1 = GetChildText(BigScoreUIElementPrefab, new Vector3(-154, 125, 0));
		score2 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -122, 0));
		score3 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -337, 0));
		score4 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -554, 0));
		score5 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -768, 0));

		//score2 = GetChildText(scorePanel2);
		//score3 = GetChildText(scorePanel3);
		//score4 = GetChildText(scorePanel4);
		//score5 = GetChildText(scorePanel5);
		totalScores = GetChildText(TotalScoreUIElementPrefab, new Vector3(-196, 716, 0));
		//totalScores = GetChildText(totalScorePanel);
		//totalthrows = GetChildText(throwPanel);
		totalthrows = GetChildText(TotalThrowsUIElementPrefab, new Vector3(329, 708, 0));
		//totalLives = GetChildText(lifePanel);
		totalLives = GetChildText(TotalLivesUIElementPrefab, new Vector3(329, 385, 0));
		//totalLives = GetChildText(lifePanel, new Vector3(326, 385, 0));
		//totalLives[0].fontSize = 400;
		//TextMeshProUGUI[] tempTextt = totalLives[0].GetComponentsInParent<TextMeshProUGUI>();
		//tempTextt[1].fontSize = 400;
		//print(tempTextt.Length);
		GameName.text = CurrentGame;




	}
	private void OnDisable() {
		EventManager.NewGame -= newGame;
		EventManager.ChangedDir -= changeGame;
		EventManager.NewScore -= updateScore;
	}
	private void changeGame(bool increse) {
		if(setHighScorePanel.activeSelf == false) {


			RemoveAllListObjects();
			score1 = GetChildText(BigScoreUIElementPrefab, new Vector3(-154, 125, 0));
			score2 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -122, 0));
			score3 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -337, 0));
			score4 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -554, 0));
			score5 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -768, 0));
			totalScores = GetChildText(TotalScoreUIElementPrefab, new Vector3(-196, 716, 0));
			totalthrows = GetChildText(TotalThrowsUIElementPrefab, new Vector3(329, 708, 0));
			totalLives = GetChildText(TotalLivesUIElementPrefab, new Vector3(329, 385, 0));


			if(increse) {
				gameMode += 1;
				if(gameMode > gameModeMax) {
					gameMode = 0;
				}
			} else {
				gameMode -= 1;
				if(gameMode < 0) {
					gameMode = gameModeMax;
				}
			}
			highScorePanel.SetActive(false);
			GameName.text = CurrentGame;
			NewGame();
			if(GameChanged != null) {
				GameChanged();
			}
			}
		}
	private void updateScore(int newScore) {
		if(gameOver == false) {
			if(gameMode == 0) {
				UpdateDisplay(Mathf.RoundToInt((float)newScore / 10));
			} else {
				UpdateDisplay(newScore);
			}
			blinkOn = true;
		}
	}


	private void newGame() {
		if(setHighScorePanel.activeSelf == false) {
			highScorePanel.SetActive(false);
			NewGame();
		}

	}


	void Start()
    {
		NewGame();

		//Open();
		
	}

	public int Points {
		get {
			return totalScore;
		}
	}

	public string CurrentGame {
		get {
			switch(gameMode) {
				case 0:
					return "HigherHigher_";
				case 1:
					return "HigherHigher_no_life";
				case 2:
					return "InBetween";
				case 3:
					return "HigherHigher";
				case 4:
					return "LowerLower";
				case 5:
					return "LowerLower";
				default:
					return "HighScore";
			}
		}
	}
	public int Throws {
		get {
			return totalThrows;
		}
	}
	public bool IsGameOver {
		get {
			return gameOver;
		}
	}
	// Update is called once per frame
	bool NotShowingHighScore() {
		if(setHighScorePanel.activeSelf == true || highScorePanel.activeSelf == true) {
			return false;
		} else {
			return true;
		}
	}


	void Update() {
		deltaTime = Time.deltaTime;
		timer += deltaTime;

		if(blinkOn == true) {
			flashTimer += deltaTime;
			if((flashTimer) > 0.1) {
				nrOfBlink += 1;
				if(nrOfBlink > 4) {
					blinkOn = false;
					nrOfBlink = 1;
				}
				flashTimer = 0;
				even = !even;
				blinkDisplay(even);

			} 

		}

    }
	//void changeColor(Color32 color, string tag) {
	//	GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
	//	for (int i = 0; i < gameObjects.Length; i++) {
	//		//score1[i] = gameObjects[i].GetComponent<TMPro.TextMeshProUGUI>();
	//		gameObjects[i].GetComponent<TMPro.TextMeshProUGUI>().color = color;
	//	}

	//}
	private void changeColor2(Color32 color, TextMeshProUGUI[] text) {
		foreach(TextMeshProUGUI Text in text) {
			Text.color = color;
		}
	}

	void readByte(int stat) {
		//if(stat == 2) {
		//print(stat);
		blinkOn = true;
		UpdateDisplay(stat);
		//} ///else {
		///	print(0);
		///}
	}
	private void blinkDisplay(bool enabled) {
		score1[0].enabled = enabled;
		score1[1].enabled = enabled;
		score1[2].enabled = enabled;
		if(gameMode != 0) {
			score1[3].enabled = enabled;
		}
	}
	private void NewGame() {
		changeColor2(new Color32(0, 255, 0, 220), score1);


		ResetScore(score1, true, false);
		ResetScore(score2, true, false);
		ResetScore(score3, true, false);
		ResetScore(score4, true, false);
		ResetScore(score5, true, false);

		ResetScore(totalScores, true, true);


		totalthrows[0].text = "";
		totalthrows[1].text = "0";
		score3[4].text = "";
		score4[4].text = "";
		score5[4].text = "";

		totalLives[0].text = "";
		totalLifes = 0;

		if(gameMode == 0) {                 // no decimal
			totalScores[5].text = "";
			score1[4].text = "";
			score2[4].text = "";
			score3[4].text = "";
			score4[4].text = "";
			score5[4].text = "";
			totalScores[4].text = "";
			gameModeMultiplyer = 1;
		}
		else if((gameMode == 1) || (gameMode == 2) || (gameMode == 3) || gameMode == 4) {
			totalScores[5].text = ".";
			score1[4].text = ".";
			score2[4].text = ".";
			if(gameMode != 2) {
				score3[4].text = ".";
				score4[4].text = ".";
				score5[4].text = ".";
				if(gameMode == 3 || gameMode == 4) {
					totalLives[0].text = "1";
					totalLifes = 1;
				}
			}

			totalScores[4].text = "0";
			gameModeMultiplyer = 10;
		}

		totalScore = 0;
		oldScore = 0;
		oldScore2 = 0;
		totalThrows = 0;
		gameOver = false;


	}
	

	private void GameOver() {
		changeColor2(new Color32(255, 0, 0, 220), score1);
		if(totalLifes > 0) {
			totalLifes -= 1;
			totalLives[0].text = totalLifes.ToString();
			lostLife = true;
		} else {
			gameOver = true;
			Invoke("GameIsOverAfterDelay", 1f);
		}
	}
	private void GameIsOverAfterDelay() {
		if(GameIsOver != null) {
			GameIsOver();
		}
	}
	private bool IsWithin(int a, int b, int c) {
		//bool returnIsWithin = (c >= Mathf.Min(a, b) && c <= Mathf.Max(a, b));
		return (c >= Mathf.Min(a, b) && c <= Mathf.Max(a, b)); //returnIsWithin;

		//return true;
	}

	private void UpdateDisplay(int point) {
		if(lostLife == true) {
			changeColor2(new Color32(0, 255, 0, 220), score1);
			lostLife = false;
			skipMoveDown = true;
		}
		if(gameMode == 2) {
			if((IsWithin(oldScore2, oldScore, point)) || totalThrows < 2) {
				oldScore2 = oldScore;
				oldScore = point;
				totalThrows += 1;
				totalScore += point;
			} else {
				GameOver();
				score3[4].text = ".";
			}
		} else if(gameMode == 4) {
			if(point <= oldScore || totalThrows == 0) {
				totalScore += point;
				oldScore = point;
				totalThrows += 1;
			} else {
				GameOver();
			}
		} else {
			if(point >= oldScore) {
				totalScore += point;
				oldScore = point;
				totalThrows += 1;
			} else {
				GameOver();
			}
		}



		if(skipMoveDown == false) {

			if((gameMode != 2) || (gameOver == true)) {

				MoveText(score4, score5);
				MoveText(score3, score4);
				MoveText(score2, score3);

			}
			MoveText(score1, score2);
		} else {
			skipMoveDown = false;
		}
		string totThrow = string.Format("{0:00}", totalThrows);
		if(totalThrows >= 10) {
			totalthrows[0].text = totThrow[0].ToString();
		} else {
			totalthrows[0].text = "";
		}
		totalthrows[1].text = totThrow[1].ToString();

		if(gameMode == 0) {
			totScore = string.Format("{0:0000}", totalScore);
		} else {
			totScore = string.Format("{0:00000}", totalScore);
			totalScores[4].text = totScore[4].ToString();
		}
		//for(int j = 0; j < totScore.Length; j++) {
		//	totalScores[j].text = totScore[j].ToString();
		//}
		if(totalScore >= 1000 * gameModeMultiplyer) {
			totalScores[0].text = totScore[0].ToString();
		} else {
			totalScores[0].text = "";
		}
		if(totalScore >= 100 * gameModeMultiplyer) {
			totalScores[1].text = totScore[1].ToString();
		} else {
			totalScores[1].text = "";
		}
		if(totalScore >= 10 * gameModeMultiplyer) {
			totalScores[2].text = totScore[2].ToString();
		} else {
			totalScores[2].text = "";
		}
		totalScores[3].text = totScore[3].ToString();



		if(gameMode == 0) {
			score = string.Format("{0:000}", point);
		} else {
			score = string.Format("{0:0000}", point);
			score1[3].text = score[3].ToString();
		}
		if(point >= 100 * gameModeMultiplyer) {
			score1[0].text = score[0].ToString();
		} else {
			score1[0].text = "";
		}
		if(point >= 10 * gameModeMultiplyer) {
			score1[1].text = score[1].ToString();
		} else {
			score1[1].text = "";
		}
		score1[2].text = score[2].ToString();







	}
}
