using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	private GameObject[] pausedObjects;
	private GameObject pauseButton;
	private string currentScene;

	void Start () {

		Time.timeScale = 1;
		currentScene = SceneManager.GetActiveScene ().name;

		if (currentScene == "Game") {
			
			GameState.currentState = GameState.GameStates.PLAYING;
			GameState.currentLevel = 1;
			GameState.score = 0;
			GameState.activeEnemyCount = 0;
			pauseButton = GameObject.Find ("Pause");
			pausedObjects = GameObject.FindGameObjectsWithTag ("PausedScreen");
			hidePaused ();

		} else if (currentScene == "Menu") {
			ObjectPooler.instance.deactivateAll ();
		}
	}
	
	void Update () {

		if (currentScene == "Game") {
			if (Input.GetKeyDown (KeyCode.P)) {
				if (GameState.currentState == GameState.GameStates.PAUSED) {
					resumeGame ();
				} else {
					pauseGame ();
				}
			}
		}
	}

	public void loadScene (string sceneName) {

		SceneManager.LoadScene (sceneName);
	}

	public void quitGame () {

		Application.Quit ();
	}

	public void pauseGame () {

		GameState.currentState = GameState.GameStates.PAUSED;
		Time.timeScale = 0;
		foreach(GameObject gObj in pausedObjects) {
			gObj.SetActive(true);
		}
		pauseButton.SetActive(false);
	}

	void hidePaused() {

		foreach(GameObject gObj in pausedObjects) {
			gObj.SetActive(false);
		}
		pauseButton.SetActive(true);
	}

	public void resumeGame () {

		GameState.currentState = GameState.GameStates.PLAYING;
		Time.timeScale = 1;
		hidePaused ();
	}

	public void restartGame () {

		ObjectPooler.instance.deactivateAll ();
		loadScene ("Game");
	}
}