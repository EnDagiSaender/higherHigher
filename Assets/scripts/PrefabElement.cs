using System;
using System.Collections;
using UnityEngine;
using TMPro;

[Serializable]
public class PrefabElement {
	public GameObject prefab;
	public Vector3 position;


	public PrefabElement(GameObject prefab, Vector3 position) {
		this.prefab = prefab;
		this.position = position;

	}

}