using UnityEngine;
using System.Collections;

public class rePositionHover : MonoBehaviour {
    private Vector3 myPosIni;
    private Vector3 myPos;
    private Quaternion myrot;
    private float dimensions = 32f;

    public GameObject sibling;

    float latd=0f;
    float lati = 0f;

    float latar =0f;
    float latab = 0f;
    // Use this for initialization
    void Start () {
        myPosIni = new Vector3(0+16, 2,-58+10);
        latd = myPosIni.x + dimensions / 2;
        lati = myPosIni.x - dimensions / 2;

        latar = myPosIni.z + dimensions / 2;
        latab = myPosIni.z - dimensions / 2;
       // Debug.Log(latd+" "+ lati+" "+ latar+" "+ latab);
    }
    void OnDrawGizmos() {
        Gizmos.DrawWireCube(myPosIni, new Vector3(dimensions, 0.1f, dimensions));
    }
	// Update is called once per frame

    public void checkPosInGrid() {
        myPos = gameObject.transform.position;
        Vector3 postimesdimensions = myPos / dimensions;
        //Debug.Log(postimesdimensions.ToString());
        float newposX = myPos.x > latd ? lati+ (myPos.x - (dimensions * (Mathf.FloorToInt(postimesdimensions.x)))) :
                          myPos.x < lati ? latd - dimensions - ((dimensions * (Mathf.FloorToInt(postimesdimensions.x))) - myPos.x) : myPos.x;

        float newposZ = myPos.z > latar ? latab + (myPos.z - dimensions * (Mathf.FloorToInt(postimesdimensions.z))) :
                        myPos.z < latab ? latar - dimensions - ((dimensions * (Mathf.FloorToInt(postimesdimensions.z))) - myPos.z):myPos.z;

        this.transform.position = new Vector3(newposX, sibling.transform.position.y, newposZ);
    }

    public void posFromParent(Vector3 offsetFromParent,Vector3 parent) {
       transform.position = parent + offsetFromParent;
    }
}
