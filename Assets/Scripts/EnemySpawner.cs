using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour {

	public AudioClip laserSound;
	public AudioClip crashSound;

	private float spawnTimer;
	private bool spawnEnemies;
	private float spawnGap;
	private int currentScore;

	void OnEnable () {

		SceneManager.sceneLoaded += GameLoaded;
	}
	
	void Update () {

		currentScore = GameState.score;
		if (currentScore > 200) {
			spawnGap = 1f;
		} else if (currentScore > 100) {
			spawnGap = 1.5f;
		} else if (currentScore > 50) {
			spawnGap = 2f;
		} 

		if (spawnEnemies && Time.time > spawnTimer && GameState.activeEnemyCount < GameState.maxEnemyCount) {
			spawnTimer = Time.time + spawnGap;
			activateEnemy ();
		}
	}

	void activateEnemy () {

		GameObject enemy = ObjectPooler.instance.getPooledObject("EnemyShip");
		enemy.GetComponent<EnemyShip> ().enabled = true;
		GameState.activeEnemyCount++;
	}

	private void GameLoaded (Scene scene, LoadSceneMode mode) {

		if (SceneManager.GetActiveScene ().name == "Game") {
			spawnEnemies = true;
			spawnTimer = Time.time + 1;
			spawnGap = 3f;
		} else {
			spawnEnemies = false;
		}
	}
}