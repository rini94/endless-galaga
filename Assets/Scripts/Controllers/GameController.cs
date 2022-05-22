using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	//Assigned in inspector
	public UIController uiController;
	public AudioController audioController;
	public EnemySpawner enemySpawner;

	void Start () {

		Time.timeScale = 1;
		if (enemySpawner == null) {
			enemySpawner = FindObjectOfType<EnemySpawner> ();
		}
	}
	
	void Update () {

		if (Input.GetKeyDown (KeyCode.P)) {

			if (GameState.currentState == GameConstants.GameStates.PAUSED) {
				ResumeGame ();
			}
			else if (GameState.currentState == GameConstants.GameStates.PLAYING) {
				PauseGame ();
			}
		}
	}

	public void PauseGame () {

		ChangeGameState (GameConstants.GameStates.PAUSED);
		Time.timeScale = 0;
		uiController.ShowPauseScreen ();
	}

	public void ResumeGame () {

		ChangeGameState (GameConstants.GameStates.PLAYING);
		Time.timeScale = 1;
		uiController.HidePauseScreen ();
	}

	public void StartGame () {

		GameState.ResetGameState ();
		ChangeGameState (GameConstants.GameStates.PLAYING);
		SceneManager.LoadScene ("Game");
	}

	public void RestartGame () {

		ObjectPooler.instance.DeactivateAll ();
		StartGame ();
	}

	public void GameOver () {

		ScoreController.instance.UpdateHighestScore ();
		ChangeGameState (GameConstants.GameStates.ENDED);
		ObjectPooler.instance.DeactivateAll ();
		enemySpawner.StopSpawning ();
		uiController.ShowEndScreen ();
	}

	public void GoToMenu () {

		ObjectPooler.instance.DeactivateAll ();
		enemySpawner.StopSpawning ();
		ChangeGameState (GameConstants.GameStates.NOT_STARTED);
		SceneManager.LoadScene ("Menu");
	}

	public void ChangeGameState (GameConstants.GameStates state) {

		GameState.currentState = state;
		audioController = FindObjectOfType<AudioController> ();
		audioController.CheckPlayBGM ();
	}
}