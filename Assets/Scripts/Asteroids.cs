using UnityEngine;

public class Asteroids : MonoBehaviour {

	private Vector3 screenPos;
	private float asteroidTimer;

	void OnEnable () {

		screenPos = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width * 0.97f, 0, 0));
		asteroidTimer = Time.time + 1;
	}
	
	void Update () {

		if (Time.time > asteroidTimer) {

			asteroidTimer = Time.time + 3;
			GameObject asteroid = ObjectPooler.instance.GetPooledObject (GameConstants.PooledObject.ASTEROID);
			asteroid.transform.position = new Vector3 (Random.Range (-screenPos.x, screenPos.x), transform.position.y, transform.position.z);
			float size = Random.Range (0.3f, 1.0f);
			asteroid.transform.parent = transform;
			asteroid.transform.localScale = new Vector3 (size, size, 1);
		}
	}
}