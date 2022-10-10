using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class EventManager : MonoBehaviour
{
	private float timer = 0;
	private float deltaTime;

	public delegate void ChangeDirection(bool right);
	public static event ChangeDirection ChangedDir;

	public delegate void StartGame();
	public static event StartGame NewGame;

	public delegate void SerialScore(int score);
	public static event SerialScore NewScore;

	SerialPort portNo = new SerialPort("\\\\.\\COM3", 115200);
	// Start is called before the first frame update
	void Start()
    {
		Open();
	}

	void Open() {

		string[] ports = { };
		while(ports.Length < 1) {
			ports = SerialPort.GetPortNames();
			if(ports.Length < 1) {
				//print("no port, try again");
				new WaitForSeconds(1);
			} else {
				//print(ports[0]);
			}
		}
		foreach(string port in ports) {
			print(port);
		}

		portNo = new SerialPort("\\\\.\\" + ports[0], 115200);
		try {
			portNo.Open();
			portNo.ReadTimeout = 500;
		} catch {
			print("no port to open");
		}

	}

	// Update is called once per frame
	void Update()
    {
		deltaTime = Time.deltaTime;
		timer += deltaTime;
		if(timer > 0.1) {
			timer = 0;
			if(portNo.IsOpen) {
				try {
					///readByte(portNo.ReadByte());
					string msg = portNo.ReadLine();
					string[] message = msg.Split(',');
					if(Equals(message[0], "s")) {
						//print(message[1]);
						int speed = int.Parse(message[1]);
						if(NewScore != null) {
							NewScore(speed);
						}
					} else if(Equals(message[0], "n")) {
						print("New");
						if(NewGame != null) {
							NewGame();
						}
					} else if(Equals(message[0], "c")) {
						bool rightChange = true;
						print("right");
						if(ChangedDir != null) {
							ChangedDir(rightChange);							
						}
					} else if(Equals(message[0], "d")) {
						bool rightChange = false;
						print("left");
						if(ChangedDir != null) {
							ChangedDir(rightChange);
						}
					}
				} catch {

				}
			}
		}
	}
}
