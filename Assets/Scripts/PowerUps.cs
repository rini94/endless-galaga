using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUps : MonoBehaviour {

	public AudioClip lifeGained;
	public AudioClip powerUp;

	Vector3 playerPos;

	void Update () {
	
		transform.position += Vector3.down * 2f * Time.deltaTime;
		if (gameObject.tag == "Asteroid") {
			transform.Rotate (0, 0, 7f * Time.deltaTime);
		}
	}

	void OnTriggerEnter2D (Collider2D collider) {

		Player player = GameObject.FindObjectOfType<Player> ();
		if (gameObject.tag != "Asteroid" && collider.tag == "Player") {
			
			GameObject powerUpPop = ObjectPooler.instance.getPooledObject ("PowerUpPopup");
			powerUpPop.transform.position = transform.position;
			if (gameObject.tag == "Life") {
				
				AudioSource.PlayClipAtPoint (lifeGained, transform.position);
				if (player.health < 30f) {
					player.health += 10f;
				}
				player.healthBar.fillAmount = player.health / player.maxHealth;
				powerUpPop.GetComponent<Text> ().text = "Health +1";

			} else if (gameObject.tag == "PowerUp") {
				
				AudioSource.PlayClipAtPoint (powerUp, transform.position);
				if (GameState.currentLevel < GameState.maxLaserLevel) {
					GameState.currentLevel++;
				}
				powerUpPop.GetComponent<Text> ().text = "Laser Level Up!";

			} else if (gameObject.tag == "Bonus") {
				
				AudioSource.PlayClipAtPoint (powerUp, transform.position);
				player.doublePoints ();
				powerUpPop.GetComponent<Text> ().text = "Points x2";
			}
			powerUpPop.transform.SetParent (GameObject.Find ("Canvas").transform);
			RectTransform rectTransform = powerUpPop.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = new Vector2 (0, 0);
			gameObject.SetActive (false);
		}
	}
}