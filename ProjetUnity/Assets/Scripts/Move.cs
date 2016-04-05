using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{

	GameObject m_pCase0;
	GameObject m_pCase1;
	GameObject m_pCase2;
	GameObject m_pCase3;
	GameObject m_pCase4;

	Camera m_pCam;

	bool m_bIsInFury = false;

	public float m_fTurningSpeed = 10f;
	public float m_fForwardImpulseForce = 10f;

	public float m_fFurySpeed = 100f;

	public float threshold = 1.0f;
	public MicroInput micIn;
	public Move _moveScript;

	void Awake ()
	{
		SetPointers ();
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
		ComputeInputs ();
		//Debug.Log (GetActualPos ());
	}

	void SetPointers ()
	{
		m_pCase0 = GameObject.Find ("Case 0");
		m_pCase1 = GameObject.Find ("Case 1");
		m_pCase2 = GameObject.Find ("Case 2");
		m_pCase3 = GameObject.Find ("Case 3");
		m_pCase4 = GameObject.Find ("Case 4");
		m_pCam = Camera.main;
	}

	void ComputeInputs ()
	{
		if (Input.GetKeyDown (KeyCode.F)) {
		
			ToggleIsInFury ();
			Debug.Log (GetIsInFury ());
		}
			

		if (!GetIsInFury ()) {
		
			//LEFT OR RIGHT
			if (Input.GetKey (KeyCode.LeftArrow)) {
				transform.position = Vector3.Lerp (GetActualPos (), new Vector3 (m_pCase1.transform.position.x, GetActualPos ().y, GetActualPos ().z), Time.deltaTime * m_fTurningSpeed);
				Debug.Log ("FarLeft");
			} else if (Input.GetKey (KeyCode.UpArrow)) {
				transform.position = Vector3.Lerp (GetActualPos (), new Vector3 (m_pCase2.transform.position.x, GetActualPos ().y, GetActualPos ().z), Time.deltaTime * m_fTurningSpeed);
				Debug.Log ("Left");
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				transform.position = Vector3.Lerp (GetActualPos (), new Vector3 (m_pCase3.transform.position.x, GetActualPos ().y, GetActualPos ().z), Time.deltaTime * m_fTurningSpeed);
				Debug.Log ("Right");
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				transform.position = Vector3.Lerp (GetActualPos (), new Vector3 (m_pCase4.transform.position.x, GetActualPos ().y, GetActualPos ().z), Time.deltaTime * m_fTurningSpeed);
				Debug.Log ("FarRight");
			} else {
				transform.position = Vector3.Lerp (GetActualPos (), new Vector3 (m_pCase0.transform.position.x, GetActualPos ().y, GetActualPos ().z), Time.deltaTime * m_fTurningSpeed);
				Debug.Log ("Center");
			}

			//FORWARD
			if (Input.GetKeyDown (KeyCode.Space)) {

				GetComponent<Rigidbody> ().AddForce (Vector3.forward * Time.deltaTime * m_fForwardImpulseForce, ForceMode.Impulse);
				m_pCam.GetComponent<Rigidbody> ().AddForce (Vector3.forward * Time.deltaTime * m_fForwardImpulseForce, ForceMode.Impulse);
			}

			float _l = micIn.loudness;
			if (_l > threshold) {
				GetComponent<Rigidbody> ().AddForce (Vector3.forward * Time.deltaTime * m_fForwardImpulseForce, ForceMode.Impulse);
				m_pCam.GetComponent<Rigidbody> ().AddForce (Vector3.forward * Time.deltaTime * m_fForwardImpulseForce, ForceMode.Impulse);
			}
		} else if (GetIsInFury ()) {
		
			//LEFT OR RIGHT
			if (Input.GetKey (KeyCode.LeftArrow)) {
				transform.position = Vector3.Lerp (GetActualPos (), new Vector3 (m_pCase1.transform.position.x, GetActualPos ().y, GetActualPos ().z), Time.deltaTime * m_fFurySpeed / 2f);
				Debug.Log ("FarLeft");
			} else if (Input.GetKey (KeyCode.UpArrow)) {
				transform.position = Vector3.Lerp (GetActualPos (), new Vector3 (m_pCase2.transform.position.x, GetActualPos ().y, GetActualPos ().z), Time.deltaTime * m_fFurySpeed / 2f);
				Debug.Log ("Left");
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				transform.position = Vector3.Lerp (GetActualPos (), new Vector3 (m_pCase3.transform.position.x, GetActualPos ().y, GetActualPos ().z), Time.deltaTime * m_fFurySpeed / 2f);
				Debug.Log ("Right");
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				transform.position = Vector3.Lerp (GetActualPos (), new Vector3 (m_pCase4.transform.position.x, GetActualPos ().y, GetActualPos ().z), Time.deltaTime * m_fFurySpeed / 2f);
				Debug.Log ("FarRight");
			} else {
				transform.position = Vector3.Lerp (GetActualPos (), new Vector3 (m_pCase0.transform.position.x, GetActualPos ().y, GetActualPos ().z), Time.deltaTime * m_fFurySpeed / 2f);
				Debug.Log ("Center");
			}

			//FORWARD
			if (Input.GetKeyDown (KeyCode.Space)) {

				GetComponent<Rigidbody> ().AddForce (Vector3.forward * Time.deltaTime * m_fFurySpeed, ForceMode.Impulse);
				m_pCam.GetComponent<Rigidbody> ().AddForce (Vector3.forward * Time.deltaTime * m_fFurySpeed, ForceMode.Impulse);
			}
			float _l = micIn.loudness;
			if (_l > threshold) {
				GetComponent<Rigidbody> ().AddForce (Vector3.forward * Time.deltaTime * m_fFurySpeed, ForceMode.Impulse);
				m_pCam.GetComponent<Rigidbody> ().AddForce (Vector3.forward * Time.deltaTime * m_fFurySpeed, ForceMode.Impulse);
			}
		}

	}

	public bool GetIsInFury ()
	{
		return m_bIsInFury;
	}

	public void ToggleIsInFury ()
	{
		m_bIsInFury = !m_bIsInFury;
	}

	public Vector3 GetActualPos ()
	{
		Vector3 vPos = this.gameObject.transform.position;
		return vPos;
	}

	void OnCollisionEnter (Collision col)
	{

		if (col.gameObject.tag == "Obstacle") {
		
			StartCoroutine (RespawnAfterHit ());
			GetComponent<ScoreManager> ().BreakMultiplicator ();

		}

	}

	IEnumerator RespawnAfterHit ()
	{
		Debug.Log ("Hit !");
		GetComponent<Collider> ().enabled = false;
		Vector3 vPlayerPos = GetActualPos ();
		Vector3 vCamPos = m_pCam.transform.position;
		transform.position = new Vector3 (0, 0, GetActualPos ().z);
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
		m_pCam.transform.position = new Vector3 (0, vCamPos.y, vCamPos.z);
		m_pCam.GetComponent<Rigidbody> ().velocity = Vector3.zero;
	
		GetComponent<Renderer> ().material.color = new Color (1, 0, 0, 0.2f);
		yield return new WaitForSeconds (0.25f);
		GetComponent<Renderer> ().material.color = new Color (1, 0, 0, 1f);
		yield return new WaitForSeconds (0.25f);
		GetComponent<Renderer> ().material.color = new Color (1, 0, 0, 0.2f);
		yield return new WaitForSeconds (0.25f);
		GetComponent<Renderer> ().material.color = new Color (1, 0, 0, 1f);
		yield return new WaitForSeconds (0.25f);
		GetComponent<Renderer> ().material.color = new Color (1, 0, 0, 0.2f);
		yield return new WaitForSeconds (0.25f);
		GetComponent<Renderer> ().material.color = new Color (1, 0, 0, 1f);
		yield return new WaitForSeconds (0.25f);
		GetComponent<Renderer> ().material.color = new Color (1, 0, 0, 0.2f);
		yield return new WaitForSeconds (0.25f);
		GetComponent<Renderer> ().material.color = new Color (1, 0, 0, 1f);
		yield return new WaitForSeconds (0.25f);
		GetComponent<Renderer> ().material.color = new Color (1, 0, 0, 1f);
		GetComponent<Collider> ().enabled = true;
	}
}
