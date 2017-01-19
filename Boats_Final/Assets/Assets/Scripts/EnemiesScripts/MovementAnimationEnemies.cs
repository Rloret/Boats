using UnityEngine;
using System.Collections;

public class MovementAnimationEnemies : MonoBehaviour {

	private GameObject Rows;
	private GameObject[] GearZ;
	private GameObject GearX;
	private GameObject Wheel;
	public float AngularVel = 200f;
	public float maxWheelAngle = 45;
	private Quaternion inirot;
	private float x_ant;
	private float z_ant;

	void Start () {
		Rows = GameObject.FindGameObjectWithTag("PaddleRow");
		GearZ= GameObject.FindGameObjectsWithTag("GearZ");
		GearX= GameObject.FindGameObjectWithTag("GearX");
		Wheel= GameObject.FindGameObjectWithTag("Wheel");
		inirot = Wheel.transform.rotation;
		x_ant = transform.position.x;
		z_ant = transform.position.z;
	}

	// Update is called once per frame
	void Update () {
		if(x_ant!=transform.position.x || z_ant != transform.position.z){

			Rows.transform.Rotate(Vector3.forward * (AngularVel * Time.deltaTime * -2));
			for(int i=0; i<GearZ.Length;i++)GearZ[i].transform.Rotate(Vector3.forward * (AngularVel * Time.deltaTime*2));
			GearX.transform.Rotate(Vector3.right * (AngularVel * Time.deltaTime * -2));

		}

	}

}