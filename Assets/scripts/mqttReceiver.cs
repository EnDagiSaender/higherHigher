using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using M2MqttUnity;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class mqttReceiver : M2MqttUnityClient {
	[SerializeField] GameObject NoConnectionDot;
	[Header("MQTT topics")]
	[Tooltip("Set the topic to subscribe. !!!ATTENTION!!! multi-level wildcard # subscribes to all topics")]
	public string topicSubscribe = "faster/#"; // topic to subscribe. !!! The multi-level wildcard # is used to subscribe to all the topics. Attention i if #, subscribe to all topics. Attention if MQTT is on data plan
	[Tooltip("Set the topic to publish (optional)")]
	public string topicPublish = ""; // topic to publish
	public string messagePublish = ""; // message to publish

	[Tooltip("Set this to true to perform a testing cycle automatically on startup")]
	public bool autoTest = false;
	Ping ping;
	//List<int> pingList;
	//using C# Property GET/SET and event listener to reduce Update overhead in the controlled objects
	private string m_msg;

	public string msg {
		get {
			return m_msg;
		}
		set {
			if(m_msg == value) return;
			m_msg = value;
			if(OnMessageArrived != null) {
				OnMessageArrived(m_msg);
			}
		}
	}

	public event OnMessageArrivedDelegate OnMessageArrived;
	public delegate void OnMessageArrivedDelegate(string newMsg);

	//using C# Property GET/SET and event listener to expose the connection status
	private bool m_isConnected;

	public bool isConnected {
		get {
			return m_isConnected;
		}
		set {
			if(m_isConnected == value) return;
			m_isConnected = value;
			if(OnConnectionSucceeded != null) {
				OnConnectionSucceeded(isConnected);
			}
		}
	}
	public event OnConnectionSucceededDelegate OnConnectionSucceeded;
	public delegate void OnConnectionSucceededDelegate(bool isConnected);

	// a list to store the messages
	private List<string> eventMessages = new List<string>();

	public void Publish() {
		client.Publish(topicPublish, System.Text.Encoding.UTF8.GetBytes(messagePublish), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
		Debug.Log("Test message published");
	}
	public void Publish2(string msg, string topic) {
		client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
		Debug.Log("Published: " + msg + " To topic: " + topic);
	}

	public void SetEncrypted(bool isEncrypted) {
		this.isEncrypted = isEncrypted;
	}

	protected override void OnConnecting() {
		base.OnConnecting();
	}

	protected override void OnConnected() {
		base.OnConnected();
		isConnected = true;

		if(autoTest) {
			Publish();
		}
	}

	protected override void OnConnectionFailed(string errorMessage) {
		Debug.Log("CONNECTION FAILED! " + errorMessage);
	}

	protected override void OnDisconnected() {
		Debug.Log("Disconnected.");
		isConnected = false;
		ping = new Ping("10.99.10.123");
		Invoke("PingStatus", 10f);
		NoConnectionDot.SetActive(true);
	}

	protected override void OnConnectionLost() {
		Debug.Log("CONNECTION LOST!");
		isConnected = false;
		ping = new Ping("10.99.10.123");
		Invoke("PingStatus", 10f);
		NoConnectionDot.SetActive(true);
	}

	protected override void SubscribeTopics() {
		client.Subscribe(new string[] { topicSubscribe }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
	}

	protected override void UnsubscribeTopics() {
		client.Unsubscribe(new string[] { topicSubscribe });
	}

	protected override void Start() {
		//pingList = new List<int>();
		ping = new Ping("10.99.10.123");
		Invoke("PingStatus", 1f);
		//base.Start();
	}
	private void PingStatus() {
		if(ping.isDone) {
			//print("ping is done");
			base.Start();
			NoConnectionDot.SetActive(false);
		} else {
			//print("No ping");
			ping = new Ping("10.99.10.123");
			Invoke("PingStatus", 10f);
		}

	}
	protected override void DecodeMessage(string topic, byte[] message) {
		//The message is decoded
		msg = System.Text.Encoding.UTF8.GetString(message);

		Debug.Log("Received: " + msg);
		Debug.Log("from topic: " + m_msg);

		StoreMessage(msg);
		if(topic == topicSubscribe) {
			if(autoTest) {
				autoTest = false;
				Disconnect();
			}
		}
	}

	private void StoreMessage(string eventMsg) {
		if(eventMessages.Count > 50) {
			eventMessages.Clear();
		}
		eventMessages.Add(eventMsg);
	}

	protected override void Update() {
		base.Update(); // call ProcessMqttEvents()

	}

	private void OnDestroy() {
		Disconnect();
	}

	private void OnValidate() {
		if(autoTest) {
			autoConnect = true;
		}
	}
}