using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCounter : MonoBehaviour {
    //[SerializeField] PointHUD pointHUD;
	[SerializeField] serial serial;
	[SerializeField] GameHandler GameHandler;
	bool gameStopped = false;
	void Update() {
		if(gameStopped != serial.IsGameOver) {
			gameStopped = serial.IsGameOver;
			if(gameStopped == true) {
				GameHandler.StopGame();
			}
		}
		if(gameStopped == true) {

		}
	}
	public void StartGame () {
        //gameStopped = false;
        //pointHUD.ResetPoints ();
        //StartCoroutine (CountPoints ());
    }

    public void StopGame () {
        //gameStopped = true;
    }

   // private IEnumerator CountPoints () {
        //while (!gameStopped) {
         //   pointHUD.Points += 5;

         //   yield return new WaitForSeconds (1);
    //    }
   // }
}

