using System;

[Serializable]
public class HighscoreElement {
    public string playerName;
    public int points;
	public int throws;


	public HighscoreElement (string name, int points, int throws) {
        playerName = name;
        this.points = points;
		this.throws = throws;

	}

}