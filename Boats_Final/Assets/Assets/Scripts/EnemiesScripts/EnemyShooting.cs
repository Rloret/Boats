using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour {

    public GameObject bullet;

    public float bulletForce = 35f;
    public float timeBetweenBullets = 10f;

    private bool canshoot = false;

    public void Shoot() {

        GameObject bulletInstance = Instantiate(bullet, transform.position - new Vector3(GetComponent<BoxCollider>().bounds.extents.x * 3f, GetComponent<BoxCollider>().bounds.extents.y * 2f, GetComponent<BoxCollider>().bounds.extents.z * 3f), new Quaternion(0, 0, 0, 0)) as GameObject;

    }
}
