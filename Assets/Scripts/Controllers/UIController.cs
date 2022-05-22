using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

	//Assign in inspector
	public Image healthBar;
	public Text scoreText;
	public GameObject pausedScreen;
	public GameObject pauseButton;
	public GameObject endScreen;
	public GameObject instructionsScreen;
	public GameObject mainScreen;
	public GameObject hudScreen;
	public Text highScoreText;
	public Text finalScoreText;

	void Start () {

		if (highScoreText != null) {
			highScoreText.text = ScoreController.instance.GetHighestScore ().ToString ();
		}
	}

	public void ShowPauseScreen () {

		if (pausedScreen != null) {
			pausedScreen.SetActive (true);
		}
		if (pauseButton != null) {
			pauseButton.SetActive (false);
		}
	}

	public void HidePauseScreen () {

		if (pausedScreen != null) {
			pausedScreen.SetActive (false);
		}
		if (pauseButton != null) {
			pauseButton.SetActive (true);
		}
	}

	public void ShowInstructions () {

		if (mainScreen != null) {
			mainScreen.SetActive (false);
		}
		if (instructionsScreen != null) {
			instructionsScreen.SetActive (true);
		}
	}

	public void HideInstructions () {

		if (instructionsScreen != null) {
			instructionsScreen.SetActive (false);
		}
		if (mainScreen != null) {
			mainScreen.SetActive (true);
		}
	}

	public void ShowEndScreen () {

		if (hudScreen != null) {
			hudScreen.SetActive (false);
		}
		if (endScreen != null) {

			endScreen.SetActive (true);
			finalScoreText.text = ScoreController.instance.GetScore ().ToString ();
		}
	}

	public void UpdateHealthBar (int health) {

		healthBar.fillAmount = (float) health / GameConstants.MAX_PLAYER_HEALTH;
	}

	public void UpdateScore () {

		scoreText.text = ScoreController.instance.GetScore ().ToString ();
	}

	public void ShowPopup (Vector3 position, string message) {

		GameObject powerUpPop = ObjectPooler.instance.GetPooledObject (GameConstants.PooledObject.POWER_UP_POPUP);
		powerUpPop.transform.position = position;
		powerUpPop.GetComponent<Text> ().text = message;
		powerUpPop.transform.SetParent (GameObject.Find ("Canvas").transform);
		RectTransform rectTransform = powerUpPop.GetComponent<RectTransform> ();
		rectTransform.anchoredPosition = new Vector2 (0, 0);

		StartCoroutine (DestroyPopup (powerUpPop));
	}

	IEnumerator DestroyPopup (GameObject popup) {

		yield return new WaitForSeconds (popup.GetComponent<Animator> ().speed);
		GameObject parentObject = ObjectPooler.instance.GetParent (GameConstants.PooledObject.POWER_UP_POPUP);
		popup.transform.SetParent (parentObject.transform);
		popup.SetActive (false);
	}
}
