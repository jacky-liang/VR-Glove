using UnityEngine;
using System.Collections;

public class arduino : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update(){
		if (Input.GetKeyDown ("a")) {
			Debug.Log("key press a logged");
			Sending.testVibrateAll();
		}
		if (Input.GetKeyDown ("1")) {
			Debug.Log("key press 1 logged");
			Sending.testVibrate(0);
		}
		if (Input.GetKeyDown ("2")) {
			Debug.Log("key press 2 logged");
			Sending.testVibrate(1);
		}
		if (Input.GetKeyDown ("3")) {
			Debug.Log("key press 3 logged");
			Sending.testVibrate(2);
		}
		if (Input.GetKeyDown ("4")) {
			Debug.Log("key press 4 logged");
			Sending.testVibrate(3);
		}
		if (Input.GetKeyDown ("5")) {
			Debug.Log("key press 5 logged");
			Sending.testVibrate(4);
		}
		if (Input.GetKeyDown ("6")) {
			Debug.Log("key press 6 logged");
			Sending.testVibrate(5);
		}

	}
}