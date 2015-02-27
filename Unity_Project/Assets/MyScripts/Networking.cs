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

	static void GetToServer(string query){
		Debug.Log ("get to server with query: " + query);

		//Sending Request
		WebRequest request = WebRequest.Create (serverDest+query);
		//request.Method = "GET";
		/*
		byte[] byteArray = Encoding.UTF8.GetBytes (query);
		request.ContentType = "application/x-www-form-urlencoded";
		request.ContentLength = byteArray.Length;
		//writing request to data stream
		Stream dataStream = request.GetRequestStream ();
		dataStream.Write (byteArray, 0, byteArray.Length);
		dataStream.Close ();*/

		//Getting Response
		WebResponse response = request.GetResponse ();
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
		GetToServer ("?data="+msg);
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
