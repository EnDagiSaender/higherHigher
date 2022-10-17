using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class EventManager : MonoBehaviour
{
	private float timer = 0;
	private float deltaTime;
	private bool anyPortOpen = false;

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
		//while(ports.Length < 1) {
		//	ports = SerialPort.GetPortNames();
		//	if(ports.Length < 1) {
		//		//print("no port, try again");
		//		new WaitForSeconds(1);
		//	} else {
		//		//print(ports[0]);
		//	}
		//}
		if(ports.Length > 0) {
			foreach(string port in ports) {
				print(port);
			}

			portNo = new SerialPort("\\\\.\\" + ports[0], 115200);
			try {
				portNo.Open();
				portNo.ReadTimeout = 500;
				anyPortOpen = true;
			} catch {
				print("no port to open");
			}
		} else {
			Invoke("Open",10f);
			//print("try open a port in 10 sec again");

		}

	}

	// Update is called once per frame
	void Update()
    {

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if (ChangedDir != null)
			{
				ChangedDir(false);
			}
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (ChangedDir != null)
			{
				ChangedDir(true);
			}
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (NewGame != null)
			{
				NewGame();
			}
		}
		if (Input.GetKeyDown(KeyCode.Return))
		{
			if (NewGame != null)
			{
				NewGame();
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			if(NewScore != null) {
				NewScore(100);
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			if (NewScore != null)
			{
				NewScore(200);
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			if (NewScore != null)
			{
				NewScore(300);
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			if (NewScore != null)
			{
				NewScore(400);
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			if (NewScore != null)
			{
				NewScore(500);
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			if (NewScore != null)
			{
				NewScore(600);
			}
		}
		deltaTime = Time.deltaTime;
		timer += deltaTime;
		if(timer > 0.1) {
			timer = 0;
			if(anyPortOpen && portNo.IsOpen) {
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
						//print("New");
						if(NewGame != null) {
							NewGame();
						}
					} else if(Equals(message[0], "c")) {
						bool rightChange = true;
						//print("right");
						if(ChangedDir != null) {
							ChangedDir(rightChange);							
						}
					} else if(Equals(message[0], "d")) {
						bool rightChange = false;
						//print("left");
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
