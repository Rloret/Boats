using UnityEngine;
using System.Collections;

public class Float : MonoBehaviour {

	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(Vector3.up * 1.5f * Time.deltaTime);
	}
}
