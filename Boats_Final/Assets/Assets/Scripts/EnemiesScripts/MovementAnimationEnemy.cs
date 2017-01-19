using UnityEngine;
using System.Collections;

public class MovementAnimationEnemy : MonoBehaviour {

	public GameObject Rows;
	public GameObject[] GearZ;
	public GameObject GearX;
	public GameObject Wheel;
	public float AngularVel = 200f;
	public float maxWheelAngle = 45;

	void Update () {

		Rows.transform.Rotate (Vector3.forward * (-AngularVel * Time.deltaTime ));
		for (int i = 0; i < GearZ.Length; i++)
			GearZ [i].transform.Rotate (Vector3.forward * (AngularVel * Time.deltaTime ));
		GearX.transform.Rotate (Vector3.right * (-AngularVel * Time.deltaTime ));
	}
}
