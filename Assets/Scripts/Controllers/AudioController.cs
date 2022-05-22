using UnityEngine;

public class AudioController : MonoBehaviour {

    //Assign in inspector
    public AudioClip playerLaser;
    public AudioClip enemyLaser;
    public AudioClip crash;
    public AudioClip powerUp;
    public AudioClip lifeGained;
    AudioSource audioSource;

    private void Start () {

        audioSource = GetComponent<AudioSource> ();
        CheckPlayBGM ();
	}

	public void PlaySound (AudioClip clip, Vector3 position) {

        AudioSource.PlayClipAtPoint (clip, position);
    }

    public void CheckPlayBGM () {

        if (audioSource == null) {
            audioSource = GetComponent<AudioSource> ();
        }
        if (GameState.currentState != GameConstants.GameStates.PLAYING) {
            audioSource.Play ();
        }
        else {
            audioSource.Stop ();
        }
    }
}
