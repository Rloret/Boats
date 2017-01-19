using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {

	public GameObject bullet;

	public float bulletForce=35f;
	public float timeBetweenBullets=2f;
	private float lastTime;

    public GameObject dropBombPosition;

    void Start () {
		lastTime = Mathf.Infinity;
	}
    void Update()
    {
        if (Input.GetKeyDown("space") && Mathf.Abs(Time.time - lastTime) >= timeBetweenBullets)
        {
            Shoot();
        }
    }
    void Shoot(){
		lastTime = Time.time;
		GameObject bulletInstance = Instantiate (bullet,dropBombPosition.transform.position,new Quaternion(0,0,0,0)) as GameObject;

	}

}
