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

	[SerializeField] GameObject freePlay;
	[SerializeField] GameObject freePlayValue;
	[SerializeField] GameObject prizeLagom;
	[SerializeField] GameObject prizeLagomValue;
	[SerializeField] GameObject prizeFaster;	
	[SerializeField] GameObject prizeFasterValue;
	[SerializeField] GameObject addCoin;
	[SerializeField] GameObject clearCoin;

	private int prizeLagomMin = 10;
	private int prizeLagomMax = 40;
	private int prizeFasterMin = 10;
	private int prizeFasterMax = 40;
	int menuIndex = 0;
	int lastMenuIndex = 0;
	bool settingNotValue = true; 

	List<Image> settingsList = new List<Image>();
	List<Image> valueList = new List<Image>();
	List<TextMeshProUGUI> valueListText = new List<TextMeshProUGUI>();

	private void OnEnable() {
		print("On Enable Keypress");
		//serial.OkButton += Ok;
		//serial.LeftButton += MoveLeft;
		//serial.RightButton += MoveRight;
		EventManager.NewGame += Ok;
		EventManager.ChangedDir += Move;
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
	}
	private void Ok() {
		if(menuIndex == 3) {
			serial.addCoin();
			return;
		}
		if(menuIndex == 4) {
			serial.clearCoin();
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
				case 0: //FreePlay
					serial.freePlay = !serial.freePlay;
					valueListText[menuIndex].text = serial.freePlay ? "ON" : "OFF";
					//if(serial.freePlay) {
					//	valueListText[menuIndex].text = "ON";
					//} else {
					//	valueListText[menuIndex].text = "OFF";
					//}
					break;
				case 1: // prizeLagom
					serial.prizeLagom = down ? serial.prizeLagom + 1 : serial.prizeLagom - 1;
					if(serial.prizeLagom > prizeLagomMax) {
						serial.prizeLagom = prizeLagomMin;
					} else if(serial.prizeLagom < prizeLagomMin) {
						serial.prizeLagom = prizeLagomMax;
					}
					valueListText[menuIndex].text = serial.prizeLagom.ToString();
					break;
				case 2: // prizeFaster
					serial.prizeFaster = down ? serial.prizeFaster + 1 : serial.prizeFaster -1;
					if(serial.prizeFaster > prizeFasterMax) {
						serial.prizeFaster = prizeFasterMin;
					} else if(serial.prizeFaster < prizeFasterMin) {
						serial.prizeFaster = prizeFasterMax;
					}
					valueListText[menuIndex].text = serial.prizeFaster.ToString();
					break;
				default:
					break;


			}



		}
	}
	// Start is called before the first frame update
	void Start()
    {
		settingsList.Add(freePlay.GetComponent<Image>());
		settingsList.Add(prizeLagom.GetComponent<Image>());
		settingsList.Add(prizeFaster.GetComponent<Image>());
		settingsList.Add(addCoin.GetComponent<Image>());
		settingsList.Add(clearCoin.GetComponent<Image>());

		valueList.Add(freePlayValue.GetComponent<Image>());
		valueList.Add(prizeLagomValue.GetComponent<Image>());
		valueList.Add(prizeFasterValue.GetComponent<Image>());
		//TextMeshProUGUI[] temparray = freePlayValue.GetComponentInChildren<TextMeshProUGUI>();
		valueListText.Add(freePlayValue.GetComponentInChildren<TextMeshProUGUI>());
		valueListText.Add(prizeLagomValue.GetComponentInChildren<TextMeshProUGUI>());
		valueListText.Add(prizeFasterValue.GetComponentInChildren<TextMeshProUGUI>());
		freePlay.GetComponent<Image>().color = active;
		valueListText[0].text = serial.freePlay ? "ON" : "OFF";
		valueListText[1].text = serial.prizeLagom.ToString();
		valueListText[2].text = serial.prizeFaster.ToString();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
