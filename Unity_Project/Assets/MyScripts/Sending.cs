using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class Sending : MonoBehaviour {
    //public static SerialPort sp = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
	public static SerialPort sp = new SerialPort("COM3", 9600);
	public static int[] positions = {0,0,0,0,0,0};
	public static int[] levels = {0,0,0,0,0,0};

	void Start () {
		OpenConnection();
	}

	void OnApplicationQuit() 
	{
		sp.Close();
	}

	public void OpenConnection() 
    {
       if (sp != null) 
         if (sp.IsOpen) 
         {
          sp.Close();
          print("Closing port, because it was already open!");
         }
         else 
         {
          sp.Open();  // opens the connection
          sp.ReadTimeout = 16;  // sets the timeout value before reporting error
          print("Port Opened!");
         }
	   else 
         if (sp.IsOpen)
          print("Port is already open");
         else 
          print("Port == null");
    }

	public static char intToNumChar(int i){
		return (char)(i + 48);
	}

	public static string constructData(int dType, int dData, int iType){
		char[] data = new char[17];
		data [0] = '&';
		data [1] = intToNumChar(dType);
		data [2] = intToNumChar(dData);
		data [3] = intToNumChar(iType);
		for (int i = 4; i<10; i++) 
			data [i] = intToNumChar(positions[i-4]);
		for (int i = 10; i<16; i++) 
			data [i] = intToNumChar(levels[i-10]);
		data [16] = '%';
		return new string (data);
	}

	public static void resetPositions(){
		for (int i = 0; i<6; i++)
			positions[i]=0;
	}

	public static void resetLevels(){
		for (int i = 0; i<6; i++)
			levels[i]=0;
	}

	public static void resetOutputs(){
		resetPositions ();
		resetLevels ();
	}

	public static void setAPosition(int pos){
		positions [pos] = 1;;
		//printPositions ();
	}

	public static void setMultiplePositions(int[] set_positions){
		for (int i = 0; i<6; i++)
			setAPosition(set_positions[i]);
	}

	public static void setALevel(int i, int level){
		levels [i] = level;
	}
	
	public static void setMultipleLevels(int[] levels){
		for (int i = 0; i<6; i++)
			setALevel(i,levels[i]);
	}

	public static void printPositions(){
		Debug.Log ("Positions: ");
		for (int i = 0; i<6; i++)
		Debug.Log(i+" is "+positions[i]);
	}

	public static void vibrate(int pos, int dType, int dData, int iType, int iData){
		resetOutputs ();
		setAPosition (pos);
		setALevel (pos,iData);
		string msg = constructData (dType,dData,iType);
		Debug.Log ("single msg is " + msg);	
		sp.Write(msg);
	}

	public static void vibrateMultiple(int[] set_positions, int dType, int dData, int iType, int[] iData){
		resetOutputs ();
		setMultiplePositions (set_positions);
		setMultipleLevels (iData);
		string msg = constructData (dType,dData,iType);
		Debug.Log ("multiple msg is " + msg);	
		sp.Write(msg);
	}

	public static void stopVibrate(int pos){
		vibrate (pos, 0,0,0,0);
	}

	public static void startVibrate(int pos,int iType, int iData){
		vibrate (pos, 0,1,iType,iData);
	}

	public static void testVibrate(int pos){
		vibrate (pos, 1, 1, 0, 9);
	}

	public static void testVibrateAll(){
		int[] test_positions = {0,1,2,3,4,5};
		int[] test_levels = {3,4,5,6,7,8};
		vibrateMultiple (test_positions, 1, 1, 0, test_levels);
	}
}
