using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Asteroids : MonoBehaviour {

	Vector3 screenPos;
	Vector3 asteroidPos;
	float asteroidTimer;
	float size;

	void OnEnable () {

		screenPos = Camera.main.ScreenToWorldPoint(new Vector3 (Screen.width * 0.97f, 0, 0));
		asteroidTimer = Time.time + 1;
	}
	
	void Update () {

		if (Time.time > asteroidTimer) {

			asteroidTimer = Time.time + 3;
			GameObject asteroid = ObjectPooler.instance.getPooledObject ("Asteroid");
			asteroid.transform.position = new Vector3 (Random.Range(-screenPos.x, screenPos.x), transform.position.y, transform.position.z);
			size = Random.Range (0.3f, 1.0f);
			asteroid.transform.parent = transform;
			asteroid.transform.localScale = new Vector3 (size, size, 1);
		}
	}
}