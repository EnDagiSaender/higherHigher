using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;


public class videoControllerScript : MonoBehaviour {
	[SerializeField] serial serial;
	private string gameName;
	private VideoPlayer vp;
	// Start is called before the first frame update
	void Start() {
		vp = gameObject.GetComponent<VideoPlayer>();

	}

	// Update is called once per frame
	void Update() {

	}
	private void OnEnable() {
		serial.GameIsOver += GameOver;
		serial.GameChanged += GameChanged;
		GameChanged();


	}

	private void OnDisable() {
		serial.GameIsOver -= GameOver;
		serial.GameChanged -= GameChanged;

	}
	public void GameChanged() {
		gameName = serial.DisplayCurrentGameName;
		string fileName = Application.streamingAssetsPath + "/video/HowToPlay/" + gameName + ".mp4";
		if(File.Exists(fileName)) {
			if(vp.url != fileName) {
				vp.url = fileName;
				vp.Play();
			}
		}else {
			print("VIDEO DO NOT Exists");
			print(fileName);
		}

	}
	public void GameOver() {

	}
}
