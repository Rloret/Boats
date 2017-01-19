using UnityEngine;
using System.Collections;

public class IAMovement : MonoBehaviour {

    private GameObject Target;

    [HideInInspector]
    public bool dropBomb = false;

	public Transform[] checkPointsList;
    public GameObject boatReference;

	NavMeshAgent nav;

	// Use this for initialization
	void Start () {
		nav = GetComponent<NavMeshAgent> ();

	}
	
	// Update is called once per frame
	void Update () {
        if (Target != null && nav.enabled)
        {
            nav.SetDestination(Target.transform.position);

            if (dropBomb)
            {
                if (calculateDistances())
                {
                    //dropBomb en nav.path.corners.Length*0.8f
                    boatReference.GetComponent<EnemyShooting>().Shoot();
                    Target = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().determineBombArea();
                }

            }
        }

    
    }

    public void setTarget(GameObject targ) {
        Target = targ;
    }

    private bool calculateDistances() {
        if (Vector3.Distance(this.transform.position, Target.transform.position) < 15) {
            return true;
        }
        return false;            
    }
}
