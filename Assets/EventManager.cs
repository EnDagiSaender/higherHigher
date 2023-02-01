using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class EventManager : MonoBehaviour
{
	private float timer = 0;
	private float timer2 = 0;
	private float deltaTime;
	private bool anyPortOpen = false;
	private bool ball = false;

	public delegate void ChangeDirection(bool right);
	public static event ChangeDirection ChangedDir;

	public delegate void ChangeTotalLives(bool addMoreLives);
	public static event ChangeTotalLives ChangeLives;

	public delegate void StartGame();
	public static event StartGame NewGame;

	public delegate void SerialScore(int score);
	public static event SerialScore NewScore;

	public delegate void ChangeLanguage();
	public static event ChangeLanguage NewLanguage;

	public delegate void ChangeCoin();
	public static event ChangeCoin AddCoin;

	public delegate void Bollstatus();
	public static event Bollstatus BallFound;

	public delegate void ChangeFreeplay();
	public static event ChangeFreeplay ToggleFreeplay;

	public delegate void TestMenu();
	public static event TestMenu ToggleTestMenu;

	[SerializeField] serial serial;

	SerialPort portNo = new SerialPort("\\\\.\\COM3", 115200);

	private void OnEnable() {
		serial.CloseBallGate += closeGate;
		serial.OpenBallGate += openGate;
		serial.SetCoinLock += setCoinLock;
	}

	private void OnDisable() {
		serial.SetCoinLock -= setCoinLock;
		serial.CloseBallGate -= closeGate;
		serial.OpenBallGate -= openGate;
	}
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
		ports = SerialPort.GetPortNames();
		if(ports.Length > 0) {
			foreach(string port in ports) {
				print(port);
			}

			portNo = new SerialPort("\\\\.\\" + ports[0], 115200);
			try {
				portNo.Open();
				portNo.ReadTimeout = 100;
				anyPortOpen = true;
			} catch {
				print("no port to open");
			}
		} else {
			Invoke("Open",10f);
			print("try open a port in 10 sec again");

		}

	}
	void closeGate() {
		if(anyPortOpen && portNo.IsOpen) {
			try {
				portNo.WriteLine("l");
				print("Close");
			}catch{
				print("fail to send command to esp32");
			}
		}
	}
	void openGate() {
		if(anyPortOpen && portNo.IsOpen) {
			try {
				portNo.WriteLine("o");
				print("Open");
			} catch {
				print("fail to send command to esp32");
			}
		}
	}
	void setCoinLock(bool on) {
		if(anyPortOpen && portNo.IsOpen) {
			try {
				if(on) {
					portNo.WriteLine("c");
					print("coin lock on");
				} else {
					portNo.WriteLine("r");
					print("coin lock off");
				}
			} catch {
				print("fail to send command to esp32");
			}
		}
	}

	public bool IsBall {
		get {
			return ball;
		}
	}
	// Update is called once per frame
	void Update()
    {
		if(Input.GetKeyDown(KeyCode.Q)) {
			Application.Quit();
		}
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
		if(Input.GetKeyDown(KeyCode.UpArrow) )
		{
			if(!(serial.IsGameStarted)) {
				if(ChangeLives != null) {
					ChangeLives(true);
				}
			}
		}
		if(Input.GetKeyDown(KeyCode.DownArrow)) {
			if(!(serial.IsGameStarted)) {
				if(ChangeLives != null) {
					ChangeLives(false);
				}
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
		if(Input.GetKeyDown(KeyCode.A)) {
			if(NewLanguage != null) {
				NewLanguage();
			}
		}
		if(Input.GetKeyDown(KeyCode.S)) {
			if(AddCoin != null) {
				AddCoin();
			}
		}
		if(Input.GetKeyDown(KeyCode.F)) {
			//if(ToggleFreeplay != null) {
			//	ToggleFreeplay();
			//}
			if(ToggleTestMenu != null) {
				ToggleTestMenu();
			}
		}
		if(Input.GetKeyDown(KeyCode.T)) {
			if(ToggleTestMenu != null) {
				ToggleTestMenu();
			}
		}
		if(Input.GetKeyDown(KeyCode.O)) {
			if(anyPortOpen && portNo.IsOpen) {
				try {
					portNo.Write("o");
				} catch {
				}
			}
		}
		if(Input.GetKeyDown(KeyCode.L)) {
			if(anyPortOpen && portNo.IsOpen) {
				try {
					portNo.Write("l");
				} catch {
				}
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
		timer2 += deltaTime;
		if(timer > 0.1) {
			timer = 0;
			if(anyPortOpen && portNo.IsOpen) {
				if(portNo.BytesToRead != 0) {
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
						} else if(Equals(message[0], "l")) {
							if(NewLanguage != null) {
								NewLanguage();
							}
						} else if(Equals(message[0], "p")) {
							if(AddCoin != null) {
								AddCoin();
							}
						} else if(Equals(message[0], "f")) {
							//if(ToggleFreeplay != null) {
							//	ToggleFreeplay();
							//}
							if(ToggleTestMenu != null) {
								ToggleTestMenu();
							}
						} else if(Equals(message[0], "t")) {
							if(ToggleTestMenu != null) {
								ToggleTestMenu();
							}
						} else if(Equals(message[0], "b")) {
							if(!ball) {
								ball = true;
							}
							if(serial.IsBallMissing) {
								if(BallFound != null) {
									BallFound();
								}
							}
							print("ball");
						} else if(Equals(message[0], "v")) {
							if(ball) {
								ball = false;
							}
							print("not ball");
						}
					} catch {

					}
				}
			}
		}
		if(timer2 > 5) {
			timer2 = 0;
			if(serial.IsGameOver) {
				if(anyPortOpen && portNo.IsOpen) {
					try {
						portNo.WriteLine("g");
						print("ask boll status");
					} catch {
						print("fail to send command to esp32");
					}
				}
			}
		}
	}
}
