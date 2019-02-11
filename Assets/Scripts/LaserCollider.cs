using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCollider : MonoBehaviour {
	
	void OnTriggerEnter2D (Collider2D collider) {

		if (collider.tag == "PlayerLaser" || collider.tag == "EnemyLaser" || collider.tag == "Asteroid" || collider.gameObject.GetComponent<PowerUps> () != null) {
		
			collider.gameObject.SetActive (false);
		}
	}
}