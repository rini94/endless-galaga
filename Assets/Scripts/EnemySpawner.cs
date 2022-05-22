using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour {

	//Assigned in inspector - Used by enemy object
	public AudioController audioController;
	public UIController uiController;
	public Player player;

	private float spawnTimer;
	private bool spawnEnemies;
	private float spawnGap;

	void OnEnable () {

		SceneManager.sceneLoaded += SceneEnabled;
	}

	void Update () {

		if (ScoreController.instance.GetScore () > 200) {
			spawnGap = 1f;
		}
		else if (ScoreController.instance.GetScore () > 100) {
			spawnGap = 1.5f;
		}
		else if (ScoreController.instance.GetScore () > 50) {
			spawnGap = 2f;
		}
		if (spawnEnemies && Time.time > spawnTimer && GameState.activeEnemyCount < GameConstants.MAX_ENEMY_COUNT) {

			spawnTimer = Time.time + spawnGap;
			ActivateEnemy ();
		}
	}

	void SceneEnabled (Scene scene, LoadSceneMode mode) {

		if (GameState.currentState == GameConstants.GameStates.PLAYING) {

			player = FindObjectOfType<Player> ();
			audioController = FindObjectOfType<AudioController> ();
			uiController = FindObjectOfType<UIController> ();
			StartSpawning ();
		}
	}

	void ActivateEnemy () {

		GameObject enemy = ObjectPooler.instance.GetPooledObject (GameConstants.PooledObject.ENEMY);
		enemy.GetComponent<Enemy> ().enabled = true;
		GameState.activeEnemyCount++;
	}

	private void StartSpawning () {

		spawnEnemies = true;
		spawnTimer = Time.time + 1;
		spawnGap = 3f;
	}

	public void StopSpawning () {

		spawnEnemies = false;
	}
}