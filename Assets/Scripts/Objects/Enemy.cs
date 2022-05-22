using UnityEngine;

public class Enemy : MonoBehaviour {

	//Assign in inspector
	public GameController gameController;
	public AudioController audioController;

	public bool inPosition;

	private int health;
	private EnemySpawner spawner;
	private UIController uiController;
	private Vector3 screenPos;
	private Vector3 targetPosition;
	private float speed;
	private Player player;
	private float enterTime;
	private Vector3 initialSpawnPos;

	void OnEnable () {

		spawner = gameObject.transform.GetComponentInParent<EnemySpawner> ();
		audioController = spawner.audioController;
		uiController = spawner.uiController;
		player = spawner.player;
		inPosition = false;
		health = GameConstants.MAX_ENEMY_HEALTH;
		enterTime = Time.time + 2;
		initialSpawnPos = new Vector3 (0, 2.5f, 0);
		ComeAlive ();
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
		}
		else {
			
			transform.position += new Vector3 (speed * Time.deltaTime, 0, 0);
			if ((transform.position.x >= screenPos.x) || (transform.position.x <= -screenPos.x)) {
				speed = -(speed / Mathf.Abs (speed)) * Random.Range (1.0f, 2.5f);
			}
		}
	}

	void OnTriggerEnter2D (Collider2D collider) {

		if (collider.gameObject.TryGetComponent (out Laser laser)) {

			if (laser.type == GameConstants.LaserType.PLAYER && !laser.used) {

				laser.used = true;
				collider.gameObject.SetActive (false);
				GotHit ();
			}
		}
	}

	void GotHit () {

		health -= 10; // 10f is laser power;
		GameObject explosion = ObjectPooler.instance.GetPooledObject (GameConstants.PooledObject.EXPLOSION);
		if (health <= 0) {

			if (transform.childCount > 0) {

				foreach (Transform child in transform) { //return any existing explosions back to pool
					child.GetComponent<Explosion> ().DeactivateGO ();
				}
			}
			gameObject.SetActive (false);
			explosion.GetComponent<Explosion> ().CreateExplosion (gameObject, true);
			audioController.PlaySound (audioController.crash, transform.position);
			ScoreController.instance.AddScore (player.scoreRate * 5);
			uiController.UpdateScore ();
			CreatePowerUps ();
			GameState.activeEnemyCount--;
		}
		else {
			explosion.GetComponent<Explosion> ().CreateExplosion (gameObject, false);
		}
	}

	public void ComeAlive () {

		transform.position = new Vector3 (targetPosition.x, 6.5f, transform.position.z);
		screenPos = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width * 0.97f, 0, 0));
		speed = Random.Range (1.0f, 3.0f);
		targetPosition = new Vector3 (Random.Range (-screenPos.x, screenPos.x), Random.Range (initialSpawnPos.y - 1.5f, initialSpawnPos.y + 1.5f), transform.position.z);
		Invoke (nameof (FireLaser), Random.Range (1f, 4f));
	}

	void FireLaser () {

		audioController.PlaySound (audioController.enemyLaser, transform.position);
		GameObject laser = ObjectPooler.instance.GetPooledObject (GameConstants.PooledObject.ENEMY_LASER);
		laser.transform.position = transform.position;
		laser.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, -GameConstants.ENEMY_LASER_SPEED, 0);
		Invoke (nameof (FireLaser), Random.Range (1f, 4f));
	}

	void CreatePowerUps () {

		//Probability of each powerup = 5%
		GameObject powerup = null;
		if (Random.Range (0, 20) == 1) {
			powerup = ObjectPooler.instance.GetPooledObject (GameConstants.PooledObject.LIFE);
		}
		else if (Random.Range (0, 20) == 2) {
			powerup = ObjectPooler.instance.GetPooledObject (GameConstants.PooledObject.POWER_UP);
		}
		else if (Random.Range (0, 20) == 3) {
			powerup = ObjectPooler.instance.GetPooledObject (GameConstants.PooledObject.BONUS);
		}
		if (powerup != null) {
			powerup.transform.position = transform.position;
		}
	}
}