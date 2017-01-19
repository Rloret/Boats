using UnityEngine;
using System.Collections;

public class SinusFloating : MonoBehaviour {
    float height = 0.5f;
    float speed = 2.0f;

    void FixedUpdate () {
        this.transform.position = new Vector3(this.transform.position.x,/*this.transform.position.y+*/2.5f+ Mathf.Sin(Time.time *speed)*height/2, this.transform.position.z);
	}
}
