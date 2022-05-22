using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem {

	public GameConstants.PooledObject pooledObject;
	public GameObject objectToPool;
	public int poolSize;
	public GameObject parentObject;
	public bool shouldExpand;
}

public class ObjectPooler : MonoBehaviour {

	public static ObjectPooler instance = null;
	public List<ObjectPoolItem> itemsToPool;

	void Awake () {

		if (instance == null) {
			instance = this;
		}
		else {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}

	void Start () {

		CreatePool ();
	}

	public GameObject GetParent (GameConstants.PooledObject pooledObject) {

		foreach (ObjectPoolItem item in itemsToPool) {

			if (item.pooledObject == pooledObject) {
				return item.parentObject;
			}
		}
		return null;
	}

	void CreatePool () {

		foreach (ObjectPoolItem item in itemsToPool) {

			for (int i = 0; i < item.poolSize; i++) {

				GameObject objectInstance = Instantiate (item.objectToPool, transform.position, Quaternion.identity);
				objectInstance.SetActive (false);
				objectInstance.transform.SetParent (item.parentObject.transform);
			}
		}
	}

	public GameObject GetPooledObject (GameConstants.PooledObject pooledObject) {
		 
		foreach (ObjectPoolItem item in itemsToPool) {

			if (item.pooledObject == pooledObject) {

				foreach (Transform child in item.parentObject.transform) {

					if (!child.gameObject.activeInHierarchy) {

						child.gameObject.SetActive (true);
						return child.gameObject;
					}
				}
				if (item.shouldExpand) {

					GameObject objectInstance = Instantiate (item.objectToPool, transform.position, Quaternion.identity);
					objectInstance.transform.SetParent (item.parentObject.transform);
					return objectInstance;
				}
			}
		}
		return null;
	}

	public void DeactivateAll () {

		foreach (ObjectPoolItem item in itemsToPool) {

			foreach (Transform child in item.parentObject.transform) {

				if (child.gameObject.activeInHierarchy) {
					child.gameObject.SetActive (false);
				}
			}
		}
	}
}