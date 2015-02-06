using UnityEngine;
using System.Collections.Generic;
public class CollisionResponse : MonoBehaviour {

	string targetName = "bone3";
	float max_magnitude = 10.0f;
	Dictionary <string,int> handMotors = new Dictionary <string,int>(){
		{"bone3",0},
		{"ri",1},
		{"rm",2},
		{"rr",3},
		{"rp",4}
	};


	int GetMotorID(string name){
		int motorId = handMotors[name];
		return motorId;
	}

	int GetIntensityLevel(float magnitude){
		if (magnitude > max_magnitude)
			return 9;
		if (magnitude / max_magnitude < 0.3)
			return 3;
		return (int)(magnitude/max_magnitude*9);
	}
	
	void OnCollisionEnter(Collision collision) {
		string objectName = collision.gameObject.name;
		if (objectName.Contains(targetName)) {
			int intensity = GetIntensityLevel(collision.relativeVelocity.magnitude);
			int motorID = GetMotorID(objectName);
			Debug.Log(intensity);
			Sending.startVibrate (motorID,0,intensity);
		}
	}

	void OnCollisionExit(Collision collision){
		string objectName = collision.gameObject.name;
		if (objectName.Contains(targetName)) {
			int motorID = GetMotorID(objectName);
			Sending.stopVibrate (motorID);
		}
	}
}
