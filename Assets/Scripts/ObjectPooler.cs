using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public  class ObjectPoolItem {

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
		} else {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}

	void Start () {

		createPool ();
	}

	void createPool () {

		foreach(ObjectPoolItem item in itemsToPool) {
			for(int i = 0; i < item.poolSize; i++) {
				GameObject objectInstance = Instantiate (item.objectToPool, transform.position, Quaternion.identity) as GameObject;
				objectInstance.SetActive(false);
				objectInstance.transform.SetParent(item.parentObject.transform);
			}
		}
	}

	public GameObject getPooledObject (string tag) {
		 
		foreach (ObjectPoolItem item in itemsToPool) {
			if (item.objectToPool.tag == tag) {
				foreach (Transform child in item.parentObject.transform) {
					if (child.tag == tag && child.gameObject.activeInHierarchy == false) {
						child.gameObject.SetActive (true);
						return child.gameObject;
					}
				}
				if (item.shouldExpand == true) {
					GameObject objectInstance = Instantiate (item.objectToPool, transform.position, Quaternion.identity) as GameObject;
					objectInstance.transform.SetParent (item.parentObject.transform);
					return objectInstance;
				}
			}
		}
		return null;
	}

	public void deactivateAll () {

		foreach (ObjectPoolItem item in itemsToPool) {
			foreach (Transform child in item.parentObject.transform) {
				if (child.gameObject.activeInHierarchy == true) {
					child.gameObject.SetActive (false);
				}
			}
		}
	}
}