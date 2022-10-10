using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreHandler : MonoBehaviour {
    List<HighscoreElement> highscoreList = new List<HighscoreElement> ();
	[SerializeField] serial serial;
	[SerializeField] int maxCount = 5;
    [SerializeField] string filename;
	string filename2 = "test";

	public delegate void OnHighscoreListChanged (List<HighscoreElement> list);
    public static event OnHighscoreListChanged onHighscoreListChanged;

    private void Start () {
        LoadHighscores ();
		serial.GameChanged  += GameChanged;
		
	}
	public void GameChanged() {
		//print(serial.CurrentGame);
		LoadHighscores();
	}

	private void LoadHighscores () {
		filename2 = serial.CurrentGame + ".json";
		print(filename2);
        highscoreList = FileHandler.ReadListFromJSON<HighscoreElement> (filename2);

        while (highscoreList.Count > maxCount) {
            highscoreList.RemoveAt (maxCount);
        }

        if (onHighscoreListChanged != null) {
            onHighscoreListChanged.Invoke (highscoreList);
        }
    }

    private void SaveHighscore () {
		filename2 = serial.CurrentGame + ".json";
		FileHandler.SaveToJSON<HighscoreElement> (highscoreList, filename2);
    }
	public bool IfHighscore2(int points, int throws) {
		if(highscoreList.Count < maxCount) {
			return true;
		} else if(throws > highscoreList[highscoreList.Count - 1].throws) {

			return true;
		} else if(throws == highscoreList[highscoreList.Count - 1].throws) {
			if(points > highscoreList[highscoreList.Count - 1].points) {
				return true;
			} else {
				return false;
			}
		} else {
			print("no highScore");
			return false;
		}
	}
	public bool IfHighscore(int point) {
		bool hs = false;
		for(int i = 0; i < maxCount; i++) {
			if(i >= highscoreList.Count || point > highscoreList[i].points) {
				hs = true;
			}
		}
		return hs;
	}
	public void AddHighscoreIfPossible(HighscoreElement element) {
		for(int i = 0; i < maxCount; i++) {
			if(i >= highscoreList.Count || element.throws  > highscoreList[i].throws) {
										  // add new high score
				highscoreList.Insert(i, element);
				//print(i);

				while(highscoreList.Count > maxCount) {
					highscoreList.RemoveAt(maxCount);
				}

				SaveHighscore();

				if(onHighscoreListChanged != null) {
					onHighscoreListChanged.Invoke(highscoreList);
				}

				break;
			} else if(element.throws == highscoreList[i].throws) {
				if(element.points > highscoreList[i].points) {
					highscoreList.Insert(i, element);
					//print(i);
					while(highscoreList.Count > maxCount) {
						highscoreList.RemoveAt(maxCount);
					}

					SaveHighscore();

					if(onHighscoreListChanged != null) {
						onHighscoreListChanged.Invoke(highscoreList);
					}

					break;
				}

			}
		}
	}
	//public void AddHighscoreIfPossible (HighscoreElement element) {
	//       for (int i = 0; i < maxCount; i++) {
	//           if (i >= highscoreList.Count || element.points > highscoreList[i].points) {
	//               // add new high score
	//               highscoreList.Insert (i, element);
	//			print(i);

	//               while (highscoreList.Count > maxCount) {
	//                   highscoreList.RemoveAt (maxCount);
	//               }

	//               SaveHighscore ();

	//               if (onHighscoreListChanged != null) {
	//                   onHighscoreListChanged.Invoke (highscoreList);
	//               }

	//               break;
	//           }
	//       }
	//   }

}