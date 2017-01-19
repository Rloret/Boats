using UnityEngine;
using System.Collections;

public class Shine : MonoBehaviour {

	
	
	// Update is called once per frame
	void Update () {
        this.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(Mathf.PingPong(Time.time/2, 1),0,0));
    }
}
