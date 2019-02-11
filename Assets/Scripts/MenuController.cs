using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	private GameObject[] mainScreenObjects;
	private GameObject[] instructionObjects;

	void Start () {
		
		if (SceneManager.GetActiveScene ().name == "Menu") {
			instructionObjects = GameObject.FindGameObjectsWithTag ("InstructionsScreen");
			mainScreenObjects = GameObject.FindGameObjectsWithTag ("MainScreen");
			Text highestScore = GameObject.Find ("HighestScore").GetComponent<Text> ();
			highestScore.text = PlayerPrefs.GetInt ("highestscore").ToString ();
			hideInstructions ();

		} else if (SceneManager.GetActiveScene ().name == "Finish") {
			Text finalScore = GameObject.Find ("FinalScore").GetComponent<Text> ();
			finalScore.text = GameState.score.ToString ();
		}
	}

	public void showInstructions () {

		foreach (GameObject gObj in mainScreenObjects) {
			gObj.SetActive (false);
		}
		foreach (GameObject gObj in instructionObjects) {
			gObj.SetActive (true);
		}
	}

	public void hideInstructions () {

		foreach (GameObject gObj in instructionObjects) {
			gObj.SetActive (false);
		}
		foreach (GameObject gObj in mainScreenObjects) {
			gObj.SetActive (true);
		}
	}
}