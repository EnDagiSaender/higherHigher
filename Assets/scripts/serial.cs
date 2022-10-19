using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
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
	[SerializeField] GameObject WinnerCanvas;


	[SerializeField] GameObject lagomUIElementPrefab;
	[SerializeField] GameObject scoreUIElementPrefab;
	[SerializeField] GameObject BigScoreUIElementPrefab;
	[SerializeField] GameObject SmallScoreUIElementPrefab;
	[SerializeField] GameObject TotalScoreUIElementPrefab;
	[SerializeField] GameObject TotalLivesUIElementPrefab;
	[SerializeField] GameObject TotalThrowsUIElementPrefab;


	[SerializeField] GameObject playerLagom;


	[SerializeField] Transform elementWrapper;
	List<GameObject> uiElements = new List<GameObject>();
	List<GameObject> uiElementsPlayers = new List<GameObject>();

	[SerializeField] Sprite Sprite1;
	[SerializeField] Sprite Sprite2;

	[SerializeField] GameObject PlayerNrPanel;

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
	//private int[] playerScores = new int[8];
	//private int[] playerLives = new int[] {0,0,0,0,0,0,0,0,0 };
	private int playerTurn = 0;// change to 1
	//private int lastPlayerTurn = 0;
	private int lowestScorePlayer;
	private int highestScorePlayer;
	private TextMeshProUGUI[] playerInfo = new TextMeshProUGUI[20];
	//private TextMeshProUGUI[] player1 = new TextMeshProUGUI[20];
	//private TextMeshProUGUI[] player2 = new TextMeshProUGUI[20];
	//private TextMeshProUGUI[] player3 = new TextMeshProUGUI[20];
	//private TextMeshProUGUI[] player4 = new TextMeshProUGUI[20];
	//private TextMeshProUGUI[] player5 = new TextMeshProUGUI[20];
	//private TextMeshProUGUI[] player6 = new TextMeshProUGUI[20];
	//private TextMeshProUGUI[] player7 = new TextMeshProUGUI[20];
	//private TextMeshProUGUI[] player8 = new TextMeshProUGUI[20];
	List<TextMeshProUGUI[]> playersList = new List<TextMeshProUGUI[]>();
	List<TextMeshProUGUI[]> playersListActiveDisplay = new List<TextMeshProUGUI[]>();
	List<int> playerScoreList = new List<int>();
	List<int> playerLivesList = new List<int>();
	List<int> nextPlayerTurnList = new List<int>();

	int lowNr;
	int highNr;
	private bool inBetween = false;


	private int winnerFontSizeMin = 55;
	private int winnerFontSizeMax = 84;
	private int winnerFontSize = 55;
	private int winnerFontSizeIncrease = 1;

	private Image playerFocusImage = null;
	//private Image[] playerFocusImage2 = null;
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
	//private bool even = false;
	//private bool blinkOn = false;
	private int totalScore = 0;
	private int totalThrows = 0;
	private bool gameOver = false;
	private int gameMode = 4;
	private int gameModeMultiplyer = 1;
	private string score = "   ";
	private string totScore = "    ";
	private int gameModeMax = 7;
	private int players = 0;
	private bool busyThinking = false;
	private bool gameStarted = true;



	private void AddPlayerPannel(Vector3 position) {
		var inst = Instantiate(lagomUIElementPrefab, position, Quaternion.identity);
		inst.transform.SetParent(MainCanvas, false);
		uiElementsPlayers.Add(inst);
		//print(uiElements.Count);
	}


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
			if(from[i].text != ".") {
				from[i].text = "";
			}
		}
	}
	private void RemoveAllListObjects() {
		foreach(GameObject t in uiElements) {
			Destroy(t);
		}
		uiElements.RemoveRange(0, uiElements.Count);
	}
	private void RemoveAllListObjectsPleyers() {
		foreach(GameObject t in uiElementsPlayers) {
			Destroy(t);
		}
		//uiElementsPlayers.RemoveRange(0, uiElementsPlayers.Count);
		uiElementsPlayers.Clear();
		playerLivesList.Clear();
		nextPlayerTurnList.Clear();
		playerScoreList.Clear();
		playersList.Clear();
		playersListActiveDisplay.Clear();
		players = 0;
		playerTurn = 0;
		PlayerNrPanel.SetActive(false);

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

	private void GetPlayerInfo() {
		TextMeshProUGUI[] temparray = uiElementsPlayers[players-1].GetComponentsInChildren<TextMeshProUGUI>();
		TextMeshProUGUI[] playerInfo = new TextMeshProUGUI[20];
		int w = 0;
		foreach(TextMeshProUGUI t in temparray) {
			if(t.text != "8") {
				playerInfo[w] = t;
				w++;
			}
		}
		playerInfo[0].text = "Player" + players.ToString();
		playersList.Add(playerInfo);
		//score1 = new TextMeshProUGUI[] { playerInfo[1], playerInfo[2], playerInfo[3], playerInfo[4], playerInfo[5] };

	}
	private void OnEnable() {
		EventManager.NewGame += newGame;
		EventManager.ChangedDir += changeGame;
		EventManager.NewScore += updateScore;

		//AddPlayerPannel(new Vector3(-506, 450, 0));
		//TextMeshProUGUI[] temparray = playerLagom.GetComponentsInChildren<TextMeshProUGUI>();
		//TextMeshProUGUI[] playerInfo = new TextMeshProUGUI[20];
		//int w = 0;
		//foreach(TextMeshProUGUI t in temparray) {
		//	if(t.text != "8") {
		//		playerInfo[w] = t;
		//		//print(t.text);
		//		w++;
		//	}
		//	//print(t.text);
		//}
		//playersList.Add(playerInfo);
		////score1 = new TextMeshProUGUI[] { playerInfo[1], playerInfo[2], playerInfo[3], playerInfo[4], playerInfo[5] };
		//print(score1[0].text);
		//print(playersList[0][1].text);
		//print(temparray2.Length);
		//GameObject[] gameObjects = playerLagom.gameObject.FindGameObjectsWithTag("Score1");
		//GameObject[] gameObjects = playerLagom.FindGameObjectsWithTag("Score1");
		//PlayerNrPanel.transform.localPosition = new Vector3(52, 760, 0);
		//PlayerNrPanel.transform.localPosition = new Vector3(112, 760, 0);
		//PlayerNrPanel.transform.localPosition = new Vector3(172, 760, 0);
		//PlayerNrPanel.transform.localPosition = new Vector3(234, 760, 0);
		//PlayerNrPanel.transform.localPosition = new Vector3(293, 760, 0);
		//PlayerNrPanel.transform.localPosition = new Vector3(355, 760, 0);
		//PlayerNrPanel.transform.localPosition = new Vector3(416, 760, 0);
		//PlayerNrPanel.transform.localPosition = new Vector3(477, 760, 0);

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




		//score1 = GetChildText(BigScoreUIElementPrefab, new Vector3(-154, 125, 0));
		//score2 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -122, 0));
		//score3 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -337, 0));
		//score4 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -554, 0));
		//score5 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -768, 0));

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
			
		//	if(blinkOn == false) {
			if(increse) {
				gameMode += 1;
				if(gameMode > gameModeMax) {
					gameMode = 0;////change to 0!!!!
				}
			} else {
				gameMode -= 1;
				if(gameMode < 0) {
					gameMode = gameModeMax;
				}
			}
			highScorePanel.SetActive(false);
			GameName.text = CurrentGame;
			//busyThinking = true;
			gameStarted = true;
			newGame(); ////change to NewGame if not working
			if(GameChanged != null) {
				GameChanged();
				}
			//}
		}
	}
	private void updateScore(int newScore) {
		if(gameOver == false && busyThinking == false) {// && blinkOn == false) {
			gameStarted = true;
			busyThinking = true;
			if(gameMode == 0) {
				UpdateDisplay(Mathf.RoundToInt((float)newScore / 10));
				busyThinking = false;
			} else if(gameMode == 5|| gameMode == 6 || gameMode == 7) {
				if(inBetween) {
					BetweenNumbers(newScore);
				} else {
					UpdateDisplayGameMode5(newScore);
				}
				
			} else {
				UpdateDisplay(newScore);
				busyThinking = false;
			}
			StartCoroutine(BlinkSegments(score1, 2, 0.3f));
			//			//blinkOn = true;
			
		}
		print("Not redy " + gameOver.ToString() + busyThinking.ToString());
	}
	private void MoveLowerHigherScore(int point) {
		for(int i = 0; i < playerScoreList.Count; i++) {
			if(playerScoreList[i] == point) {
				MoveText(playersListActiveDisplay[i], MakeScoreArray(playersList[nextPlayerTurnList[i]-1], 7));
				ResetScore(playersListActiveDisplay[i], true, false);
				playersListActiveDisplay[i] = MakeScoreArray(playersList[nextPlayerTurnList[i] - 1], 7);
				//MoveText(playersListActiveDisplay[i], new TextMeshProUGUI[] { playersList[i][7], playersList[i][8], playersList[i][9], playersList[i][10], playersList[i][11] });
				break;
			}
		}
	}
	private TextMeshProUGUI[] MakeScoreArray(TextMeshProUGUI[] array, int start) {
		TextMeshProUGUI[] returnArray = new TextMeshProUGUI[5];
		int j = 0;
		for(int i = start; i < start + 5; i++) {
			returnArray[j] = array[i];
			j++;
		}
		return returnArray;


	}
	public void FlashActivePlayer() {
		if(playerFocusImage != null) {
			playerFocusImage.enabled = false;
		}
		playerFocusImage = uiElementsPlayers[nextPlayerTurnList[playerTurn] - 1].GetComponentInChildren<Image>();
		playerFocusImage.enabled = true;

		StartCoroutine(BlinkSegments(playersList[nextPlayerTurnList[playerTurn] - 1][0], 2, 0.5f));
		busyThinking = false;
	}
	private void UpdateDisplayGameMode5(int point) {
		
		if(playerTurn >= nextPlayerTurnList.Count) {
			playerTurn = 0;
			playerScoreList.Clear();
			foreach(TextMeshProUGUI[] ActiveDisplay in playersListActiveDisplay) {
				ResetScore(ActiveDisplay, true, false);
			}
			playersListActiveDisplay.Clear();
			CancelInvoke("NextRound");
			print("THIS IS NOT GOOD!2");
		}
		if(nextPlayerTurnList.Count <= 2) {

			BetweenNumbers(point);
			print("THIS IS NOT GOOD!3");
			return;
		}

		if (playerScoreList.Count > 1) {
			List<int> cloneScoreList = new List<int>(playerScoreList);
			cloneScoreList.Sort();
			if(cloneScoreList[0] > point) {
				print("x1");
				lowestScorePlayer = playerTurn;
				MoveLowerHigherScore(cloneScoreList[0]);
				score1 = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn] - 1] , 1);
			} else if(cloneScoreList[cloneScoreList.Count - 1] < point) {
				print("x2");
				highestScorePlayer = playerTurn;
				MoveLowerHigherScore(cloneScoreList[cloneScoreList.Count - 1]);
				score1 = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn] - 1], 13);
			} else {
				print("x3");
				score1 = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn] - 1], 7);
			}
		} else if(playerScoreList.Count == 1) {
			if(playerScoreList[0] < point) {
				print("x4");
				lowestScorePlayer = 0;
				highestScorePlayer = playerTurn;
				MoveText(playersListActiveDisplay[0], MakeScoreArray(playersList[nextPlayerTurnList[playerTurn - 1] - 1], 1));
				ResetScore(playersListActiveDisplay[0], true, false);
				playersListActiveDisplay[0] = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn - 1] - 1], 1);
				score1 = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn] - 1], 13);
			} else {
				print("x5");
				lowestScorePlayer = playerTurn;
				highestScorePlayer = 0;
				MoveText(playersListActiveDisplay[0], MakeScoreArray(playersList[nextPlayerTurnList[playerTurn - 1] - 1], 13));
				ResetScore(playersListActiveDisplay[0], true, false);
				playersListActiveDisplay[0] = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn - 1] - 1], 13);
				score1 = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn] - 1], 1);
			}
			
		} else {
			print("x6");
			score1 = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn] - 1], 7);
		}
		playerScoreList.Add(point);
		playersListActiveDisplay.Add(score1);
		ScoreToDisplay(score1, point);
		playerTurn++;
		checkekIfNewRound();
		
	}

	private void checkekIfNewRound() {
		if(playerTurn >= nextPlayerTurnList.Count) {
			//if(nextPlayerTurnList.Count != 2) {
			if(inBetween == false) {
				if(lowestScorePlayer > highestScorePlayer) {
					RemovePlayerIfDead2(lowestScorePlayer);
					RemovePlayerIfDead2(highestScorePlayer);
				} else {
					RemovePlayerIfDead2(highestScorePlayer);
					RemovePlayerIfDead2(lowestScorePlayer);
				}
				if(nextPlayerTurnList.Count <= 2) {
					inBetween = true;
				}

			} else {
				// Make it display 2 times new round otherwise
				CancelInvoke("DisplayBetweenNumbers");
			}
			if(nextPlayerTurnList.Count < 2) {
				Winner();
			}
			
			CancelInvoke("NextRound");
			Invoke("NextRound", 2.5f);
			playerTurn = 0;
		} else {
			CancelInvoke("FlashActivePlayer");
			Invoke("FlashActivePlayer", 1.5f);
		}

	}
	private void Winner() {
		string winnerText;
		if(nextPlayerTurnList.Count == 0) {
			winnerText = "DRAW!";
		} else {
			winnerText = "Winner Player " + nextPlayerTurnList[0].ToString();
		}
		print(winnerText);
		WinnerCanvas.GetComponentInChildren<TextMeshProUGUI>().text = winnerText;
		WinnerCanvas.SetActive(true);
		

		//busyThinking = true;
		gameOver = true;


	}

	private void DisplayBetweenNumbers() {
		if(playerTurn == 0) {
			lowNr = UnityEngine.Random.Range(1, 8) * 50;
			highNr = lowNr + 50;
		}
		TextMeshProUGUI[] tempScore;// = new TextMeshProUGUI[5];
		score2 = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn] -1], 1);
		ScoreToDisplay(score2, lowNr);
		//StartCoroutine(BlinkSegments(score2, 1, 0.2f));
		playersListActiveDisplay.Add(score2);
		tempScore = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn] - 1], 13);
		ScoreToDisplay(tempScore, highNr);
		//StartCoroutine(BlinkSegments(tempScore, 1, 0.2f));
		playersListActiveDisplay.Add(tempScore);
		busyThinking = false;

	}
	private void BetweenNumbers(int point) {
		if(playerTurn >= nextPlayerTurnList.Count) {
			playerTurn = 0;
			playerScoreList.Clear();
			foreach(TextMeshProUGUI[] ActiveDisplay in playersListActiveDisplay) {
				ResetScore(ActiveDisplay, true, false);
			}
			playersListActiveDisplay.Clear();
			CancelInvoke("NextRound");
			print("THIS IS NOT GOOD!");
		}

		score1 = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn] - 1], 7);
		ScoreToDisplay(score1, point);
		if(point < lowNr || point > highNr) {
			changeColor2(new Color32(255, 0, 0, 220), score1);
			RemovePlayerIfDead2(playerTurn);
			//if(nextPlayerTurnList.Count == 1) {
			//	Winner();
			//}
		} else {
			changeColor2(new Color32(0, 255, 0, 220), score1);
		}
		playersListActiveDisplay.Add(score1);
		playerTurn++;
		Invoke("DisplayBetweenNumbers", 1.5f);
		checkekIfNewRound();
		 
		//if(playerTurn != 0) {
		//	Invoke("DisplayBetweenNumbers", 1.5f);
		//	//DisplayBetweenNumbers();
		//}
	}
	private void ScoreToDisplay(TextMeshProUGUI[] display,int score) {
		string stringScore = string.Format("{0:0000}", score);
		display[3].text = stringScore[3].ToString();
		if(score >= 1000) {
			display[0].text = stringScore[0].ToString();
		} else {
			display[0].text = "";
		}
		if(score >= 100) {
			display[1].text = stringScore[1].ToString();
		} else {
			display[1].text = "";
		}
		display[2].text = stringScore[2].ToString();

	}
	private void Shuffle(List<int> a) {
		// Loop array
		for(int i = a.Count - 1; i > 0; i--) {
			// Randomize a number between 0 and i (so that the range decreases each time)
			int rnd = UnityEngine.Random.Range(0, i);

			// Save the value of the current i, otherwise it'll overwrite when we swap the values
			int temp = a[i];

			// Swap the new and old values
			a[i] = a[rnd];
			a[rnd] = temp;
		}
	}
	private void NextRound() {
		playerTurn = 0;
		if(gameMode == 6 || gameMode == 7) {
			Shuffle(nextPlayerTurnList);
		}
		//nextPlayerTurnList = nextPlayerTurnList.OrderBy(x => Random.value).ToList();
		playerScoreList.Clear();
		foreach(TextMeshProUGUI[] ActiveDisplay in playersListActiveDisplay) {
			ResetScore(ActiveDisplay, true, false);
		}
		playersListActiveDisplay.Clear();
		FlashActivePlayer();
		if(inBetween) {
			DisplayBetweenNumbers();
		}
	}
	//private void RemovePlayerIfDead(int playerNr) {
	//	playerLivesList[playerNr]--; //playerLivesList[playerNr]--;
	//	playersList[nextPlayerTurnList[playerNr] - 1][19].text = playerLivesList[playerNr].ToString();
	//	StartCoroutine(BlinkSegments(playersList[nextPlayerTurnList[playerNr] - 1][19], 4, 0.2f));

	//	if(playerLivesList[playerNr] <= 0) {
	//		print("player " + nextPlayerTurnList[playerNr] + " is dead!");
	//		playersList[nextPlayerTurnList[playerNr] - 1][0].text = "";
	//		nextPlayerTurnList.RemoveAt(playerNr);
	//		playerLivesList.RemoveAt(playerNr);
	//	}
	//}
	private void RemovePlayerIfDead2(int playerNr) {
		playerLivesList[nextPlayerTurnList[playerNr] - 1]--; //playerLivesList[playerNr]--;
		playersList[nextPlayerTurnList[playerNr] - 1][19].text = playerLivesList[nextPlayerTurnList[playerNr] - 1].ToString();
		StartCoroutine(BlinkSegments(playersList[nextPlayerTurnList[playerNr] - 1][19], 4, 0.2f));

		if(playerLivesList[nextPlayerTurnList[playerNr] - 1] <= 0) {
			print("player " + nextPlayerTurnList[playerNr] + " is dead!");
			playersList[nextPlayerTurnList[playerNr] - 1][0].text = "";
			nextPlayerTurnList.RemoveAt(playerNr);
			if(inBetween) {
				playerTurn--;
			}
			//playerLivesList.RemoveAt(playerNr);
		}
	}

	private void AddPlayer() {
		if(PlayerNrPanel.activeSelf == false) {
			PlayerNrPanel.SetActive(true);
		}
		players++;
		if(players > 8) {
			players = 1;
			nextPlayerTurnList.Clear();
		}
		switch(players) {
			case 1:
				AddPlayerPannel(new Vector3(-430, 450, 0));
				GetPlayerInfo();
				PlayerNrPanel.transform.localPosition = new Vector3(52, 760, 0);
				break;
			case 2:
				AddPlayerPannel(new Vector3(-430, 279, 0));
				//PlayerXPrefab.transform.localPosition = new Vector3(-506, 279, 0);
				PlayerNrPanel.transform.localPosition = new Vector3(112, 760, 0);
				GetPlayerInfo();
				break;
			case 3:
				AddPlayerPannel(new Vector3(-430, 90, 0));
				PlayerNrPanel.transform.localPosition = new Vector3(172, 760, 0);
				GetPlayerInfo();
				break;
			case 4:
				AddPlayerPannel(new Vector3(-430, -90, 0));
				PlayerNrPanel.transform.localPosition = new Vector3(234, 760, 0);
				GetPlayerInfo();
				break;
			case 5:
				AddPlayerPannel(new Vector3(-430, -258, 0));
				PlayerNrPanel.transform.localPosition = new Vector3(293, 760, 0);
				GetPlayerInfo();
				break;
			case 6:
				AddPlayerPannel(new Vector3(-430, -436, 0));
				PlayerNrPanel.transform.localPosition = new Vector3(355, 760, 0);
				GetPlayerInfo();
				break;
			case 7:
				AddPlayerPannel(new Vector3(-430, -613, 0));
				PlayerNrPanel.transform.localPosition = new Vector3(416, 760, 0);
				GetPlayerInfo();
				break;
			case 8:
				AddPlayerPannel(new Vector3(-430, -775, 0));
				PlayerNrPanel.transform.localPosition = new Vector3(477, 760, 0);
				GetPlayerInfo();
				break;
			default:
				PlayerNrPanel.transform.localPosition = new Vector3(52, 760, 0);
				break;
		}
		//playerLives[players - 1] = 2;
		playerLivesList.Add(2);
		nextPlayerTurnList.Add(players);
		CancelInvoke("FlashActivePlayer");
		Invoke("FlashActivePlayer", 2f);
		if(inBetween && nextPlayerTurnList.Count == 1) {
			busyThinking = true;
			Invoke("DisplayBetweenNumbers", 2f);
		}
		//foreach( int t in nextPlayerTurnList){
		//	print(t);

		//}
	}
	private void newGame() {
		if(setHighScorePanel.activeSelf == false){// && blinkOn == false) {
			highScorePanel.SetActive(false);
			gameOver = false;
			if(gameMode == 5 || gameMode == 6 || gameMode == 7) {
				if(gameStarted == true) {
					RemoveAllListObjects();
					RemoveAllListObjectsPleyers();
				}
				if(gameMode == 7) {
					inBetween = true;
				} else {
					inBetween = false;
				}
				AddPlayer();
			}
			gameStarted = false;
			//busyThinking = false;
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
					return "InBetween_1P";
				case 3:
					return "HigherHigher";
				case 4:
					return "LowerLower";
				case 5:
					return "Kasta_Lagom";
				case 6:
					return "Kasta_Lagom_Rnd";
				case 7:
					return "_InBetween_";
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


		if(WinnerCanvas.activeSelf) {
			flashTimer += deltaTime;
			if((flashTimer) > 0.04) {
				winnerFontSize = winnerFontSize + winnerFontSizeIncrease;
				if(winnerFontSize >= winnerFontSizeMax || winnerFontSize <= winnerFontSizeMin) {
					winnerFontSizeIncrease = winnerFontSizeIncrease * -1;
				}
				WinnerCanvas.GetComponentInChildren<TextMeshProUGUI>().fontSize = winnerFontSize;
				flashTimer = 0;
			}

		}
		//if(blinkOn == true) {
		//	flashTimer += deltaTime;
		//	if((flashTimer) > 0.1) {
		//		nrOfBlink += 1;
		//		if(nrOfBlink > 2) {
		//			blinkOn = false;
		//			nrOfBlink = 1;
		//		}
		//		flashTimer = 0;
		//		even = !even;
		//		blinkDisplay(even);

		//	} 

		//}

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
	private IEnumerator  BlinkSegments(TextMeshProUGUI[] segments, int nrOfBlinks, float blinkInterval) {
		for(int i = 0; i <= nrOfBlinks*2; i++){
			foreach(TextMeshProUGUI segment in segments) {
				if(i % 2 == 1) {
					segment.enabled = false;
				} else {
					segment.enabled = true;
				}
			}
			yield return new WaitForSeconds(blinkInterval);

		}

	}
	private IEnumerator BlinkSegments(TextMeshProUGUI segment, int nrOfBlinks, float blinkInterval) {
		for(int i = 0; i <= nrOfBlinks*2; i++) {
				if(i % 2 == 1) {
					segment.enabled = false;
				} else {
					segment.enabled = true;
				}
			yield return new WaitForSeconds(blinkInterval);

		}

	}


	//void readByte(int stat) {
	//	//if(stat == 2) {
	//	//print(stat);
	//	blinkOn = true;
	//	UpdateDisplay(stat);
	//	//} ///else {
	//	///	print(0);
	//	///}
	//}
	private void blinkDisplay(bool enabled) {
		score1[0].enabled = enabled;
		score1[1].enabled = enabled;
		score1[2].enabled = enabled;
		if(gameMode != 0) {
			score1[3].enabled = enabled;
		}
	}
	private void NewGame() {
		WinnerCanvas.SetActive(false);
		if(gameMode < 5) {
			MainCanvas.GetComponent<UnityEngine.UI.Image>().sprite = Sprite1;
			RemoveAllListObjects();
			RemoveAllListObjectsPleyers();
			score1 = GetChildText(BigScoreUIElementPrefab, new Vector3(-154, 125, 0));
			score2 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -122, 0));
			score3 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -337, 0));
			score4 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -554, 0));
			score5 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -768, 0));
			totalScores = GetChildText(TotalScoreUIElementPrefab, new Vector3(-196, 716, 0));
			totalthrows = GetChildText(TotalThrowsUIElementPrefab, new Vector3(329, 708, 0));
			totalLives = GetChildText(TotalLivesUIElementPrefab, new Vector3(329, 385, 0));
			GameName.GetComponentInParent<Transform>().localPosition =  new Vector3(-129, 469, 0);

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
			} else if((gameMode == 1) || (gameMode == 2) || (gameMode == 3) || gameMode == 4) {
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
		} else if(gameMode == 5 || gameMode == 6 || gameMode == 7) {
			MainCanvas.GetComponent<UnityEngine.UI.Image>().sprite = Sprite2;
			RemoveAllListObjects();
			GameName.GetComponentInParent<Transform>().localPosition = new Vector3(0, 888, 0);
			//print("removed");
			//score1 = GetChildText(BigScoreUIElementPrefab, new Vector3(-154, 125, 0));
			//score2 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -122, 0));
			//score3 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -337, 0));
			//score4 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -554, 0));
			//score5 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -768, 0));
			//totalScores = GetChildText(TotalScoreUIElementPrefab, new Vector3(-196, 716, 0));
			//totalthrows = GetChildText(TotalThrowsUIElementPrefab, new Vector3(329, 708, 0));
			//totalLives = GetChildText(TotalLivesUIElementPrefab, new Vector3(329, 385, 0));
			//GameName.GetComponentInParent<Transform>().localPosition = new Vector3(0, 888, 0);

		}


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
