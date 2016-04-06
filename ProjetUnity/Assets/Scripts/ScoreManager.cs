using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

	public float _timerF;
	public float _scoreF;
	float _palierMeterF = 10f;
	float _multiplierXF = 1f;

	Text _multiplierUI;
	Text _scoreUI;
	Text _timerUI;

	Animator _multiplierAn;

	public string _minutes;
	public string _seconds;

	// Use this for initialization
	void Start ()
	{
		_multiplierUI = GameObject.Find ("MultiplicateurInGameUI").GetComponent<Text> ();
		_scoreUI = GameObject.Find ("ScoreInGameUI").GetComponent<Text> ();
		_timerUI = GameObject.Find ("TimeInGameUI").GetComponent<Text> ();
		_multiplierAn = GameObject.Find ("MultiplicateurInGameUI").GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		_timerF += Time.deltaTime;
		_scoreF += (Time.deltaTime * (int)_multiplierXF * 0.5f);

		_minutes = Mathf.Floor (_timerF / 60).ToString ("00");
		_seconds = Mathf.Floor (_timerF % 60).ToString ("00");

		if (_scoreF > _palierMeterF) {
			_palierMeterF += 10;
			_multiplierXF++;
			_multiplierAn.Play ("Shake");
		}

		_scoreUI.text = "" + (int)_scoreF;
		_timerUI.text = _minutes + " : " + _seconds;
		_multiplierUI.text = "X" + (int)_multiplierXF;
	}

	public void BreakMultiplicator ()
	{
		_multiplierXF = 1;
		_multiplierAn.Play ("Shake");
	}
}
