using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class MicroInput : MonoBehaviour
{
	public float sensitivity = 100.0f;
	public float loudness = 0.0f;
	public float frequency = 0.0f;
	public int samplerate = 11024;
	// Use this for initialization
	void Start ()
	{
	
		foreach (string device in Microphone.devices) {
			Debug.Log ("Name: " + device);
		}

		GetComponent<AudioSource> ().clip = Microphone.Start (Microphone.devices [0], true, 1, samplerate);
		GetComponent<AudioSource> ().loop = true; // Set the AudioClip to loop
		GetComponent<AudioSource> ().mute = false; // Mute the sound, we don't want the player to hear it

		GetComponent<AudioSource> ().Play (); // Play the audio source!
	}

	void Update ()
	{
		loudness = GetAveragedVolume () * sensitivity;
	}

	float GetAveragedVolume ()
	{ 
		float[] data = new float[256];
		float a = 0;
		GetComponent<AudioSource> ().GetOutputData (data, 0);
		foreach (float s in data) {
			a += Mathf.Abs (s);
		}
		return a / 256;
	}
}