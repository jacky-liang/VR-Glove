using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class HandCollisionResponse : MonoBehaviour {

	static float max_intensity = 20.0f;

	//Hard-coded position variables
	static Dictionary <string,object> allBones = new Dictionary <string,object>(){
		{"bone1thumb",new Vector2(8f,2f)},
		{"bone2thumb",new Vector2(8f,5.5f)},
		{"bone3thumb",new Vector2(8f,7.6f)},

		{"bone1index",new Vector2(6.4f,8.5f)},
		{"bone2index",new Vector2(6.4f,11.5f)},
		{"bone3index",new Vector2(6.4f,13.5f)},

		{"bone1middle",new Vector2(4.3f,9.6f)},
		{"bone2middle",new Vector2(4.3f,12.8f)},
		{"bone3middle",new Vector2(4.3f,14.8f)},
		
		{"bone1ring",new Vector2(2.5f,9.6f)},
		{"bone2ring",new Vector2(2.5f,12.7f)},
		{"bone3ring",new Vector2(2.5f,14.6f)},

		{"bone1pinky",new Vector2(0.9f,9f)},
		{"bone2pinky",new Vector2(0.9f,11.2f)},
		{"bone3pinky",new Vector2(0.9f,12.8f)},

		{"palm",new Vector2(3.5f,4f)}
	};

	static Dictionary<int,object> outsideMotors = new Dictionary<int, object>(){
		{0, new Vector2(3f,4f)},
		{1, new Vector2(5.2f,10.2f)},
		{2, new Vector2(1.5f,8.5f)}
	};

	static Dictionary<int,object> insideMotors = new Dictionary<int, object>(){
		{3, new Vector2(3.8f,4.8f)},
		{4, new Vector2(4.6f,10.6f)},
		{5, new Vector2(2f,8f)}
	};

	static Dictionary<string, Dictionary<int,object>> setMotors = new Dictionary<string,Dictionary<int,object>>(){
		{"inside",insideMotors},
		{"outside",outsideMotors}
	};
	
	static GameObject hand = null;

	string[] boneNames = {"bone1","bone2","bone3","palm"};

	//Helper Functions
	int GetIntensityLevel(float intensity){
		if (intensity > max_intensity)
			return 9;
		if (intensity / max_intensity < 0.3)
			return 3;
		return (int)(intensity/max_intensity*9);
	}

	//Calculating k's, the magnitude decay constant used to calculate vibration intensity
	float getK(Vector2 bone, string affectedMotors){
		float[] ds = new float[3];
		int i = 0;
		foreach (object value in setMotors[affectedMotors].Values) {
			Vector2 dif = (Vector2) value - bone;
			ds[i] = dif.magnitude;
			i++;
		}

		float d1 = ds [0];
		float d2 = ds [1];
		float d3 = ds [2];

		return (d1*d2*d3)/(d1*d2+d2*d3+d3*d1);
	}

	public static void setHand(GameObject hand){
		HandCollisionResponse.hand = hand;
	}

	public static void unsetHand(){
		HandCollisionResponse.hand = null;
	}

	Vector3 averageContacts(ContactPoint[] contacts){
		Vector3 resultant = new Vector3 (0, 0, 0);
		foreach (ContactPoint contact in contacts) {
			resultant += contact.normal;
		}
		return resultant;
	}

	GameObject getPalm(){
		foreach (Transform t in hand.transform)
			if(t.name == "palm")
				return t.gameObject;
		return null;
	}

	bool collisionDirection(ContactPoint[] contacts){
		Vector3 contact_normal = averageContacts(contacts);
		Vector3 palm_normal = getPalm().transform.rotation.eulerAngles;
		float angle_difference = Vector3.Angle(contact_normal,palm_normal);

		return angle_difference < 90.0f;
	}

	float getSignalStrength(Vector2 motor, Vector2 bone, double magnitude, float k){
		Vector2 dif = motor - bone;
		float d = dif.magnitude;
		return k/d*(float)magnitude;
	}

	void OnCollisionEnter(Collision collision) {
		GameObject collided = collision.gameObject;
		string collided_name = collided.name;

		//checking if collided with a hand
		if (boneNames.Contains(collided_name)) {
			//get bone position
			Vector2 bone_position = new Vector2();
			if(collided_name == "palm")
				bone_position = (Vector2) allBones[collided_name];
			else{ 
				string parent_name = collided.transform.parent.gameObject.name;
				bone_position = (Vector2) allBones[collided_name+parent_name];
			}

			//Getting key and k for affected motors
			string affectedMotors = "inside";
			bool isOutsideCollision = collisionDirection(collision.contacts);
			if(isOutsideCollision)
				affectedMotors = "outside";
			float k = getK (bone_position,affectedMotors);
				
			//loop through affected motors
			//associate motor id with calculated signal strength
			double magnitude = collision.relativeVelocity.magnitude;
			Dictionary <int,float> motorSignals = new Dictionary<int,float>();
			foreach(KeyValuePair <int, object> motor in setMotors[affectedMotors])
				motorSignals.Add(motor.Key,getSignalStrength((Vector2)motor.Value,bone_position,magnitude,k));

			//Construct Parameters to pass to Sending
			int[] levels = {0,0,0,0,0,0};
			int[] positions = {0,0,0,0,0,0};
			int i = 0;
			//unit is specified in arduino
			int period = 1;
			foreach(KeyValuePair <int,float> signal in motorSignals){
				positions[i] = signal.Key;
				levels[i] = GetIntensityLevel(signal.Value);
				i++;
			}

			Sending.vibrateMultiple(positions,1,period,0,levels);

		}
	}
}
