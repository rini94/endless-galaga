using UnityEngine;

public class GameEdgeCollider : MonoBehaviour {
	
	void OnTriggerEnter2D (Collider2D collider) {

		if (collider.gameObject.GetComponent<Laser> () != null || collider.gameObject.GetComponent<Asteroid> () != null || collider.gameObject.GetComponent<PowerUp> () != null) {
			//return falling objects to pool when they reach the bottom of the screen
			collider.gameObject.SetActive (false);
		}
	}
}