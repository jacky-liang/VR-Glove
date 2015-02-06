using UnityEngine;
using System.Collections;

public class interact_hand : MonoBehaviour {

	// Use this for initialization
	void Start () {
		HandCollisionResponse.setHand (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy(){
		HandCollisionResponse.unsetHand ();
	}
}
