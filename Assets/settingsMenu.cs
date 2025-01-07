using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class settingsMenu : MonoBehaviour
{
	Color active = new Color32(0, 0, 50, 255);
	Color notActive = new Color32(10, 0, 120, 255);
	[SerializeField] serial serial;
	[SerializeField] EventManager EventManager;
	[SerializeField] GameObject SettingsPanel;
	[SerializeField] TextMeshProUGUI mynt;
	[SerializeField] TextMeshProUGUI myntFastboll;
	[SerializeField] TextMeshProUGUI myntFaster;
	[SerializeField] TextMeshProUGUI vinstFaster;
	[SerializeField] TextMeshProUGUI helper;
	[SerializeField] TextMeshProUGUI myntLagom;
	[SerializeField] TextMeshProUGUI vinstLagom;


	[SerializeField] GameObject openGate;
	[SerializeField] GameObject openGateValue;
	[SerializeField] GameObject freePlay;
	[SerializeField] GameObject freePlayValue;
	[SerializeField] GameObject prizeLagom;
	[SerializeField] GameObject prizeLagomValue;
	[SerializeField] GameObject prizeFaster;	
	[SerializeField] GameObject prizeFasterValue;
	[SerializeField] GameObject oneSensor;
	[SerializeField] GameObject oneSensorValue;
	[SerializeField] GameObject clown;
	[SerializeField] GameObject clownValue;
	[SerializeField] GameObject vinstFreeplay;
	[SerializeField] GameObject vinstFreeplayValue;
	[SerializeField] GameObject disableSensor;
	[SerializeField] GameObject disableSensorValue;
	[SerializeField] GameObject games;
	[SerializeField] GameObject gamesValue;
	[SerializeField] GameObject addCoin;
	[SerializeField] GameObject clearCoin;
	[SerializeField] GameObject clearStats;


	private int prizeLagomMin = 5;
	private int prizeLagomMax = 40;
	private int prizeFasterMin = 5;
	private int prizeFasterMax = 40;
	private int clownStatus = 0;
	private string[] allGames = new string[] { "SnabbBoll SnabbSnabbare KastaLagom", "SnabbSnabbare KastaLagom" , "SnabbSnabbare", "SnabbBoll" };
	private int oldGameModeChoices;
	int menuIndex = 1;
	int lastMenuIndex = 1;
	bool settingNotValue = true; 

	List<Image> settingsList = new List<Image>();
	List<Image> valueList = new List<Image>();
	List<TextMeshProUGUI> valueListText = new List<TextMeshProUGUI>();

	private void OnEnable() {
		//print("On Enable Keypress");
		//serial.OkButton += Ok;
		//serial.LeftButton += MoveLeft;
		//serial.RightButton += MoveRight;
		EventManager.NewGame += Ok;
		EventManager.ChangedDir += Move;		
		clownStatus = 0;
		if(valueListText.Count > 3) {
			valueListText[0].text = serial.ballOut ? "Open" : "Closed";
			valueListText[5].text = "Closed";
			valueListText[8].text = allGames[serial.savedGameMode];
		}
		mynt.text = serial.myntIn.ToString();
		myntFastboll.text = serial.myntInFastboll.ToString();
		myntFaster.text = serial.myntInFaster.ToString();
		vinstFaster.text = serial.vinsterUtFaster.ToString();
		helper.text = serial.helper.ToString();
		myntLagom.text = serial.myntInLagom.ToString();
		vinstLagom.text = serial.vinsterUtLagom.ToString();
		print(serial.vinstProcentLagom());
		oldGameModeChoices = serial.savedGameMode;
		//setHighScorePanel.SetActive(true);
		//print("HighScore panel visible!");


		//valueList[menuIndex]
		//print(settingsList.Count);
		//GameObject[] temparray = SettingsPanel.GetComponentsInChildren<GameObject>();

		//print(temparray.Length);
	}

	private void OnDisable() {
		//serial.OkButton -= Ok;
		//serial.LeftButton -= MoveLeft;
		//serial.RightButton -= MoveRight;
		EventManager.NewGame -= Ok;
		EventManager.ChangedDir -= Move;
		//setHighScorePanel.SetActive(false);
		//print("HighScore panel not visible!");
		serial.saveGameSettings();
		serial.UpdateCreditText();
		if(!serial.IsGameOver) {
			EventManager.openGate();
		} else {
			EventManager.closeGate();
		}
		if(oldGameModeChoices != serial.savedGameMode) {
			switch(serial.savedGameMode) {
				case 0:
					serial.gameModeChoices = new int[] { 1, 3, 7 };
					break;
				case 1:
					serial.gameModeChoices = new int[] { 3, 7 };
					break;
				case 2:
					serial.gameModeChoices = new int[] { 3 };
					break;
				case 3:
					serial.gameModeChoices = new int[] { 1 };
					break;
				default:
					break;
			}
		}
		serial.closeMouth();
		serial.ExitSettings();

	}
	private void Ok() {
		if(menuIndex == settingsList.Count -3) {//5
			serial.addServiceCoin();
			return;
		}
		if(menuIndex == settingsList.Count - 2) {//6
			serial.clearCoin();
			return;
		}
		if(menuIndex == settingsList.Count - 1) {//6
			serial.clearStats();
			return;
		}
		if(settingNotValue) {
			settingsList[menuIndex].color = notActive;
			valueList[menuIndex].color = active;
		} else {
			settingsList[menuIndex].color = active;
			valueList[menuIndex].color = notActive;
		}
		settingNotValue = !settingNotValue;
	}
	private void Move( bool down) {
		if(settingNotValue) {
			settingsList[menuIndex].color = notActive;
			if(down) {
				menuIndex++;
				if(menuIndex >= settingsList.Count) {
					menuIndex = 0;
				}
			} else {
				menuIndex--;
				if(menuIndex < 0) {
					menuIndex = settingsList.Count - 1;
				}
			}
			settingsList[menuIndex].color = active;
		} else {
			switch(menuIndex) {
				case 0: //OpenGate
					//serial.freePlay = !serial.freePlay;
					valueListText[menuIndex].text = valueListText[menuIndex].text == "Open" ? "Closed" : "Open";
					if(valueListText[menuIndex].text == "Open") {
						EventManager.openGate();
					} else {
						EventManager.closeGate();
					}
					//if(serial.freePlay) {
					//	valueListText[menuIndex].text = "ON";
					//} else {
					//	valueListText[menuIndex].text = "OFF";
					//}
					break;
				case 1: //FreePlay
					serial.freePlay = !serial.freePlay;
					valueListText[menuIndex].text = serial.freePlay ? "ON" : "OFF";
					//if(serial.freePlay) {
					//	valueListText[menuIndex].text = "ON";
					//} else {
					//	valueListText[menuIndex].text = "OFF";
					//}
					break;
				case 2: // prizeLagom
					serial.prizeLagom = down ? serial.prizeLagom + 1 : serial.prizeLagom - 1;
					if(serial.prizeLagom > prizeLagomMax) {
						serial.prizeLagom = prizeLagomMin;
					} else if(serial.prizeLagom < prizeLagomMin) {
						serial.prizeLagom = prizeLagomMax;
					}
					valueListText[menuIndex].text = serial.prizeLagom.ToString();
					break;
				case 3: // prizeFaster
					serial.prizeFaster = down ? serial.prizeFaster + 1 : serial.prizeFaster -1;
					if(serial.prizeFaster > prizeFasterMax) {
						serial.prizeFaster = prizeFasterMin;
					} else if(serial.prizeFaster < prizeFasterMin) {
						serial.prizeFaster = prizeFasterMax;
					}
					valueListText[menuIndex].text = serial.prizeFaster.ToString();
					break;
				case 4: // prizeFaster
					serial.oneSensor = !serial.oneSensor;
					valueListText[menuIndex].text = serial.oneSensor ? "ON" : "OFF";
					break;
				case 5: // clown
					clownStatus = down ? clownStatus + 1 : clownStatus - 1;
					if(clownStatus > 2) {
						clownStatus = 0;
					} else if(clownStatus < 0) {
						clownStatus = 3;
					}
					if(clownStatus == 0) {
						valueListText[menuIndex].text = "Closed";
						serial.closeMouth();
					} else if(clownStatus == 1) {
						valueListText[menuIndex].text = "Open";
						serial.openMouth();
					} else {
						valueListText[menuIndex].text = "Cycle";
						serial.repeatBubble = true;
						serial.cycleBubbles1();
					}
					break;
				case 6: // vinstFreeplay
					serial.freePlayVinst = !serial.freePlayVinst;
					valueListText[menuIndex].text = serial.freePlayVinst ? "ON" : "OFF";
					break;
				case 7: // disableSensor
					serial.disableSensor = !serial.disableSensor;
					valueListText[menuIndex].text = serial.disableSensor ? "OFF" : "ON";
					break;
				case 8: // disableSensor
					serial.savedGameMode = down ? serial.savedGameMode + 1 : serial.savedGameMode - 1;
					if(serial.savedGameMode +1 > allGames.Length) {
						serial.savedGameMode = 0;
					} else if(serial.savedGameMode < 0) {
						serial.savedGameMode = allGames.Length -1;
					}
					valueListText[menuIndex].text = allGames[serial.savedGameMode];
					/*
					if(serial.CurrentGame == "HigherHigher") {
						serial.savedGameMode = down ? serial.savedGameMode + 1 : serial.savedGameMode - 1;
						if(serial.savedGameMode + 1 > allGames.Length) {
							serial.savedGameMode = 0;
						} else if(serial.savedGameMode < 0) {
							serial.savedGameMode = allGames.Length - 1;
						}
						valueListText[menuIndex].text = allGames[serial.savedGameMode];
					} else {
						valueListText[menuIndex].text = "Byt spel till SnabbSnabbare för att ändra";
					}*/
					break;
				default:
					break;


			}



		}
	}
	// Start is called before the first frame update
	void Start()
    {
		settingsList.Add(openGate.GetComponent<Image>());
		settingsList.Add(freePlay.GetComponent<Image>());
		settingsList.Add(prizeLagom.GetComponent<Image>());
		settingsList.Add(prizeFaster.GetComponent<Image>());
		settingsList.Add(oneSensor.GetComponent<Image>());
		settingsList.Add(clown.GetComponent<Image>());
		settingsList.Add(vinstFreeplay.GetComponent<Image>());
		settingsList.Add(disableSensor.GetComponent<Image>());
		settingsList.Add(games.GetComponent<Image>());
		settingsList.Add(addCoin.GetComponent<Image>());
		settingsList.Add(clearCoin.GetComponent<Image>());
		settingsList.Add(clearStats.GetComponent<Image>());

		/*
			[SerializeField] GameObject clown;
	[SerializeField] GameObject clownValue;
	[SerializeField] GameObject vinstFreeplay;
	[SerializeField] GameObject vinstFreeplayValue;
	[SerializeField] GameObject disableSensor;
	[SerializeField] GameObject disableSensorValue;
	*/
		valueList.Add(openGateValue.GetComponent<Image>());
		valueList.Add(freePlayValue.GetComponent<Image>());
		valueList.Add(prizeLagomValue.GetComponent<Image>());
		valueList.Add(prizeFasterValue.GetComponent<Image>());
		valueList.Add(oneSensorValue.GetComponent<Image>());
		valueList.Add(clownValue.GetComponent<Image>());
		valueList.Add(vinstFreeplayValue.GetComponent<Image>());
		valueList.Add(disableSensorValue.GetComponent<Image>());
		valueList.Add(gamesValue.GetComponent<Image>());
		//TextMeshProUGUI[] temparray = freePlayValue.GetComponentInChildren<TextMeshProUGUI>();
		valueListText.Add(openGateValue.GetComponentInChildren<TextMeshProUGUI>());
		valueListText.Add(freePlayValue.GetComponentInChildren<TextMeshProUGUI>());
		valueListText.Add(prizeLagomValue.GetComponentInChildren<TextMeshProUGUI>());
		valueListText.Add(prizeFasterValue.GetComponentInChildren<TextMeshProUGUI>());
		valueListText.Add(oneSensorValue.GetComponentInChildren<TextMeshProUGUI>());
		valueListText.Add(clownValue.GetComponentInChildren<TextMeshProUGUI>());
		valueListText.Add(vinstFreeplayValue.GetComponentInChildren<TextMeshProUGUI>());
		valueListText.Add(disableSensorValue.GetComponentInChildren<TextMeshProUGUI>());
		valueListText.Add(gamesValue.GetComponentInChildren<TextMeshProUGUI>());
		//openGate.GetComponent<Image>().color = active;
		freePlay.GetComponent<Image>().color = active;
		valueListText[0].text = serial.ballOut ? "Open" : "Closed";
		valueListText[1].text = serial.freePlay ? "ON" : "OFF";
		valueListText[2].text = serial.prizeLagom.ToString();
		valueListText[3].text = serial.prizeFaster.ToString();
		valueListText[4].text = serial.oneSensor ? "ON" : "OFF";
		valueListText[5].text = "Closed";
		valueListText[6].text = serial.freePlayVinst ? "ON" : "OFF";
		valueListText[7].text = serial.disableSensor ? "OFF" : "ON";
		//valueListText[8].text = "";
		valueListText[8].text = allGames[serial.savedGameMode];


	}

    // Update is called once per frame

}
