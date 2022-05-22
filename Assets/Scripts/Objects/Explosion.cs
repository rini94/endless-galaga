using UnityEngine;

public class Explosion : MonoBehaviour {

    private GameObject explodingObj;

    public void DeactivateGO () {

        GameObject poolParent = ObjectPooler.instance.GetParent (GameConstants.PooledObject.EXPLOSION);
        transform.SetParent (poolParent.transform);
        if (GameState.currentState == GameConstants.GameStates.ENDED) {

            explodingObj.SetActive (false); //disable player
            GameController gameController = FindObjectOfType<GameController> ();
            gameController.GameOver ();
        }
        gameObject.SetActive (false);
    }

    public void CreateExplosion (GameObject explodingObj, bool isDestroyed) {

        this.explodingObj = explodingObj;
        transform.position = explodingObj.transform.position;
        if (!isDestroyed) {
            transform.SetParent (explodingObj.transform);
        }
    }
}
