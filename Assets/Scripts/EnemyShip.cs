using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyShip : MonoBehaviour {

	public bool inPosition;
	public AudioClip laserSound;
	public AudioClip crashSound;

	Vector3 screenPos;
	Vector3 targetPosition;
	float health;
	float speed;
	float laserSpeed;
	Player player;
	Text scoreText;
	GameObject powerup;
	float enterTime;
	Vector3 initialSpawnPos;

	void OnEnable () {

		inPosition = false;
		player = GameObject.FindObjectOfType<Player> ();
		scoreText = GameObject.Find ("Score").GetComponent<Text> ();
		health = 30f;
		laserSpeed = 7f;
		enterTime = Time.time + 2;
		initialSpawnPos = new Vector3 (0, 2.5f, 0);

		comeAlive ();
	}

	void OnDisable () {

		CancelInvoke ();
	}

	void Update () {

		if (!inPosition) {

			transform.position = Vector3.MoveTowards (transform.position, targetPosition, 4 * Time.deltaTime);
			if (transform.position.y <= targetPosition.y || (Time.time > enterTime && transform.position.y < 4.1f)) {
				inPosition = true;
			}
		} else {
			
			transform.position += new Vector3 (speed * Time.deltaTime, 0, 0);
			if ((transform.position.x >= screenPos.x) || (transform.position.x <= -screenPos.x)) {
				speed = -(speed / Mathf.Abs (speed)) * Random.Range (1.0f, 2.5f);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {

		if (collider.tag == "PlayerLaser" && collider.GetComponent<Laser> ().hit == 0) {

			collider.GetComponent<Laser> ().hit = 1;
			collider.gameObject.SetActive (false);
			health -= 10f;
			if (health <= 0) {

				GameObject explosion = ObjectPooler.instance.getPooledObject ("Explosion");
				explosion.transform.position = transform.position;
				GameState.score = GameState.score + player.scoreRate * 5;
				scoreText.text = GameState.score.ToString ();
				if (crashSound != null) {
					AudioSource.PlayClipAtPoint (crashSound, transform.position);
				}
				createPowerUps ();
				GameState.activeEnemyCount--;
				gameObject.SetActive(false);
			}
		}
	}

	public void comeAlive () {

		transform.position = new Vector3 (targetPosition.x, 6.5f, transform.position.z);
		screenPos = Camera.main.ScreenToWorldPoint(new Vector3 (Screen.width * 0.97f, 0, 0));
		speed = Random.Range (1.0f, 3.0f);
		targetPosition = new Vector3(Random.Range(-screenPos.x, screenPos.x), Random.Range(initialSpawnPos.y - 1.5f, initialSpawnPos.y + 1.5f), transform.position.z);

		Invoke ("fireEnemyLaser", Random.Range (1f, 4f)); //Random range is the rate of enemy shots
	}

	void fireEnemyLaser () {

		AudioSource.PlayClipAtPoint (laserSound, transform.position);
		GameObject eLaser = ObjectPooler.instance.getPooledObject ("EnemyLaser");
		eLaser.transform.position = transform.position;
		eLaser.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, -laserSpeed, 0);
		Invoke ("fireEnemyLaser", Random.Range (1f, 4f));
	}

	void createPowerUps () {

		//Probability of each powerup = 5%
		if (Random.Range (0, 20) == 1) {
			powerup = ObjectPooler.instance.getPooledObject ("Life");
		}
		if (Random.Range (0, 20) == 2) {
			powerup = ObjectPooler.instance.getPooledObject ("PowerUp");
		}
		if (Random.Range (0, 20) == 3) {
			powerup = ObjectPooler.instance.getPooledObject ("Bonus");
		}
		if (powerup != null) {
			powerup.transform.position = transform.position;
		}
	}
}