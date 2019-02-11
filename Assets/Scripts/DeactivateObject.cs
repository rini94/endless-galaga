using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateObject : MonoBehaviour {

	void DestroyGo () {

		gameObject.SetActive (false);
		if (gameObject.tag == "PowerUpPopup") {
			gameObject.transform.SetParent (GameObject.Find ("PowerUps").transform);
		}
	}
}
