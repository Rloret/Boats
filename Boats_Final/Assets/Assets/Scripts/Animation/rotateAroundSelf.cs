using UnityEngine;
using System.Collections;

public class rotateAroundSelf : MonoBehaviour {

	void Update () {
        this.transform.Rotate(Vector3.forward);
	}
}
