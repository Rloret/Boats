using UnityEngine;
using System.Collections;

public class MovementAnimation : MonoBehaviour {

	public GameObject Rows;
	public GameObject[] GearZ;
	public GameObject GearX;
	public GameObject Wheel;
    public float AngularVel = 200f;
    public float maxWheelAngle = 45;

    private float v;

    void Start () {
		
    }
	void Update () {

        v = Input.GetAxis("Vertical");
        Rows.transform.Rotate(Vector3.forward * (AngularVel * Time.deltaTime * 2*-v));
        for(int i=0; i<GearZ.Length;i++)GearZ[i].transform.Rotate(Vector3.forward * (AngularVel * Time.deltaTime*2 * v));
        GearX.transform.Rotate(Vector3.right * (AngularVel * Time.deltaTime * 2*-v));

        
    }
}
