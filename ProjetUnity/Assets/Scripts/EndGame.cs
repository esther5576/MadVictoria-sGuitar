using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{

	public GameObject _panelUI;
	public ScoreManager _playerScore;
	MicroInput _playerMicInput;

	// Use this for initialization
	void Awake ()
	{
		_panelUI = GameObject.Find ("PanelEndGame");
		_panelUI.SetActive (false);
		_playerScore = GameObject.Find ("Controller").GetComponent<ScoreManager> ();
		_playerMicInput = GameObject.Find ("Controller").GetComponent<MicroInput> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player") {
			_panelUI.SetActive (true);
			_panelUI.GetComponentInChildren<Text> ().text = "Score: " + (int)_playerScore._scoreF + "\nTime: " + _playerScore._minutes + ":" + _playerScore._seconds;
			SavWav.Save ("GuitarRecording", _playerMicInput._myAudioClip);
		}
	}
}
