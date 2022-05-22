using UnityEngine;

public class PowerUp : MonoBehaviour {

	public GameConstants.PowerUpType powerUpType;

	private UIController uiController;
	private bool used;

	void OnEnable () {

		uiController = FindObjectOfType<UIController> ();
		used = false;
	}

	void Update () {
	
		transform.position += 2f * Time.deltaTime * Vector3.down;
	}

	void OnTriggerEnter2D (Collider2D collider) {

		if (used) {
			return;
		}
		if (collider.TryGetComponent (out Player player)) {

			used = true;
			string message = null;
			if (powerUpType == GameConstants.PowerUpType.HEALTH) {
				
				player.Heal ();
				message = GameConstants.MESSAGE_HEALTH_UP;
			}
			else if (powerUpType == GameConstants.PowerUpType.POWER_UP) {

				player.IncreaseLevel ();
				message = GameConstants.MESSAGE_LASER_POWER_UP;
			}
			else if (powerUpType == GameConstants.PowerUpType.BONUS) {

				player.DoublePoints ();
				message = GameConstants.MESSAGE_POINTS_UP;
			}
			if (message != null) {

				uiController.ShowPopup (transform.position, message);
				gameObject.SetActive (false);
			}
		}
	}
}