using UnityEngine;

public class Asteroid : MonoBehaviour {

    void Update () {

        transform.position += 2f * Time.deltaTime * Vector3.down;
        transform.Rotate (0, 0, 7f * Time.deltaTime);
    }
}
