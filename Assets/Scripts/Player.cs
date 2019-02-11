using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public float maxHealth;
	public float health;
	public AudioClip laserSound;
	public int scoreRate;
	public Image healthBar;

	Vector3 screenBounds;
	float speed;
	float laserSpeed;
	float laserRate;
	float maxHeight;
	float bonusTime;
	Animator animator;
	Behaviour bonusHalo;
	bool hasEntered;
	GameController gameController;

	void Start () {

		health = maxHealth;
		healthBar = GameObject.Find("HealthBar").GetComponent<Image> ();
		gameController = GameObject.FindObjectOfType<GameController> ();
		screenBounds = Camera.main.ScreenToWorldPoint(new Vector3 (Screen.width * 0.97f, Screen.height * 0.95f, 0));
		speed = 8f;
		laserSpeed = 10f;
		laserRate = 0.3f;
		maxHeight = -(screenBounds.y * 0.5f);
		animator = gameObject.GetComponent<Animator> ();
		bonusHalo = gameObject.GetComponent ("Halo") as Behaviour;
		hasEntered = false;
		scoreRate = 1;
		bonusTime = 0;
	}

	void Update () {

		playerMovements ();
		if (Time.time <= bonusTime) {
			bonusHalo.enabled = true;
			scoreRate = 2;
		} else {
			bonusHalo.enabled = false;
			scoreRate = 1;
		}
		if (hasEntered) {
			animator.enabled = false;
		}
		if (Input.GetKeyDown (KeyCode.Space) && GameState.currentState == GameState.GameStates.PLAYING) {
			InvokeRepeating ("fireLaser", 0.0001f, laserRate);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke ();
		}
	}

	void playerMovements () {
	
		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.position = new Vector3 (Mathf.Clamp (transform.position.x - speed * Time.deltaTime, -screenBounds.x, screenBounds.x), transform.position.y, transform.position.z);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.position = new Vector3 (Mathf.Clamp (transform.position.x + speed * Time.deltaTime, -screenBounds.x, screenBounds.x), transform.position.y, transform.position.z);
		}
		if (Input.GetKey (KeyCode.UpArrow)) {
			transform.position = new Vector3 (transform.position.x, Mathf.Clamp (transform.position.y + speed * Time.deltaTime, -screenBounds.y, maxHeight), transform.position.z);
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			transform.position = new Vector3 (transform.position.x, Mathf.Clamp (transform.position.y - speed * Time.deltaTime, -screenBounds.y, maxHeight), transform.position.z);
		}
	}

	void fireLaser () {
	
		AudioSource.PlayClipAtPoint (laserSound, transform.position);

		if (GameState.currentLevel == 1) {
			createSingleLaser (transform.position);

		} else if (GameState.currentLevel == 2) {

			createSingleLaser (new Vector3 (transform.position.x - 0.2f, transform.position.y, transform.position.z));
			createSingleLaser (new Vector3 (transform.position.x + 0.2f, transform.position.y, transform.position.z));

		} else if (GameState.currentLevel == 3) {

			createSingleLaser (new Vector3 (transform.position.x - 0.25f, transform.position.y, transform.position.z));
			createSingleLaser (new Vector3 (transform.position.x, transform.position.y + 0.25f, transform.position.z));
			createSingleLaser (new Vector3 (transform.position.x + 0.25f, transform.position.y, transform.position.z));
		}
	}

	void createSingleLaser (Vector3 position) {
		
		GameObject laser = ObjectPooler.instance.getPooledObject ("PlayerLaser");
		laser.transform.position = position;
		laser.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, laserSpeed, 0);
	}

	void OnTriggerEnter2D (Collider2D collider) {
	
		if (collider.tag == "EnemyLaser") {
			collider.gameObject.SetActive(false);
			health -= 10f;// 10f is laser power;
			healthBar.fillAmount = health / maxHealth;
			if (health <= 0) {
				if (PlayerPrefs.GetInt ("highestscore") < GameState.score) {
					PlayerPrefs.SetInt ("highestscore", GameState.score);
				}
				ObjectPooler.instance.deactivateAll ();
				gameController.loadScene ("Finish");
			}
			if (GameState.currentLevel > 1) {
				GameState.currentLevel--;
			}
		}
	}

	void playerEntered () {

		hasEntered = true;
	}

	public void doublePoints () {

		bonusTime = Time.time + 10; //10 seconds of bonus points;
	}
}