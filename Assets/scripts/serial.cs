using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.IO.Ports;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;
using System.Linq;
//using System;



public class serial : MonoBehaviour
{
	public enum lang {
		Svenska,
		Engelska
	}
	public lang currentLanguage;
	public object[][] language = {
		new string[] {"Spelare", "FRISPEL", "L�GG P� MYNT", "TRYCK P� START", "KREDITER", "Kast nr: ", "Oavgjort!", "Vinnare Spelare", "Kast", "Kasta tillbaka bollen i spelet"},
		new string[] { "Player", "FREEPLAY" , "INSERT COIN" , "PUSH START", "CREDITS", "Throw nr: ", "DRAW!", "Winner Player", "Throws" , "Please throw the ball back into the game" }

	};
	
	//private string[][] svenska = {"0"}{"Spelare"};
	//private string[] engelska = { "Spelare" };
	public delegate void GameRunning();
	public static event GameRunning GameIsOver;

	public delegate void GameIsChanged();
	public static event GameIsChanged GameChanged;

	public delegate void BallGateOpen();
	public static event BallGateOpen OpenBallGate;

	public delegate void BallGateClose();
	public static event BallGateClose CloseBallGate;

	public delegate void ToggleCoinLock(bool on);
	public static event ToggleCoinLock SetCoinLock;

	private AudioClip[] throwSounds;
	private AudioClip[] coinSounds;
	private AudioClip[] winnerSounds;
	private AudioClip[] drawSounds;
	private AudioClip[] changeGameSounds;
	private AudioClip[] okSounds;
	private string[] soundFolders = { "throw", "winner", "ok", "draw", "changeGame", "coin" };
	[SerializeField] GameObject highScorePanel;
	[SerializeField] GameObject setHighScorePanel;
	[SerializeField] EventManager EventManager;
	[SerializeField] Transform MainCanvas;
	[SerializeField] GameObject WinnerCanvas;
	[SerializeField] GameObject SettingsCanvas;
	[SerializeField] GameObject MissingBallCanvas;
	[SerializeField] GameObject CreditsCanvas;
	[SerializeField] HighscoreUI HighscoreUI;
	[SerializeField] Image BgImage;

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
	List<GameObject> uiElementsPlayersSingleGame = new List<GameObject>();
	//List<Sprite> spriteBuffert = new List<Sprite>();
	//[SerializeField] Sprite Sprite1;
	//[SerializeField] Sprite Sprite2;


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
	[SerializeField] TextMeshProUGUI[] creditText;
	[SerializeField] TextMeshProUGUI insertCoinText;
	[SerializeField] TextMeshProUGUI highscoreGamename;
	[SerializeField] TextMeshProUGUI highscoreHighscore;
	[SerializeField] TextMeshProUGUI highscoreName;
	[SerializeField] TextMeshProUGUI highscoreScore;
	[SerializeField] TextMeshProUGUI highscoreThrows;
	[SerializeField] TextMeshProUGUI highscoreGamenameSmallDisplay;
	[SerializeField] TextMeshProUGUI highscoreHighscoreSmallDisplay;
	[SerializeField] TextMeshProUGUI highscoreNameSmallDisplay;
	[SerializeField] TextMeshProUGUI highscoreScoreSmallDisplay;
	[SerializeField] TextMeshProUGUI highscoreThrowsSmallDisplay;


	private TextMeshProUGUI[] totalLives;
	private TextMeshProUGUI[] totalthrows;
	private TextMeshProUGUI[] totalScores;
	
	private TextMeshProUGUI[] score1;
	private TextMeshProUGUI[] score2;
	private TextMeshProUGUI[] score3;
	private TextMeshProUGUI[] score4;
	private TextMeshProUGUI[] score5;
	private int playerTurn = 0;// change to 1
	//private int lastPlayerTurn = 0;
	private int lowestScorePlayer;
	private int highestScorePlayer;
	private TextMeshProUGUI[] playerInfo = new TextMeshProUGUI[20];
	List<TextMeshProUGUI[]> playersList = new List<TextMeshProUGUI[]>();
	List<TextMeshProUGUI[]> playersListActiveDisplay = new List<TextMeshProUGUI[]>();
	List<int> playerScoreList = new List<int>();
	List<int> playerLivesList = new List<int>();
	List<int> nextPlayerTurnList = new List<int>();

	int lowNr;
	int highNr;
	private bool inBetween = false;


	private int winnerFontSizeMin = 140;//36;//180;
	private int winnerFontSizeMax = 200;//59;//250;
	private int winnerFontSize = 140;//38;
	private int winnerFontSizeIncrease = 2;

	private Sprite[] bufferSprite = new Sprite[3];
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
	private bool ballOut = false;
	private bool skipMoveDown = false;
	private int oldScore = 0;
	private int oldScore2 = 0;
	private int nrOfBlink;
	private bool ballMissing = true;
	//private float timer = 0;
	private float flashTimer = 0;
	private float deltaTime;
	//private bool even = false;
	//private bool blinkOn = false;
	private int totalScore = 0;
	private int totalThrows = 0;
	private bool gameOver = true;
	private int[] gameModeChoices = {1,3,7};
	private int gameMode = 3;
	private int gameModeMultiplyer = 1;
	private string score = "   ";
	private string totScore = "    ";
	private int gameModeMax = 7;
	private int players = 0;
	private bool busyThinking = false;
	private bool gameStarted = false;
	private bool randomizeNewNr = true;
	private bool onePlayerGame = false;
	private bool wrapAroundPlayers = false;
	private bool firstTimeEasyMode = true;
	[SerializeField] AudioSource audioSource;
	private bool modifiedLives = false;
	private int credits = 3;
	public bool freePlay = true;
	public int prizeLagom = 16;
	public int prizeFaster = 20;
	Coroutine loadBgImageAllRutine = null;
	Coroutine changeBgImageRutine = null;
	Coroutine blinkSegmentsRutine = null;
	Coroutine flashPlayer = null;
	private void AddPlayerPannel(Vector3 position) {
		var inst = Instantiate(lagomUIElementPrefab, position, Quaternion.identity);
		inst.transform.SetParent(MainCanvas, false);
		uiElementsPlayers.Add(inst);
		//print(uiElements.Count);
	}
	private void ChangeLanguage() {
		if(!busyThinking) {
			busyThinking = true;
			if(currentLanguage == lang.Svenska) {
				currentLanguage = lang.Engelska;
				highscoreHighscore.text = "HighScores";
				highscoreName.text = "Name";
				highscoreScore.text = "Score";
				if(gameMode == 1) {
					highscoreThrows.text = "Km/h";
				} else {
					highscoreThrows.text = "Throws";
				}
			} else {
				currentLanguage = lang.Svenska;
				highscoreHighscore.text = "Topplista";
				highscoreName.text = "Namn";
				highscoreScore.text = "Po�ng";
				if(gameMode == 1) {
					highscoreThrows.text = "Km/h";
				} else {
					highscoreThrows.text = "Kast";
				}
			}
			highscoreHighscoreSmallDisplay.text = highscoreHighscore.text;
			highscoreNameSmallDisplay.text = highscoreName.text;
			highscoreScoreSmallDisplay.text = highscoreScore.text;
			highscoreThrowsSmallDisplay.text = highscoreThrows.text;
			highscoreGamename.text = DisplayCurrentGameName;
			highscoreGamenameSmallDisplay.text = highscoreGamename.text;
			//print(currentLanguage);
			UpdateCreditText();
			if(onePlayerGame) {
				for(int i = 0; i < uiElementsPlayersSingleGame.Count; i++) {
					//foreach(GameObject t in uiElementsPlayersSingleGame) {
					TextMeshProUGUI[] array = uiElementsPlayersSingleGame[i].GetComponentsInChildren<TextMeshProUGUI>();
					print(array[0].text);
					array[0].text = language[(int)currentLanguage][5].ToString() + (totalThrows - i).ToString();
				}
				playersList[0][0].text = language[(int)currentLanguage][5].ToString() + (totalThrows + 1).ToString();
			} else {
				for(int i = 0; i < playersList.Count; i++) {
					//foreach(TextMeshProUGUI[] t in playersList) {
					playersList[i][0].text = language[(int)currentLanguage][0].ToString() + (i + 1).ToString();
				}
			}
			//string texturePath = Application.streamingAssetsPath + "/picture/" + DisplayCurrentGameName + ".png";
			loadBgImageAllRutine = StartCoroutine(LoadBgImageAll());
			//StartCoroutine(LoadBgImage());
			GameName.text = DisplayCurrentGameName;
			DisplayMissingBall();
			//	//playersList[0][0].text = "Throw nr: " + totalThrows.ToString();
			//foreach(TextMeshProUGUI[] t in playersList) {
			//	if(onePlayerGame) {
			//		t[0].text = language[(int)currentLanguage][5].ToString();
			//		//uiElementsPlayersSingleGame[0];
			//		print(t[0].text.IndexOf(":"));
			//	} else {
			//		t[0].text = language[(int)currentLanguage][0].ToString();
			//	}
			//}
			//foreach(GameObject t in uiElementsPlayersSingleGame) {
			//	TextMeshProUGUI[] array = t.GetComponentsInChildren<TextMeshProUGUI>();
			//	print(array[0].text);
			//}
		}
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
	private void DisplayCredits() {
		highScorePanel.SetActive(false);
		CreditsCanvas.SetActive(true);
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
		foreach(GameObject t in uiElementsPlayersSingleGame) {
			Destroy(t);
		}
		//uiElementsPlayers.RemoveRange(0, uiElementsPlayers.Count);
		uiElementsPlayersSingleGame.Clear();
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
	public void UpdateCreditText() {
		//{ "Player", "FREEPLAY" , "INSERT COIN" , "PUSH START", "CREDITS"}
		foreach(TextMeshProUGUI text in creditText) {
			if(freePlay) {
				text.text = language[(int)currentLanguage][1].ToString();// FREEPLAY
			} else {
				text.text = language[(int)currentLanguage][4].ToString() + " " + credits.ToString(); //CREDITS
			}
		}
		if(credits > 0 || freePlay ) {
			insertCoinText.text = language[(int)currentLanguage][3].ToString(); // PUSH START
		} else {
			insertCoinText.text = language[(int)currentLanguage][2].ToString(); //INSERT COIN
		}

	}
	public void addCoin() {
		playCoin();
		credits++;
		UpdateCreditText();
	}
	private void removeCoin() {		
		if(credits > 0) {
			credits--;
			UpdateCreditText();
		}
		
	}
	public void clearCoin() {
		credits = 0;
		UpdateCreditText();
	}
	private void toggleTestMenu() {
		SettingsCanvas.SetActive(!SettingsCanvas.activeSelf);
	}
	private void toggleFreeplay() {
		freePlay = !freePlay;
		UpdateCreditText();
		saveGameSettings();
	}
	//public void updateFreeplay() {
	//	UpdateCreditText();
	//	saveGameSettings();
	//}
	private void loadGameSettings() {
		if(PlayerPrefs.HasKey("freeplay")) {
			freePlay = PlayerPrefs.GetInt("freeplay") == 1;
			print("Load Fisrt Time");
		} else {
			print("nothing to load");
		}
		prizeLagom = PlayerPrefs.HasKey("prizelagom") ? PlayerPrefs.GetInt("prizelagom"): 16;
		prizeFaster = PlayerPrefs.HasKey("prizefaster") ? PlayerPrefs.GetInt("prizefaster") : 20;
	}
	public void saveGameSettings() {
		PlayerPrefs.SetInt("freeplay", freePlay ? 1 : 0);
		PlayerPrefs.SetInt("prizelagom", prizeLagom);
		PlayerPrefs.SetInt("prizefaster", prizeFaster);

	}
	private void OnEnable() {
		currentLanguage = lang.Svenska; // S�tt spr�ket H�R
										//print(currentLanguage);
										//print((int)lang.Engelska);
		EventManager.ToggleTestMenu += toggleTestMenu;
		EventManager.ToggleFreeplay += toggleFreeplay;
		EventManager.BallFound += ballFound;
		EventManager.AddCoin += addCoin;
		EventManager.NewGame += newGame;
		EventManager.NewLanguage += ChangeLanguage;
		EventManager.ChangedDir += changeGame;
		EventManager.ChangeLives += ChangeTotalLives;
		EventManager.NewScore += updateScore;
		HighscoreUI.PlayWinnerSound += PlayWinner;
		HighscoreUI.PlayDrawSound += PlayDraw;
		totalScores = GetChildText(TotalScoreUIElementPrefab, new Vector3(-196, 716, 0));
		totalthrows = GetChildText(TotalThrowsUIElementPrefab, new Vector3(329, 708, 0));
		totalLives = GetChildText(TotalLivesUIElementPrefab, new Vector3(329, 385, 0));
		GameName.text = DisplayCurrentGameName;
		StartCoroutine(LoadAudioFiles()); //Load all audiofiles from StreamingAssets/audio folder
		loadBgImageAllRutine = StartCoroutine(LoadBgImageAll());
		Invoke("isBallBackAtSensor", 10f);
		loadGameSettings();
		//saveGameSettings();
		if(CloseBallGate != null) {
			CloseBallGate();
		}

	}

	private void GetPlayerInfo() {
		TextMeshProUGUI[] temparray = uiElementsPlayers[players - 1].GetComponentsInChildren<TextMeshProUGUI>();
		TextMeshProUGUI[] playerInfo = new TextMeshProUGUI[20];
		int w = 0;
		foreach(TextMeshProUGUI t in temparray) {
			if(t.text != "8") {
				playerInfo[w] = t;
				w++;
			}
		}
		playerInfo[0].text = language[(int)currentLanguage][0] + players.ToString();
		//playerInfo[0].text = "Player" + players.ToString();
		playersList.Add(playerInfo);
		//score1 = new TextMeshProUGUI[] { playerInfo[1], playerInfo[2], playerInfo[3], playerInfo[4], playerInfo[5] };

	}
	private IEnumerator LoadBgImageAll() {
		int nextGameMode;
		busyThinking = true;
		UnityWebRequest req = UnityWebRequestTexture.GetTexture(Application.streamingAssetsPath + "/picture/" + CurrentGameName(gameMode) + ".png");
		yield return req.SendWebRequest();
		var texture = DownloadHandlerTexture.GetContent(req);
		Sprite tempSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
		MainCanvas.GetComponent<UnityEngine.UI.Image>().sprite = tempSprite;
		bufferSprite[1] = tempSprite;
		for(int i = 0; i < gameModeChoices.Length; i++) {
			if(gameModeChoices[i] == gameMode) {
				if(i + 1 < gameModeChoices.Length) {
					nextGameMode = gameModeChoices[i + 1];
				} else {
					nextGameMode = gameModeChoices[0];
				}
				req = UnityWebRequestTexture.GetTexture(Application.streamingAssetsPath + "/picture/" + CurrentGameName(nextGameMode) + ".png");
				yield return req.SendWebRequest();
				texture = DownloadHandlerTexture.GetContent(req);
				bufferSprite[2] = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
				//MainCanvas.GetComponent<UnityEngine.UI.Image>().sprite = bufferSprite[0];
				if(i == 0) {
					nextGameMode = gameModeChoices[gameModeChoices.Length - 1];
				} else {
					nextGameMode = gameModeChoices[i - 1];
				}
				req = UnityWebRequestTexture.GetTexture(Application.streamingAssetsPath + "/picture/" + CurrentGameName(nextGameMode) + ".png");
				yield return req.SendWebRequest();
				texture = DownloadHandlerTexture.GetContent(req);
				bufferSprite[0] = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
				busyThinking = false;
				loadBgImageAllRutine = null;
				break;
			}			
		}
	}

	private IEnumerator LoadBgImage() {
		UnityWebRequest req = UnityWebRequestTexture.GetTexture(Application.streamingAssetsPath + "/picture/" + CurrentGameName(gameMode) + ".png");// + DisplayCurrentGameName + ".png");
		yield return req.SendWebRequest();
		var texture = DownloadHandlerTexture.GetContent(req);
		Sprite tempSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
		MainCanvas.GetComponent<UnityEngine.UI.Image>().sprite = tempSprite;

	}
	private IEnumerator LoadBgImage(bool increse) {
		int nextGameMode;
		for(int i = 0; i < gameModeChoices.Length; i++) {
			if(gameModeChoices[i] == gameMode) {
				busyThinking = true;
				if(increse) {
					MainCanvas.GetComponent<UnityEngine.UI.Image>().sprite = bufferSprite[2];
					if(i + 1 < gameModeChoices.Length) {
						nextGameMode = gameModeChoices[i + 1];
					} else {
						nextGameMode = gameModeChoices[0];
					}
					UnityWebRequest req = UnityWebRequestTexture.GetTexture(Application.streamingAssetsPath + "/picture/" + CurrentGameName(nextGameMode) + ".png");
					yield return req.SendWebRequest();
					var texture = DownloadHandlerTexture.GetContent(req);
					Sprite tempSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
					bufferSprite[0] = bufferSprite[1];
					bufferSprite[1] = bufferSprite[2];
					bufferSprite[2] = tempSprite;
				} else {
					MainCanvas.GetComponent<UnityEngine.UI.Image>().sprite = bufferSprite[0];
					if(i == 0) {
						nextGameMode = gameModeChoices[gameModeChoices.Length - 1];
					} else {
						nextGameMode = gameModeChoices[i - 1];
					}
					UnityWebRequest req = UnityWebRequestTexture.GetTexture(Application.streamingAssetsPath + "/picture/" + CurrentGameName(nextGameMode) + ".png");
					yield return req.SendWebRequest();
					var texture = DownloadHandlerTexture.GetContent(req);
					Sprite tempSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
					bufferSprite[2] = bufferSprite[1];
					bufferSprite[1] = bufferSprite[0];
					bufferSprite[0] = tempSprite;
				}
				busyThinking = false;
				changeBgImageRutine = null;
				break;
			}
		}
	}
	private IEnumerator LoadAudioFiles() {
		for(int j = 0; j < soundFolders.Length; j++) {
			string referenceFolder = Application.streamingAssetsPath + "/audio/" + soundFolders[j] + "/";
			string[] soundsNames = Directory.GetFiles(referenceFolder, "*.wav");
			AudioClip[] soundClip = new AudioClip[soundsNames.Length];
			for(int i = 0; i < soundsNames.Length; i++) {
				UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(soundsNames[i], AudioType.WAV);
				yield return req.SendWebRequest();
				soundClip[i] = DownloadHandlerAudioClip.GetContent(req);
				if(soundsNames[i].Length > 9) {
					soundClip[i].name = soundsNames[i].Remove(0, referenceFolder.Length);
					soundClip[i].name = soundClip[i].name.Remove(soundClip[i].name.Length-4);
				}
			}
			List<AudioClip> stringList = soundClip.ToList();
			soundsNames = Directory.GetFiles(Application.streamingAssetsPath + "/audio/" + soundFolders[j] + "/", "*.ogg");
			soundClip = new AudioClip[soundsNames.Length];
			for(int i = 0; i < soundsNames.Length; i++) {
				UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(soundsNames[i], AudioType.OGGVORBIS);
				yield return req.SendWebRequest();
				soundClip[i] = DownloadHandlerAudioClip.GetContent(req);
				if(soundsNames[i].Length > 9) {
					soundClip[i].name = soundsNames[i].Remove(0, referenceFolder.Length);
					soundClip[i].name = soundClip[i].name.Remove(soundClip[i].name.Length - 4);
				}
			}
			stringList.AddRange(soundClip.ToList());
			soundsNames = Directory.GetFiles(Application.streamingAssetsPath + "/audio/" + soundFolders[j] + "/", "*.mp3");
			soundClip = new AudioClip[soundsNames.Length];
			for(int i = 0; i < soundsNames.Length; i++) {
				UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(soundsNames[i], AudioType.MPEG);
				yield return req.SendWebRequest();
				soundClip[i] = DownloadHandlerAudioClip.GetContent(req);
				if(soundsNames[i].Length > 9) {
					soundClip[i].name = soundsNames[i].Remove(0, referenceFolder.Length);
					soundClip[i].name = soundClip[i].name.Remove(soundClip[i].name.Length - 4);
				}
			}
			stringList.AddRange(soundClip.ToList());
			soundClip = stringList.ToArray();

			switch(soundFolders[j]) {
				case "throw":
					throwSounds = soundClip;
					break;
				case "winner":
					winnerSounds = soundClip;
					break;
				case "ok":
					okSounds = soundClip;
					break;
				case "draw":
					drawSounds = soundClip;
					break;
				case "changeGame":
					changeGameSounds = soundClip;
					break;
				case "coin":
					coinSounds = soundClip;
					break;

				default:
					print("no sound variable" + soundFolders[j]);
					break;
			}

		}
		print("Done loading audio files");

	}
	private void ballFound() {
		ballMissing = false;
		if(SetCoinLock != null) {
			SetCoinLock(false);
		}
		DisplayMissingBall(false);
		
	}
	private void OnDisable() {
		EventManager.BallFound -= ballFound;
		EventManager.ToggleTestMenu -= toggleTestMenu;
		EventManager.ToggleFreeplay -= toggleFreeplay;
		EventManager.AddCoin -= addCoin;
		EventManager.NewGame -= newGame;
		EventManager.NewLanguage -= ChangeLanguage;
		EventManager.ChangedDir -= changeGame;
		EventManager.NewScore -= updateScore;
		EventManager.ChangeLives -= ChangeTotalLives;
		HighscoreUI.PlayWinnerSound -= PlayWinner;
		HighscoreUI.PlayDrawSound -= PlayDraw;
	}
	private void changeGame(bool increse) {
		if(setHighScorePanel.activeSelf == false && SettingsCanvas.activeSelf == false && busyThinking == false && (freePlay || !gameStarted)) {
			PlayChangeGame();
			if(!gameStarted && !gameOver) {
				ballOut = true;
				if(!freePlay) {
					addCoin();
				}
				
			}
			gameStarted = false;
			for(int i = 0; i< gameModeChoices.Length; i++){
				if(gameModeChoices[i] == gameMode) {
					if(increse) {
						if(i + 1 < gameModeChoices.Length) {
							gameMode = gameModeChoices[i + 1];
							//spriteBuffert.RemoveAt(0);
							//spriteBuffert.Add();
						} else {
							gameMode = gameModeChoices[0];
						}
					} else {
						if(i == 0) {
							gameMode = gameModeChoices[gameModeChoices.Length - 1];
						} else {
							gameMode = gameModeChoices[i - 1];
						}
					}
					if(gameMode == 1) {
							highscoreThrows.text = "Km/h";
					} else {
						highscoreThrows.text = language[(int)currentLanguage][8].ToString();
						//if(currentLanguage == lang.Engelska) {
						//	highscoreThrows.text = "Throws";
						//} else {
						//	highscoreThrows.text = "Kast";
						//}
					}
					StopAllCoroutines();
					changeBgImageRutine = StartCoroutine(LoadBgImage(increse));
					highscoreThrowsSmallDisplay.text = highscoreThrows.text;					
					break;
				}
			}
			//if(increse) {
			//	gameMode += 1;
			//	if(gameMode > gameModeMax) {
			//		gameMode = 0;////change to 0!!!!
			//	}
			//} else {
			//	gameMode -= 1;
			//	if(gameMode < 0) {
			//		gameMode = gameModeMax;
			//	}
			//}
			//StopAllCoroutines();
			//StopCoroutine(flashPlayer);
			//StopCoroutine(blinkSegmentsRutine);
			CancelInvoke("FlashActivePlayer");
			//if(flashPlayer != null) {
			//	StopCoroutine(flashPlayer);
			//}
			highScorePanel.SetActive(false);
			GameName.text = DisplayCurrentGameName;
			//gameStarted = true; // to not create more players while changing gamemode
			//newGame(); ////change to NewGame if not working
			gameOver = true;
			ResetScores();
			CreditsCanvas.SetActive(true);
			if(GameChanged != null) {
				GameChanged();
				}
		}
	}
	private void playCoin() {
		AudioClip randomClip = coinSounds[Random.Range(0, coinSounds.Length)];
		if(randomClip.name.Length > 5 && (randomClip.name.Substring(randomClip.name.Length - 6, 3) == "vol")) {
			float volume = (float)(int.Parse(randomClip.name.Substring(randomClip.name.Length - 3, 3))) / 100;
			audioSource.PlayOneShot(coinSounds[Random.Range(0, coinSounds.Length)], volume);
		} else {
			audioSource.PlayOneShot(coinSounds[Random.Range(0, coinSounds.Length)], Random.Range(0.75f, 1f));
		}
	}
	private void PlayThrow() {
		AudioClip randomClip = throwSounds[Random.Range(0, throwSounds.Length)];
		if(randomClip.name.Length > 5 && (randomClip.name.Substring(randomClip.name.Length - 6, 3) == "vol")) {
			float volume = (float)(int.Parse(randomClip.name.Substring(randomClip.name.Length - 3, 3))) / 100;
			audioSource.PlayOneShot(throwSounds[Random.Range(0, throwSounds.Length)], volume);
		} else {
			audioSource.PlayOneShot(throwSounds[Random.Range(0, throwSounds.Length)], Random.Range(0.75f, 1f));
		}
	}
	private void PlayWinner() {
		AudioClip randomClip = winnerSounds[Random.Range(0, winnerSounds.Length)];
		if(randomClip.name.Length > 5 && (randomClip.name.Substring(randomClip.name.Length - 6, 3) == "vol")) {
			float volume = (float)(int.Parse(randomClip.name.Substring(randomClip.name.Length - 3, 3))) / 100;
			audioSource.PlayOneShot(winnerSounds[Random.Range(0, winnerSounds.Length)], volume);
		} else {
			audioSource.PlayOneShot(winnerSounds[Random.Range(0, winnerSounds.Length)], Random.Range(0.4f, 0.5f));
		}
	}
	private void PlayDraw() {
		AudioClip randomClip = drawSounds[Random.Range(0, drawSounds.Length)];
		if(randomClip.name.Length > 5 && (randomClip.name.Substring(randomClip.name.Length - 6, 3) == "vol")) {
			float volume = (float)(int.Parse(randomClip.name.Substring(randomClip.name.Length - 3, 3))) / 100;
			audioSource.PlayOneShot(drawSounds[Random.Range(0, drawSounds.Length)], volume);
		} else {
			audioSource.PlayOneShot(drawSounds[Random.Range(0, drawSounds.Length)], Random.Range(0.4f, 0.5f));
		}
	}
	public void PlayChangeGame() {
		AudioClip randomClip = changeGameSounds[Random.Range(0, changeGameSounds.Length)];
		if(randomClip.name.Length > 5 && (randomClip.name.Substring(randomClip.name.Length - 6, 3) == "vol")) {
			float volume = (float)(int.Parse(randomClip.name.Substring(randomClip.name.Length - 3, 3))) / 100;
			audioSource.PlayOneShot(changeGameSounds[Random.Range(0, changeGameSounds.Length)], volume);
		} else {
			audioSource.PlayOneShot(changeGameSounds[Random.Range(0, changeGameSounds.Length)], Random.Range(0.7f, 1f));
		}
	}
	public void PlayOk() {
		AudioClip randomClip = okSounds[Random.Range(0, okSounds.Length)];
		if(randomClip.name.Length > 5 && (randomClip.name.Substring(randomClip.name.Length - 6, 3) == "vol")) {
			float volume = (float)(int.Parse(randomClip.name.Substring(randomClip.name.Length - 3, 3))) / 100;
			audioSource.PlayOneShot(okSounds[Random.Range(0, okSounds.Length)], volume);
		} else {
			audioSource.PlayOneShot(okSounds[Random.Range(0, okSounds.Length)], Random.Range(0.4f, 0.5f));
		}
	}

	private bool ToFewPlayers() {
		//bool returnBool = false;
		if(!gameStarted){
			if(gameMode == 5 || gameMode == 6) {
				if(players < 3)
				return true;
			} 
			//else if(gameMode == 7) {
			//	if(players < 2)
			//		return true;
			//}
		}
		return false;
	}

	private void updateScore(int newScore) {
		if(gameOver == false && busyThinking == false && !ToFewPlayers()) {// && blinkOn == false) {
			PlayThrow();
			//AudioSource.PlayOneShot(throwSounds[0]);
			gameStarted = true;
			busyThinking = true;
			if(gameMode == 0) {
				UpdateDisplay(Mathf.RoundToInt((float)newScore / 10));
				//busyThinking = false;
			} else if(gameMode == 5 || gameMode == 6 || gameMode == 7) {
				if(inBetween) {
					BetweenNumbers(newScore);
				} else {
					UpdateDisplayGameMode5(newScore);
				}

			} else {
				UpdateDisplay(newScore);
				//busyThinking = false;
			}

			//blinkSegmentsRutine = StartCoroutine(BlinkSegments(score1, 2, 0.3f));
			StartCoroutine(BlinkSegments(score1, 2, 0.3f));

			//			//blinkOn = true;

		}
		//else {
		//	print("Not redy " + gameOver.ToString() + busyThinking.ToString() + ToFewPlayers().ToString());

		//}
		
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

		flashPlayer = StartCoroutine(BlinkSegments(playersList[nextPlayerTurnList[playerTurn] - 1][0], 2, 0.5f));
		busyThinking = false;
	}
	private void UpdateDisplayGameMode5(int point) {
		
		//if(playerTurn >= nextPlayerTurnList.Count) {
		//	playerTurn = 0;
		//	playerScoreList.Clear();
		//	foreach(TextMeshProUGUI[] ActiveDisplay in playersListActiveDisplay) {
		//		ResetScore(ActiveDisplay, true, false);
		//	}
		//	playersListActiveDisplay.Clear();
		//	CancelInvoke("NextRound");
		//	print("THIS IS NOT GOOD!2");
		//}
		//if(nextPlayerTurnList.Count <= 2) {

		//	BetweenNumbers(point);
		//	print("THIS IS NOT GOOD!3");
		//	return;
		//}

		if (playerScoreList.Count > 1) {
			List<int> cloneScoreList = new List<int>(playerScoreList);
			cloneScoreList.Sort();
			if(cloneScoreList[0] > point) {
				//print("x1");
				lowestScorePlayer = playerTurn;
				MoveLowerHigherScore(cloneScoreList[0]);
				score1 = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn] - 1] , 1);
			} else if(cloneScoreList[cloneScoreList.Count - 1] < point) {
				//print("x2");
				highestScorePlayer = playerTurn;
				MoveLowerHigherScore(cloneScoreList[cloneScoreList.Count - 1]);
				score1 = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn] - 1], 13);
			} else {
				//print("x3");
				score1 = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn] - 1], 7);
			}
		} else if(playerScoreList.Count == 1) {
			if(playerScoreList[0] < point) {
				//print("x4");
				lowestScorePlayer = 0;
				highestScorePlayer = playerTurn;
				MoveText(playersListActiveDisplay[0], MakeScoreArray(playersList[nextPlayerTurnList[playerTurn - 1] - 1], 1));
				ResetScore(playersListActiveDisplay[0], true, false);
				playersListActiveDisplay[0] = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn - 1] - 1], 1);
				score1 = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn] - 1], 13);
			} else {
				//print("x5");
				lowestScorePlayer = playerTurn;
				highestScorePlayer = 0;
				MoveText(playersListActiveDisplay[0], MakeScoreArray(playersList[nextPlayerTurnList[playerTurn - 1] - 1], 13));
				ResetScore(playersListActiveDisplay[0], true, false);
				playersListActiveDisplay[0] = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn - 1] - 1], 13);
				score1 = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn] - 1], 1);
			}
			
		} else {
			//print("x6");
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
					totalThrows = 0;
				}

			} else {
				// Make it display 2 times new round otherwise
				CancelInvoke("DisplayBetweenNumbers");
			}
			if(nextPlayerTurnList.Count < 2 && !onePlayerGame) {
				busyThinking = true;
				gameOverHelper();
				Invoke("Winner", 1.5f);
				return;
			} else if(nextPlayerTurnList.Count == 0) {
				busyThinking = true;
				gameOverHelper();
				Invoke("GameIsOverAfterDelay", 1f);
				return;
			}
			
			CancelInvoke("NextRound");
			Invoke("NextRound", 2.5f);
			playerTurn = 0;
			randomizeNewNr = true;
		} else {
			CancelInvoke("FlashActivePlayer");
			Invoke("FlashActivePlayer", 1.5f);
		}

	}
	private void Winner() {
		string winnerText;
		if(nextPlayerTurnList.Count == 0) {
			PlayDraw();
			//audioSource.PlayOneShot(drawSounds[Random.Range(0, drawSounds.Length)], Random.Range(0.75f, 1f));
			winnerText = language[(int)currentLanguage][6].ToString(); //"DRAW!";
		} else {
			PlayWinner();
			winnerText = language[(int)currentLanguage][7].ToString() + nextPlayerTurnList[0].ToString();//"Winner Player " + nextPlayerTurnList[0].ToString();
		}
		//print(winnerText);
		busyThinking = false;
		WinnerCanvas.GetComponentInChildren<TextMeshProUGUI>().text = winnerText;
		WinnerCanvas.SetActive(true);
		//gameOver = true;
		//if(CloseBallGate != null) {
		//	CloseBallGate();
		//}
		gameStarted = false;
	}
	private void DisplayMissingBall() {
		string missingBallText;
		missingBallText = language[(int)currentLanguage][9].ToString();
		MissingBallCanvas.GetComponentInChildren<TextMeshProUGUI>().text = missingBallText;
	}
	private void DisplayMissingBall(bool disp) {
		string missingBallText;
		if(!disp) {
			MissingBallCanvas.SetActive(false);
		} else {
			missingBallText = language[(int)currentLanguage][9].ToString();
			MissingBallCanvas.GetComponentInChildren<TextMeshProUGUI>().text = missingBallText;
			MissingBallCanvas.SetActive(true);
		}
	}

	private void DisplayBetweenNumbers() {
		if(randomizeNewNr) {
			if(firstTimeEasyMode && totalThrows < 2) {
				switch(totalThrows) {
					case 0:
						lowNr = 100;
						highNr = 500;
						break;
					case 1:
						lowNr = 150;
						highNr = 400;
						break;
					default:
						print("totalThrows out of range");
						break;
				}
			} else {
				lowNr = UnityEngine.Random.Range(10, 35) * 10;
				if(totalThrows < 11) {
					highNr = lowNr + 120 - (totalThrows * 10);
				} else {
					highNr = lowNr + 120 - (10 * 10);
				}
			}
			randomizeNewNr = false;
		}
		TextMeshProUGUI[] tempScore;// = new TextMeshProUGUI[5];
		tempScore = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn] -1], 1);
		ScoreToDisplay(tempScore, lowNr);
		//StartCoroutine(BlinkSegments(score2, 1, 0.2f));
		playersListActiveDisplay.Add(tempScore);
		tempScore = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn] - 1], 13);
		ScoreToDisplay(tempScore, highNr);
		playersListActiveDisplay.Add(tempScore);
		busyThinking = false;

	}
	private void BetweenNumbers(int point) {
		//if(playerTurn >= nextPlayerTurnList.Count) {
		//	playerTurn = 0;
		//	playerScoreList.Clear();
		//	foreach(TextMeshProUGUI[] ActiveDisplay in playersListActiveDisplay) {
		//		ResetScore(ActiveDisplay, true, false);
		//	}
		//	playersListActiveDisplay.Clear();
		//	CancelInvoke("NextRound");
		//	print("THIS IS NOT GOOD!");
		//}

		score1 = MakeScoreArray(playersList[nextPlayerTurnList[playerTurn] - 1], 7);
		ScoreToDisplay(score1, point);
		if(point < lowNr || point > highNr) {
			changeColor2(new Color32(255, 0, 0, 220), score1);
			RemovePlayerIfDead2(playerTurn);
		} else {
			changeColor2(new Color32(0, 255, 0, 220), score1);
			if(onePlayerGame) {
				totalScore += Mathf.CeilToInt(60-Mathf.Abs(((highNr + lowNr)/2)-point));
				//print(Mathf.CeilToInt(60 - Mathf.Abs(((highNr + lowNr) / 2) - point)));
				//print(totalScore);
			}
		}
		playersListActiveDisplay.Add(score1);
		playerTurn++;
		Invoke("DisplayBetweenNumbers", 1.5f);
		checkekIfNewRound();
		 
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
		totalThrows++;		
		if(gameMode == 6){ //  || gameMode == 7) {
			Shuffle(nextPlayerTurnList);
		}
		if(onePlayerGame) {
			OnePlayerGame();
			return;
		}
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

	private void OnePlayerGame() {

		playersList[0][0].text = language[(int)currentLanguage][5].ToString() + totalThrows.ToString();
		//uiElementsPlayers[0].transform.localPosition = new Vector3(-430, 279, 0);
		uiElementsPlayersSingleGame.Insert(0, uiElementsPlayers[0]);
		uiElementsPlayers.Clear();
		playersList.Clear();
		for(int i = 0; i < uiElementsPlayersSingleGame.Count; i++) {
			switch(i) {
				case 0:
					uiElementsPlayersSingleGame[i].transform.localPosition = (new Vector3(-430, 279, 0));
					break;
				case 1:
					uiElementsPlayersSingleGame[i].transform.localPosition = (new Vector3(-430, 90, 0));
					break;
				case 2:
					uiElementsPlayersSingleGame[i].transform.localPosition = (new Vector3(-430, -90, 0));
					break;
				case 3:
					uiElementsPlayersSingleGame[i].transform.localPosition = (new Vector3(-430, -258, 0));
					break;
				case 4:
					uiElementsPlayersSingleGame[i].transform.localPosition = (new Vector3(-430, -436, 0));
					break;
				case 5:
					uiElementsPlayersSingleGame[i].transform.localPosition = (new Vector3(-430, -613, 0));
					break;
				case 6:
					uiElementsPlayersSingleGame[i].transform.localPosition = (new Vector3(-430, -775, 0));
					break;
				default:
					Destroy(uiElementsPlayersSingleGame[i]);
					uiElementsPlayersSingleGame.RemoveAt(i);
					break;
			}
		}

		AddPlayerPannel(new Vector3(-430, 450, 0));
		GetPlayerInfo();
		playersList[0][0].text = language[(int)currentLanguage][5].ToString() + (totalThrows+1).ToString();
		playersList[0][19].text = playerLivesList[0].ToString();
		playersListActiveDisplay.Clear();
		FlashActivePlayer();
		//PlayerNrPanel.transform.localPosition = new Vector3(45, 760, 0);
		//GetPlayerInfo();



		DisplayBetweenNumbers();
	}

	private void RemovePlayerIfDead2(int playerNr) {
		playerLivesList[nextPlayerTurnList[playerNr] - 1]--; //playerLivesList[playerNr]--;
		playersList[nextPlayerTurnList[playerNr] - 1][19].text = playerLivesList[nextPlayerTurnList[playerNr] - 1].ToString();
		flashPlayer = StartCoroutine(BlinkSegments(playersList[nextPlayerTurnList[playerNr] - 1][19], 4, 0.2f));

		if(playerLivesList[nextPlayerTurnList[playerNr] - 1] <= 0) {
			//print("player " + nextPlayerTurnList[playerNr] + " is dead!");
			playersList[nextPlayerTurnList[playerNr] - 1][0].text = "";
			nextPlayerTurnList.RemoveAt(playerNr);
			if(inBetween) {
				playerTurn--;
			}
		}
	}

	private void AddPlayer() {
		players++;
		if(players > 8) {
			RemoveAllListObjects();
			RemoveAllListObjectsPleyers();
			players = 1;
			wrapAroundPlayers = true;
		}
		if(PlayerNrPanel.activeSelf == false) {
			PlayerNrPanel.SetActive(true);
		}
		switch(players) {
			case 1:
				totalLifes = 2;
				AddPlayerPannel(new Vector3(-430, 450, 0));
				GetPlayerInfo();
				PlayerNrPanel.transform.localPosition = new Vector3(45, 760, 0);
				//wrapAroundPlayers = false;
				if(!wrapAroundPlayers) {
					randomizeNewNr = true;
				}
				wrapAroundPlayers = false;
				break;
			case 2:
				totalLifes = 4;
				AddPlayerPannel(new Vector3(-430, 279, 0));
				//PlayerXPrefab.transform.localPosition = new Vector3(-506, 279, 0);
				PlayerNrPanel.transform.localPosition = new Vector3(95, 760, 0);
				GetPlayerInfo();
				break;
			case 3:
				totalLifes = 3;
				AddPlayerPannel(new Vector3(-430, 90, 0));
				PlayerNrPanel.transform.localPosition = new Vector3(160, 760, 0);
				GetPlayerInfo();
				break;
			case 4:
				totalLifes = 2;
				AddPlayerPannel(new Vector3(-430, -90, 0));
				PlayerNrPanel.transform.localPosition = new Vector3(225, 760, 0);
				GetPlayerInfo();
				break;
			case 5:
				
				AddPlayerPannel(new Vector3(-430, -258, 0));
				PlayerNrPanel.transform.localPosition = new Vector3(287, 760, 0);
				GetPlayerInfo();
				break;
			case 6:
				AddPlayerPannel(new Vector3(-430, -436, 0));
				PlayerNrPanel.transform.localPosition = new Vector3(353, 760, 0);
				GetPlayerInfo();
				break;
			case 7:
				totalLifes = 1;
				AddPlayerPannel(new Vector3(-430, -613, 0));
				PlayerNrPanel.transform.localPosition = new Vector3(415, 760, 0);
				GetPlayerInfo();
				break;
			case 8:
				AddPlayerPannel(new Vector3(-430, -775, 0));
				PlayerNrPanel.transform.localPosition = new Vector3(478, 760, 0);
				GetPlayerInfo();
				break;
			default:
				break;
		}
		
		playerLivesList.Add(totalLifes);
		nextPlayerTurnList.Add(players);
		CancelInvoke("FlashActivePlayer");
		Invoke("FlashActivePlayer", 2f);
		if(inBetween && nextPlayerTurnList.Count == 1) {
			busyThinking = true;
			Invoke("DisplayBetweenNumbers", 0.5f);
			onePlayerGame = true;
		} else {
			onePlayerGame = false;
		}
		UpdateTotalLives();
	}
	private void gameOverHelper() {
		gameOver = true;
		ballOut = false;
		if(CloseBallGate != null) {
			CloseBallGate();
		}
		Invoke("isBallBackAtSensor", 8f);
	}
	private void isBallBackAtSensor() {
		if(!EventManager.IsBall) {
			if(!ballMissing) {
				ballMissing = true;
				print("ball Missing");
			}
			if(SetCoinLock != null) {
				SetCoinLock(true);
			}
			DisplayMissingBall(true);
		}
		Invoke("isBallBackAtSensor", 10f);
		
	}

	private void newGame() {
		print("trying to start new game");
		if(!busyThinking && !ballMissing && (freePlay || credits > 0 || (!gameOver && (gameMode == 5 || gameMode == 6 || gameMode == 7)))) {
			print("start new game");
			if(setHighScorePanel.activeSelf == false && SettingsCanvas.activeSelf == false) {// && blinkOn == false) {
				highScorePanel.SetActive(false);
				if(gameOver){
					CancelInvoke("isBallBackAtSensor");
					if(OpenBallGate != null) {
						OpenBallGate();
					}
					if(!freePlay) {
						removeCoin();
					}
				}
				
				if(gameMode == 7) {
					inBetween = true;
				} else {
					inBetween = false;
				}
				if(gameMode == 5 || gameMode == 6 || gameMode == 7) {
					if(gameStarted == true && freePlay || gameOver) {
						RemoveAllListObjects();
						RemoveAllListObjectsPleyers();
						totalLifes = 0;
						WinnerCanvas.SetActive(false);
						AddPlayer();
						//removeCoin();
					} else if(!gameStarted) {
						AddPlayer();
						//PlayOk();
					} else {
						print("xcvx");
						return;
					}
				} else {
					if(!gameStarted || freePlay) {
						ResetScores();
						//removeCoin();
					} else {
						return;
					}
				}
				gameOver = false;
				gameStarted = false;
				modifiedLives = false;
				totalScore = 0;
				totalThrows = 0;
				CreditsCanvas.SetActive(false);
				//ResetScores();




			}
		}
	}


	void Start()
    {
		ResetScores();
		UpdateCreditText();
		CreditsCanvas.SetActive(true);
		//StartCoroutine(BlinkSegments(insertCoinText, 0.5f));
	}

	public int Points {
		get {
			return totalScore;
		}
	}
	private string CurrentGameName(int gameModeNr) {
		switch(gameModeNr) {
			case 0:
		if(currentLanguage == lang.Svenska) {
			return "SnabbareSnabbare";
		} else {
			return "HigherHigher";
		}
					
				case 1:
					if(currentLanguage == lang.Svenska) {
			return "SnabbBoll";
		} else {
			return "FastBall";
		}					
				case 2:
					return "InBetween";
				case 3:
					if(currentLanguage == lang.Svenska) {
			return "SnabbSnabbare";
		} else {
			return "HigherHigher";
		}
				case 4:
					return "LowerLower";
				case 5:
					return "KastaLagom";
				case 6:
					return "KastaLagom";
				case 7:
					if(currentLanguage == lang.Svenska) {
			return "KastaLagom";
		} else {
			return "InBetween";
		}
		default:
			return "HighScore";
	}

}
	public string DisplayCurrentGameName {
		get {
			return CurrentGameName(gameMode);
			//switch(gameMode) {
			//	case 0:
			//		if(currentLanguage == lang.Svenska) {
			//			return "SnabbareSnabbare";
			//		} else {
			//			return "HigherHigher";
			//		}

			//	case 1:
			//		if(currentLanguage == lang.Svenska) {
			//			return "SnabbBoll";
			//		} else {
			//			return "FastBall";
			//		}

			//	case 2:
			//		return "InBetween";
			//	case 3:
			//		if(currentLanguage == lang.Svenska) {
			//			return "SnabbSnabbare";
			//		} else {
			//			return "HigherHigher";
			//		}
			//	case 4:
			//		return "LowerLower";
			//	case 5:
			//		return "KastaLagom";
			//	case 6:
			//		return "KastaLagom";
			//	case 7:
			//		if(currentLanguage == lang.Svenska) {
			//			return "KastaLagom";
			//		} else {
			//			return "InBetween";
			//		}
			//	default:
			//		return "HighScore";
			//}
		}
	}
	public string CurrentGame {
		get {
			switch(gameMode) {
				case 0:
					return "HigherHigher_";
				case 1:
					return "FastBall";
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
	public bool IsBallMissing {
		get {
			return ballMissing;
		}
	}
	public bool IsGameStarted {
		get {
			return gameStarted;
		}

	}
	private void ChangeTotalLives(bool addLife) {
		if(addLife) {
			totalLifes++;
			modifiedLives = true;
		} else {
			totalLifes--;
		}
		if(gameMode < 5) {
			totalLives[0].text = (totalLifes).ToString();
		} else {
			for(int i = 0; i < playerLivesList.Count; i++) {
				playerLivesList[i] = totalLifes;
				playersList[i][19].text = totalLifes.ToString();
			}
		}

	}
	private void UpdateTotalLives() {
		for(int i = 0; i < playerLivesList.Count; i++) {
			playerLivesList[i] = totalLifes;
			playersList[i][19].text = totalLifes.ToString();
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
		//deltaTime = Time.deltaTime;
		//timer += deltaTime;


		if(WinnerCanvas.activeSelf) {
			flashTimer += Time.deltaTime;// deltaTime;
			if((flashTimer) > 0.02) {
				winnerFontSize = winnerFontSize + winnerFontSizeIncrease;
				if(winnerFontSize >= winnerFontSizeMax || winnerFontSize <= winnerFontSizeMin) {
					winnerFontSizeIncrease = winnerFontSizeIncrease * -1;
				}
				WinnerCanvas.GetComponentInChildren<TextMeshProUGUI>().fontSize = winnerFontSize;
				flashTimer = 0;
			}
		}
	}
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
		if(!inBetween) {
			busyThinking = false;
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
		flashPlayer = null;
		//blinkSegmentsRutine = null;

	}
	private IEnumerator BlinkSegments(TextMeshProUGUI segment, float blinkInterval) {
		while(true) {
			//if(segment.enabled) {
			//	segment.enabled = false;
			//} else {
			//	segment.enabled = true;
			//}
			segment.enabled = !segment.enabled;
			yield return new WaitForSeconds(blinkInterval);
		}
	}
	private void blinkDisplay(bool enabled) {
		score1[0].enabled = enabled;
		score1[1].enabled = enabled;
		score1[2].enabled = enabled;
		if(gameMode != 0) {
			score1[3].enabled = enabled;
		}
	}
	private void ResetScores() {
		WinnerCanvas.SetActive(false);
		totalThrows = 0;
		RemoveAllListObjects();
		RemoveAllListObjectsPleyers();
		//StartCoroutine(LoadBgImage());
		if(gameMode < 5) {
			//MainCanvas.GetComponent<UnityEngine.UI.Image>().sprite = Sprite1;			
			score1 = GetChildText(BigScoreUIElementPrefab, new Vector3(-154, 125, 0));
			score2 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -122, 0));
			score3 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -337, 0));
			if(gameMode != 1) {
				score4 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -554, 0));
				score5 = GetChildText(SmallScoreUIElementPrefab, new Vector3(-66, -768, 0));
			}
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
			totalLifes = 1;

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
					if(gameMode == 3 || gameMode == 4 ) {//Set Lives
						totalLives[0].text = "2";
						totalLifes = 2;
					}
				}

				totalScores[4].text = "0";
				gameModeMultiplyer = 10;
			}

			totalScore = 0;
			oldScore = 0;
			oldScore2 = 0;
			totalThrows = 0;
			//gameOver = false;
		} else if(gameMode == 5 || gameMode == 6 || gameMode == 7) {
			//MainCanvas.GetComponent<UnityEngine.UI.Image>().sprite = Sprite2;
			GameName.GetComponentInParent<Transform>().localPosition = new Vector3(0, 888, 0);
		}
	}
	

	private void GameOver() {
		if(gameMode != 1) {
			changeColor2(new Color32(255, 0, 0, 220), score1);
		}
		if(totalLifes > 1) {
			totalLifes -= 1;
			totalLives[0].text = (totalLifes).ToString();//+1
			lostLife = true;
		} else {
			gameOverHelper();
			busyThinking = true;
			Invoke("GameIsOverAfterDelay", 1f);
			
		}
	}
	private void GameIsOverAfterDelay() {
		busyThinking = false;
		if(!modifiedLives) {
			if(GameIsOver != null) {
				GameIsOver();
			}
		}
		gameStarted = false;
	}
	private bool IsWithin(int a, int b, int c) {
		return (c >= Mathf.Min(a, b) && c <= Mathf.Max(a, b)); //returnIsWithin;
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

		} else if(gameMode == 1) {
			totalScore += point;
			if(point> oldScore) {
				oldScore = point;
			}			
			totalThrows += 1;
			if(totalThrows > 2) {
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
				if(gameMode != 1){
					MoveText(score4, score5);
					MoveText(score3, score4);
				}
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

		if(gameMode == 1 && gameOver) {
			totalThrows = oldScore;
		}





	}
}
