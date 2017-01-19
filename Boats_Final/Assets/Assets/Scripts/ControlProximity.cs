using UnityEngine;
using System.Collections;

public class ControlProximity : MonoBehaviour {
    public Transform BuoyWp;
    public Light luz;
    public GameObject Compass;
    private float totalDistance;
    private float currDistance;
    public Color farColor;
    public Color nearColor;
    private Color currentColor;
    public Color newColor;

	private Vector3 originPosition;
    void Start () {
		originPosition = transform.position;
    }

    public void configureDistances(Transform waypoint) {
        BuoyWp = waypoint;
        totalDistance = Vector3.Distance(this.transform.position, BuoyWp.position);
        currentColor  = Color.Lerp(farColor, nearColor, 0.25f);
      
    }
	// Update is called once per frame
	void Update () {

		currDistance = Vector3.Distance(BuoyWp.position, this.transform.position);
		float percent = currDistance / totalDistance;
        if (percent <= 1)
        {
            newColor = Color.Lerp(currentColor, nearColor, 1-percent);

        }
        else
        {
            percent = totalDistance / currDistance;
            newColor = Color.Lerp(currentColor, farColor,1- percent);
        }
        Compass.GetComponent<Renderer>().material.SetColor("_EmissionColor", newColor);


        Quaternion targetrotation = Quaternion.LookRotation(BuoyWp.position - Compass.transform.position);
        Compass.transform.rotation = Quaternion.Slerp(Compass.transform.rotation, new Quaternion(targetrotation.x,targetrotation.y,Compass.transform.rotation.z,targetrotation.w), 50 * Time.deltaTime);
        Compass.transform.LookAt(BuoyWp.transform);
        luz.color = newColor;

    }
}
