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
		//vp.loopPointReached += CheckOver;
		//GameChanged();
		PlayAttract();
		
	}

	// Update is called once per frame
	void Update() {

	}

	private void OnEnable() {
		serial.GameIsOver += GameOver;
		serial.GameChanged += GameChanged;
//		 GameChanged();


	}

	private void OnDisable() {
		serial.GameIsOver -= GameOver;
		serial.GameChanged -= GameChanged;

	}
	void CheckOver(UnityEngine.Video.VideoPlayer vp) {
		print("Video Is Over");
	}
	public void PlayAttract() {
		string fileName = Application.streamingAssetsPath + "/video/attract.mp4";
		if(File.Exists(fileName)) {
			if(vp.url != fileName) {
				vp.url = fileName;
				vp.Play();
			}
		} else {
			print("VIDEO DO NOT Exists");
			print(fileName);
		}

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
