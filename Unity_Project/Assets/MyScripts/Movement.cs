using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	bool left = false;
	bool right = false;
	bool up = false;
	bool down = false;
	float acc = 0.1f;
	float left_t = 0f;
	float right_t = 0f;
	float up_t = 0f;
	float down_t = 0f;
	
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		Vector3 position = this.transform.position;

		if(Input.GetKeyDown(KeyCode.LeftArrow))
			left = true;

		if (Input.GetKeyDown(KeyCode.RightArrow))
			right = true;

		if(Input.GetKeyUp(KeyCode.LeftArrow))
			left = false;

		if (Input.GetKeyUp(KeyCode.RightArrow))
			right = false;

		if(Input.GetKeyDown(KeyCode.UpArrow))
			up = true;
		
		if (Input.GetKeyDown(KeyCode.DownArrow))
			down = true;
		
		if(Input.GetKeyUp(KeyCode.UpArrow))
			up = false;
		
		if (Input.GetKeyUp(KeyCode.DownArrow))
			down = false;

		//move
		if(left){
			left_t += 0.05f;
			position.x -= left_t * acc;
			this.transform.position = position;
		}
		else{
			if(left_t>0){
				left_t -= 0.05f;
				position.x -= left_t * acc;
				this.transform.position = position;
			}
		}
		if (right) {
			right_t += 0.05f;
			position.x += right_t*acc;
			this.transform.position = position;
		}
		else{
			if(right_t>0){
				right_t -= 0.05f;
				position.x += right_t * acc;
				this.transform.position = position;
			}
		}
		if(down){
			down_t += 0.05f;
			position.z -= down_t * acc;
			this.transform.position = position;
		}
		else{
			if(down_t>0){
				down_t -= 0.05f;
				position.z -= down_t * acc;
				this.transform.position = position;
			}
		}
		if (up) {
			up_t += 0.05f;
			position.z += up_t*acc;
			this.transform.position = position;
		}
		else{
			if(up_t>0){
				up_t -= 0.05f;
				position.z += up_t * acc;
				this.transform.position = position;
			}
		}





}
}
