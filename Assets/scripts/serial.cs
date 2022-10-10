using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO.Ports;



public class serial : MonoBehaviour
{

	public delegate void GameRunning();
	public static event GameRunning GameIsOver;

	//public delegate void GameButtonOk();
	//public static event GameButtonOk OkButton;

	//public delegate void GameButtonLeft();
	//public static event GameButtonLeft LeftButton;

	//public delegate void GameButtonRight();
	//public static event GameButtonRight RightButton;

	public delegate void GameIsChanged();
	public static event GameIsChanged GameChanged;

	[SerializeField]
	private TextMeshProUGUI firstDigit;
	[SerializeField]
	private TextMeshProUGUI secondDigit;
	[SerializeField]
	private TextMeshProUGUI thirdDigit;
	[SerializeField]
	private TextMeshProUGUI fourthDigit;
	[SerializeField]
	private TextMeshProUGUI dot;

	[SerializeField]
	private TextMeshProUGUI firstDigit2;
	[SerializeField]
	private TextMeshProUGUI secondDigit2;
	[SerializeField]
	private TextMeshProUGUI thirdDigit2;
	[SerializeField]
	private TextMeshProUGUI fourthDigit2;
	[SerializeField]
	private TextMeshProUGUI dot2;

	[SerializeField]
	private TextMeshProUGUI firstDigit3;
	[SerializeField]
	private TextMeshProUGUI secondDigit3;
	[SerializeField]
	private TextMeshProUGUI thirdDigit3;
	[SerializeField]
	private TextMeshProUGUI fourthDigit3;
	[SerializeField]
	private TextMeshProUGUI dot3;

	[SerializeField]
	private TextMeshProUGUI firstDigit4;
	[SerializeField]
	private TextMeshProUGUI secondDigit4;
	[SerializeField]
	private TextMeshProUGUI thirdDigit4;
	[SerializeField]
	private TextMeshProUGUI fourthDigit4;
	[SerializeField]
	private TextMeshProUGUI dot4;

	[SerializeField]
	private TextMeshProUGUI firstDigit1;
	[SerializeField]
	private TextMeshProUGUI secondDigit1;
	[SerializeField]
	private TextMeshProUGUI thirdDigit1;
	[SerializeField]
	private TextMeshProUGUI fourthDigit1;
	[SerializeField]
	private TextMeshProUGUI dot1;

	[SerializeField]
	private TextMeshProUGUI totalScoreFirstDigit;
	[SerializeField]
	private TextMeshProUGUI totalScoreSecondDigit;
	[SerializeField]
	private TextMeshProUGUI totalScoreThirdDigit;
	[SerializeField]
	private TextMeshProUGUI totalScoreFourthDigit;
	[SerializeField]
	private TextMeshProUGUI totalScoreFifthDigit;
	[SerializeField]
	private TextMeshProUGUI totalScoreDot;

	[SerializeField]
	private TextMeshProUGUI totalThrowsFirstDigit;
	[SerializeField]
	private TextMeshProUGUI totalThrowsSecondDigit;

	[SerializeField]
	private TextMeshProUGUI totalLifesFirstDigit;


	

	[SerializeField] GameObject highScorePanel;
	[SerializeField] GameObject setHighScorePanel;
	[SerializeField] EventManager EventManager;


	[SerializeField] GameObject lifePanel;
	[SerializeField] GameObject throwPanel;
	[SerializeField] GameObject totalScorePanel;
	[SerializeField] GameObject scorePanel1;
	[SerializeField] GameObject scorePanel2;
	[SerializeField] GameObject scorePanel3;
	[SerializeField] GameObject scorePanel4;
	[SerializeField] GameObject scorePanel5;

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
	//private TextMeshProUGUI[] tempText2;
	//private TextMeshProUGUI[] score1Text;
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
	private int gameModeMax = 3;
	//enum game { HigherHigher, InBetween }
	//game currentGame = game.HigherHigher;
	SerialPort portNo;


	// Start is called before the first frame update
	void Open() {

		string[] ports = { };
		while(ports.Length  < 1) {
			ports = SerialPort.GetPortNames();
			if(ports.Length < 1) {
				//print("no port, try again");
				new WaitForSeconds(1);
			} else {
				//print(ports[0]);
			}
		}
		foreach(string port in ports) {
			print(port);
		}

		portNo = new SerialPort("\\\\.\\" + ports[0], 115200);
		try {
			portNo.Open();
			portNo.ReadTimeout = 500;
		} catch {
			print("no port to open");
		}
		
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
		lifePanel.transform.localPosition = new Vector3(326, 385, 0);
		int i = 0;
		testTransform = lifePanel.GetComponentsInChildren<Transform>();

		foreach(Transform child in testTransform) {
			testText = child.GetComponentsInChildren<TextMeshProUGUI>();
			i += 1;
		}

		score1 = GetChildText(scorePanel1);
		score2 = GetChildText(scorePanel2);
		score3 = GetChildText(scorePanel3);
		score4 = GetChildText(scorePanel4);
		score5 = GetChildText(scorePanel5);
		totalScores = GetChildText(totalScorePanel);
		totalthrows = GetChildText(throwPanel);


		foreach(TextMeshProUGUI Text in score1) {
			print(Text.text);
		}

	}
	private void OnDisable() {
		EventManager.NewGame -= newGame;
		EventManager.ChangedDir -= changeGame;
		EventManager.NewScore -= updateScore;
	}
	private void changeGame(bool increse) {
		if(setHighScorePanel.activeSelf == false) {
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
					return "InBetween";
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
				//print(flashTimer);
				flashTimer = 0;
				even = !even;
				blinkDisplay(even);

			} 

		}

		//if(timer > 0.1){
		//	timer = 0;

		//	if(portNo.IsOpen) {
		//		try {
		//			///readByte(portNo.ReadByte());
		//			string msg = portNo.ReadLine();
		//			string[] message = msg.Split(',');
		//			if(Equals(message[0], "s")) {
		//				//portNo.Write("c\r\n");
		//				//portNo.WriteLine("c");
		//				//print(message[1]);
		//				if(gameOver == false) {
		//					int speed = int.Parse(message[1]);
		//					if(gameMode == 0) {
		//						readByte(Mathf.RoundToInt((float)speed / 10));
		//					} else {
		//						readByte(speed);
		//					}
		//				}
		//			} else if(Equals(message[0], "n")) {
		//				if(setHighScorePanel.activeSelf) {
		//					if(OkButton != null) {
		//						OkButton();
		//					}
		//				} else {

		//					highScorePanel.SetActive(false);
		//					NewGame();
		//				}

		//			} else if(Equals(message[0], "c")) {
		//				if(setHighScorePanel.activeSelf) {
		//					if(RightButton != null) {
		//						RightButton();
		//					}

		//				} else {							
		//					gameMode += 1;
		//					if(gameMode > 3) {
		//						gameMode = 0;
		//					}
		//					if(GameChanged != null) {
		//						GameChanged();
		//					}
		//					highScorePanel.SetActive(false);
		//					NewGame(); 
		//				}

		//			} else if(Equals(message[0], "d")) {
		//				if(setHighScorePanel.activeSelf) {
		//					if(LeftButton != null) {
		//						LeftButton();
		//					}

		//				} else {
		//					gameMode -= 1;
		//					if(gameMode < 0) {
		//						gameMode = 3;
		//					}
		//					if(GameChanged != null) {
		//						GameChanged();
		//					}
		//					highScorePanel.SetActive(false);
		//					NewGame();
		//				}

		//			}
		//		} catch {
		//		}
		//	} else {
		//		new WaitForSeconds(1);
		//		print("try open port");
		//		Open();
		//	}

		//}



    }
	void changeColor(Color32 color, string tag) {
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
		for (int i = 0; i < gameObjects.Length; i++) {
			//score1[i] = gameObjects[i].GetComponent<TMPro.TextMeshProUGUI>();
			gameObjects[i].GetComponent<TMPro.TextMeshProUGUI>().color = color;
		}

	}
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
		firstDigit.enabled = enabled;
		secondDigit.enabled = enabled;
		thirdDigit.enabled = enabled;
		if(gameMode != 0) {
			fourthDigit.enabled = enabled;
		}
	}
	private void NewGame() {
		changeColor2(new Color32(0, 255, 0, 220), score1);
		//changeColor(new Color32(0, 255, 0, 220), "Score1");
		//firstDigit.color = new Color32(0, 255, 0, 220);
		//secondDigit.color = new Color32(0, 255, 0, 220);
		//thirdDigit.color = new Color32(0, 255, 0, 220);		
		//fourthDigit.color = new Color32(0, 255, 0, 220);
		//dot.color = new Color32(0, 255, 0, 220);

		//firstDigit4.text = "";
		//secondDigit4.text = "";
		//thirdDigit4.text = "";
		//fourthDigit4.text = "";

		//firstDigit3.text = "";
		//secondDigit3.text = "";
		//thirdDigit3.text = "";
		//fourthDigit3.text = "";

		//firstDigit2.text = "";
		//secondDigit2.text = "";
		//thirdDigit2.text = "";
		//fourthDigit2.text = "";

		//firstDigit1.text = "";
		//secondDigit1.text = "";
		//thirdDigit1.text = "";
		//fourthDigit1.text = "";

		//firstDigit.text = "";
		//secondDigit.text = "";
		//thirdDigit.text = "";
		//fourthDigit.text = "";

		ResetScore(score1, true, true);
		ResetScore(score2, true, false);
		ResetScore(score3, true, false);
		ResetScore(score4, true, false);
		ResetScore(score5, true, false);

		//totalScoreFirstDigit.text = "";
		//totalScoreSecondDigit.text = "";
		//totalScoreThirdDigit.text = "";
		//totalScoreFourthDigit.text = "0";

		ResetScore(totalScores, true, true);


		totalThrowsFirstDigit.text = "";
		totalThrowsSecondDigit.text = "0";
		dot2.text = "";
		dot3.text = "";
		dot4.text = "";

		totalLifesFirstDigit.text = "";
		totalLifes = 0;

		if(gameMode == 0) {					// no decimal
			totalScoreDot.text = "";
			dot.text = "";
			dot1.text = "";
			dot2.text = "";
			dot3.text = "";
			dot4.text = "";
			totalScoreFifthDigit.text = "";
			gameModeMultiplyer = 1;
		}
		else if((gameMode == 1) || (gameMode == 2) || (gameMode == 3)) {
			totalScoreDot.text = ".";
			dot.text = ".";
			dot1.text = ".";
			if(gameMode != 2) {
				dot2.text = ".";
				dot3.text = ".";
				dot4.text = ".";
				if(gameMode == 3) {
					totalLifesFirstDigit.text = "1";
					totalLifes = 1;
				}
			}
			
			totalScoreFifthDigit.text = "0";
			gameModeMultiplyer = 10;
		}

		totalScore = 0;
		oldScore = 0;
		oldScore2 = 0;
		totalThrows = 0;
		gameOver = false;


	}
	

	private void GameOver() {
		//changeColor(new Color32(255, 0, 0, 220), "Score1");
		changeColor2(new Color32(255, 0, 0, 220), score1);
		//firstDigit.color = new Color32(255, 0, 0, 220);
		//secondDigit.color = new Color32(255, 0, 0, 220);
		//thirdDigit.color = new Color32(255, 0, 0, 220);
		//fourthDigit.color = new Color32(255, 0, 0, 220);
		//dot.color = new Color32(255, 0, 0, 220);
		if(totalLifes > 0) {
			totalLifes -= 1;
			totalLifesFirstDigit.text = totalLifes.ToString();
			lostLife = true;
		} else {
			gameOver = true;
			Invoke("GameIsOverAfterDelay", 1f);
			//if(GameIsOver != null) {
				//Invoke("GameIsOverAfterDelay", 0.5f);
				//GameIsOver.Invoke();
			//}
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
			//changeColor(new Color32(0, 255, 0, 220), "Score1");
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
				dot2.text = ".";
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

				//firstDigit4.text = firstDigit3.text;
				//secondDigit4.text = secondDigit3.text;
				//thirdDigit4.text = thirdDigit3.text;
				//fourthDigit4.text = fourthDigit3.text;

				//firstDigit3.text = firstDigit2.text;
				//secondDigit3.text = secondDigit2.text;
				//thirdDigit3.text = thirdDigit2.text;
				//fourthDigit3.text = fourthDigit2.text;

				//firstDigit2.text = firstDigit1.text;
				//secondDigit2.text = secondDigit1.text;
				//thirdDigit2.text = thirdDigit1.text;
				//fourthDigit2.text = fourthDigit1.text;

			}
			MoveText(score1, score2);
			//firstDigit1.text = firstDigit.text;
			//secondDigit1.text = secondDigit.text;
			//thirdDigit1.text = thirdDigit.text;
			//fourthDigit1.text = fourthDigit.text;
		} else {
			skipMoveDown = false;
		}
		string totThrow = string.Format("{0:00}", totalThrows);
		if(totalThrows >= 10) {
			totalThrowsFirstDigit.text = totThrow[0].ToString();
		} else {
			totalThrowsFirstDigit.text = "";
		}
		totalThrowsSecondDigit.text = totThrow[1].ToString();

		if(gameMode == 0) {
			totScore = string.Format("{0:0000}", totalScore);
		} else {
			totScore = string.Format("{0:00000}", totalScore);
			totalScoreFifthDigit.text = totScore[4].ToString();
		}
		if(totalScore >= 1000 * gameModeMultiplyer) {
			totalScoreFirstDigit.text = totScore[0].ToString();
		} else {
			totalScoreFirstDigit.text = "";
		}
		if(totalScore >= 100 * gameModeMultiplyer) {
			totalScoreSecondDigit.text = totScore[1].ToString();
		} else {
			totalScoreSecondDigit.text = "";
		}
		if(totalScore >= 10 * gameModeMultiplyer) {
			totalScoreThirdDigit.text = totScore[2].ToString();
		} else {
			totalScoreThirdDigit.text = "";
		}
		totalScoreFourthDigit.text = totScore[3].ToString();



		if(gameMode == 0) {
			score = string.Format("{0:000}", point);
		} else {
			score = string.Format("{0:0000}", point);
			fourthDigit.text = score[3].ToString();
		}
		if(point >= 100 * gameModeMultiplyer) {
			firstDigit.text = score[0].ToString();
		} else {
			firstDigit.text = "";
		}
		if(point >= 10 * gameModeMultiplyer) {
			secondDigit.text = score[1].ToString();
		} else {
			secondDigit.text = "";
		}
		thirdDigit.text = score[2].ToString();







	}
}
