using UnityEngine;

public class Laser : MonoBehaviour {

	public bool used;
	public GameConstants.LaserType type;

	void OnEnable () {
		used = false;
	}
}
