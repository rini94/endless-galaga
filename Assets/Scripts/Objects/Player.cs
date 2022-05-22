using UnityEngine;

public class Player : MonoBehaviour {

	//Assign in inspector
	public UIController uiController;
	public GameController gameController;
	public AudioController audioController;
	public int scoreRate;

	private int health;
	private Vector3 screenBounds;
	private float maxHeight;
	private float bonusTime;
	private Animator animator;
	private Behaviour bonusHalo;
	private bool hasEntered;
	

	void Start () {

		health = GameConstants.MAX_PLAYER_HEALTH;
		screenBounds = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width * 0.97f, Screen.height * 0.95f, 0));
		maxHeight = -(screenBounds.y * 0.5f);
		animator = GetComponent<Animator> ();
		bonusHalo = GetComponent ("Halo") as Behaviour;
		hasEntered = false;
		scoreRate = 1;
		bonusTime = 0;
	}

	void Update () {

		HandlePlayerMovements ();
		if (Time.time <= bonusTime) {

			bonusHalo.enabled = true;
			scoreRate = 2;
		}
		else {

			bonusHalo.enabled = false;
			scoreRate = 1;
		}
		if (hasEntered) {
			animator.enabled = false;
		}
		if (Input.GetKeyDown (KeyCode.Space) && GameState.currentState == GameConstants.GameStates.PLAYING) {
			InvokeRepeating (nameof (FireLaser), 0.0001f, GameConstants.PLAYER_LASER_RATE);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke ();
		}
	}

	void HandlePlayerMovements () {
	
		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.position = new Vector3 (Mathf.Clamp (transform.position.x - GameConstants.PLAYER_MOVEMENT_SPEED * Time.deltaTime, -screenBounds.x, screenBounds.x), transform.position.y, transform.position.z);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.position = new Vector3 (Mathf.Clamp (transform.position.x + GameConstants.PLAYER_MOVEMENT_SPEED * Time.deltaTime, -screenBounds.x, screenBounds.x), transform.position.y, transform.position.z);
		}
		if (Input.GetKey (KeyCode.UpArrow)) {
			transform.position = new Vector3 (transform.position.x, Mathf.Clamp (transform.position.y + GameConstants.PLAYER_MOVEMENT_SPEED * Time.deltaTime, -screenBounds.y, maxHeight), transform.position.z);
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			transform.position = new Vector3 (transform.position.x, Mathf.Clamp (transform.position.y - GameConstants.PLAYER_MOVEMENT_SPEED * Time.deltaTime, -screenBounds.y, maxHeight), transform.position.z);
		}
	}

	void FireLaser () {
	
		audioController.PlaySound (audioController.playerLaser, transform.position);
		if (GameState.currentLevel == 1) {
			CreateLaser (transform.position);
		}
		else if (GameState.currentLevel == 2) {

			CreateLaser (new Vector3 (transform.position.x - 0.2f, transform.position.y, transform.position.z));
			CreateLaser (new Vector3 (transform.position.x + 0.2f, transform.position.y, transform.position.z));
		}
		else if (GameState.currentLevel == 3) {

			CreateLaser (new Vector3 (transform.position.x - 0.25f, transform.position.y, transform.position.z));
			CreateLaser (new Vector3 (transform.position.x, transform.position.y + 0.25f, transform.position.z));
			CreateLaser (new Vector3 (transform.position.x + 0.25f, transform.position.y, transform.position.z));
		}
	}

	void CreateLaser (Vector3 position) {
		
		GameObject laser = ObjectPooler.instance.GetPooledObject (GameConstants.PooledObject.PLAYER_LASER);
		laser.transform.position = position;
		laser.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, GameConstants.PLAYER_LASER_SPEED, 0);
	}

	void OnTriggerEnter2D (Collider2D collider) {
	
		if (collider.gameObject.TryGetComponent (out Laser laser)) {

			if (laser.type == GameConstants.LaserType.ENEMY && !laser.used) {

				laser.used = true;
				collider.gameObject.SetActive (false);
				GotHit ();
			}
		}
	}

	void GotHit () {

		health -= 10; // 10f is laser power;
		uiController.UpdateHealthBar (health);
		GameObject explosion = ObjectPooler.instance.GetPooledObject (GameConstants.PooledObject.EXPLOSION);
		if (health <= 0) {

			explosion.GetComponent<Explosion> ().CreateExplosion (gameObject, true);
			CancelInvoke (); //In case laser is on
			audioController.PlaySound (audioController.crash, transform.position);
			GetComponent<SpriteRenderer> ().enabled = false;
			gameController.ChangeGameState (GameConstants.GameStates.ENDED);
		}
		else {
			explosion.GetComponent<Explosion> ().CreateExplosion (gameObject, false);
		}
		if (GameState.currentLevel > 1) {
			GameState.currentLevel--;
		}
	}

	void PlayerEntered () {

		hasEntered = true;
	}

	public void DoublePoints () {

		audioController.PlaySound (audioController.powerUp, transform.position);
		bonusTime = Time.time + GameConstants.BONUS_POINTS_DURATION; //10 seconds of bonus points;
	}

	public void Heal () {

		audioController.PlaySound (audioController.lifeGained, transform.position);
		if (health < GameConstants.MAX_PLAYER_HEALTH) {
			health += 10;
		}
		uiController.UpdateHealthBar (health);
	}

	public void IncreaseLevel () {

		audioController.PlaySound (audioController.powerUp, transform.position);
		if (GameState.currentLevel < GameConstants.MAX_LASER_LEVEL) {
			GameState.currentLevel++;
		}
	}
}