using UnityEngine;
using System.Collections;

public class IAFollowPivot : MonoBehaviour {

	[HideInInspector]public Transform targetTransform;
	void Start () {
	
	}
    void Update() {
    }
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 newPosition = new Vector3(targetTransform.position.x, this.transform.position.y, targetTransform.position.z);
        transform.position = newPosition;
        transform.rotation = new Quaternion(this.transform.rotation.x, targetTransform.rotation.y, this.transform.rotation.z, targetTransform.rotation.w);
    }
}
