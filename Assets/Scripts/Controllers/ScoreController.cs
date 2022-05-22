using UnityEngine;

public class ScoreController : MonoBehaviour {

    public static ScoreController instance = null;

    void Awake () {

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy (gameObject);
        }
        DontDestroyOnLoad (gameObject);
    }

    public int GetScore () {

        return GameState.score;
    }

    public void AddScore (int scoreToAdd) {

        GameState.score += scoreToAdd;
    }

    public int GetHighestScore () {

        if (GameState.highestScore == -1) { //not loaded from PlayerPrefs yet

            if (PlayerPrefs.HasKey (GameConstants.HIGHEST_SCORE)) {
                GameState.highestScore = PlayerPrefs.GetInt (GameConstants.HIGHEST_SCORE);
            }
            else {
                GameState.highestScore = 0;
            }
        }
        return GameState.highestScore;
    }

    public void UpdateHighestScore () {

        if (GetHighestScore () < GetScore ()) {

            GameState.highestScore = GetScore ();
            PlayerPrefs.SetInt (GameConstants.HIGHEST_SCORE, GameState.highestScore);
            PlayerPrefs.Save ();
        }
    }
}
