using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {
    [SerializeField] PointCounter pointCounter;
    [SerializeField] HighscoreHandler highscoreHandler;
	[SerializeField] serial serial;
	//[SerializeField] PointHUD pointHUD;
    [SerializeField] string playerName;

    public void StartGame () {
        pointCounter.StartGame ();
    }
    public void StopGame () {
        //highscoreHandler.AddHighscoreIfPossible (new HighscoreElement (playerName, serial.Points));
        pointCounter.StopGame ();
    }
}