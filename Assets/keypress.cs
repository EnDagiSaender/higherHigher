using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class keypress : MonoBehaviour
{
	List<char> letters = new List<char> {'Y', 'Z', '_', '<', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X' };
	//string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N" };
	// Start is called before the first frame update
	[SerializeField]
	private TextMeshProUGUI firstString;
	[SerializeField]
	private TextMeshProUGUI currentString;
	[SerializeField]
	private TextMeshProUGUI LastString;
	[SerializeField]
	private TextMeshProUGUI firstLetter;
	[SerializeField]
	private TextMeshProUGUI secondLetter;
	[SerializeField]
	private TextMeshProUGUI lastLetter;
	[SerializeField]
	private TextMeshProUGUI[] highScoreLetters;


	[SerializeField] GameObject highScorePanel;
	[SerializeField] GameObject setHighScorePanel;
	[SerializeField] HighscoreHandler highscoreHandler;
	[SerializeField] serial serial;
	[SerializeField] EventManager EventManager;

	


	private int letterCounter = 0;
	//char[] highScoreChar = new char[] { ' ', ' ', ' ' };
	string highScoreString = "";


	private void OnEnable() {
		print("On Enable Keypress"); 
		//serial.OkButton += Ok;
		//serial.LeftButton += MoveLeft;
		//serial.RightButton += MoveRight;
		EventManager.NewGame += Ok;
		EventManager.ChangedDir += Move;
		//setHighScorePanel.SetActive(true);
		//print("HighScore panel visible!");
	}

	private void OnDisable() {
		//serial.OkButton -= Ok;
		//serial.LeftButton -= MoveLeft;
		//serial.RightButton -= MoveRight;
		EventManager.NewGame -= Ok;
		EventManager.ChangedDir  -= Move;
		//setHighScorePanel.SetActive(false);
		//print("HighScore panel not visible!");
	}
	void Start()
    {
		//List<char> letters = new List<char>{ 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L' };
		UpdateLetters();
	}

	// Update is called once per frame
	void UpdateLetters() {
		string tempString = letters[0].ToString() + letters[1].ToString() + letters[2].ToString() + letters[3].ToString();
		firstString.text = tempString;
		tempString = letters[5].ToString() + letters[6].ToString() + letters[7].ToString() + letters[8].ToString();
		LastString.text = tempString;
		currentString.text = letters[4].ToString();
	}
	void SendHighScore() {
		//foreach (TextMeshProUGUI text in highScoreLetters){
		//	if (text.text == "_"){
		//		text.text = " ";
		//	}
		//}
		highScoreString = highScoreLetters[0].text + highScoreLetters[1].text + highScoreLetters[2].text;
		highscoreHandler.AddHighscoreIfPossible(new HighscoreElement(highScoreString, serial.Points, serial.Throws));
		highScoreLetters[0].text = "_";
		highScoreLetters[1].text = "_";
		highScoreLetters[2].text = "_";
		letters = new List<char> { 'Y', 'Z', '_', '<', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X' };
		UpdateLetters();
		letterCounter = 0;
		highScorePanel.SetActive(true);
		setHighScorePanel.SetActive(false);
		//this.enabled = false;
		//print(highScoreString);
		//print("done");
	}
	void Move(bool right) {
		if(right == true) {
			MoveRight();
		} else {
			MoveLeft();
		}
	}
	void MoveLeft() {
		//print("left");
		//print(letters.Count);
		char temp = letters[letters.Count - 1];
		letters.RemoveAt(letters.Count - 1);
		letters.Insert(0, temp);
		string test = "";
		foreach(char c in letters) {
			test += c;
		}
		//print(test);
		UpdateLetters();
	}
	void MoveRight() {
		//print("right");
		//print(letters.Count);
		char temp = letters[0];
		letters.RemoveAt(0);
		letters.Insert(letters.Count, temp);
		string test = "";
		foreach(char c in letters) {
			test += c;
		}
		//print(test);
		UpdateLetters();
	}
	void Ok() {
		//print(letters[4].ToString());
		if(letters[4] == '<') {
			if(letterCounter > 0) {
				letterCounter -= 1;
				highScoreLetters[letterCounter].text = "_";
			}
		} else {
			highScoreLetters[letterCounter].text = letters[4].ToString();
			if(highScoreLetters[letterCounter].text == "_") {
				highScoreLetters[letterCounter].text = " ";
			}
			//highScoreChar[letterCounter] = letters[4];
			letterCounter += 1;
			if(letterCounter > 2) {
				SendHighScore();
				//letterCounter = 0;
			}
		}
	}

}
