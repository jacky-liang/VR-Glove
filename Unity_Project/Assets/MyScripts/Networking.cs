using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net;
using System.Text;

public class Networking : MonoBehaviour {

	public static string serverDest = "http://localhost/send";

	// Use this for initialization
	void Start () {
		Debug.Log ("Networking Service Started");
	}

	static IEnumerator GetToServer(string query){
		Debug.Log ("get to server with query: " + query);

		//Sending Request
		WebRequest request = WebRequest.Create (serverDest+query);

		//Getting Response
		WebResponse response = request.GetResponse ();

		yield return response;

		// Display the status.
		Console.WriteLine (((HttpWebResponse)response).StatusDescription);
		Stream dataStream = response.GetResponseStream ();
		StreamReader reader = new StreamReader (dataStream);
		string responseFromServer = reader.ReadToEnd ();
		Console.WriteLine (responseFromServer);

		// Clean up the streams.
		reader.Close ();
		dataStream.Close ();
		response.Close ();
	}

	public static void send(string msg){
		string query = "?data="+msg;
		StaticCoroutine.DoCoroutine(GetToServer(query));
	}

	static void testSend(){
		send ("HI");
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("n")) {
			Debug.Log("key press n logged");
			testSend();
		}
	}
}
