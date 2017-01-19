using UnityEngine;
using System.Collections;

public class ForceMovement : MonoBehaviour {

    public GameObject[] mountainAvoiders;    //raycast source points- terrain collision

    public float forwardAcceleration = 2500f;
    public float backwardsAcceleration = 1500;
    public float currentImpulse = 0.0f;

    public float hoverForce = 9.0f;
    public float distanceTerrain = 4.5f;
    public float turnStrength = 1300f;

    private int Mask;
   
    private float currentTurn = 0.0f;
    private float noKeyPressed = 0.1f;
    private Rigidbody boatRB;

    void Start () {
        boatRB = this.GetComponent<Rigidbody>();
        Vector3 masscenter = GetComponent<Rigidbody>().worldCenterOfMass;
        float rayOffsetZ = GetComponent<BoxCollider>().bounds.extents.z;

        //gets the mask to which you can collide and bitwise translates it 
        Mask = 1 << LayerMask.NameToLayer("Characters");
        // inverts the mask so that it doesnt collide with the characters but it does with everyuthing else
        Mask = ~Mask;

    }

    void Update()
    {
        float aclAxis = Input.GetAxis("Vertical");

        // Main Impulse
        currentImpulse = 0.0f;

        if (aclAxis > noKeyPressed) currentImpulse = aclAxis * forwardAcceleration;
        else if (aclAxis < -noKeyPressed) currentImpulse = aclAxis * backwardsAcceleration;

        // Turning
        currentTurn = 0.0f;
        float turnAxis = Input.GetAxis("Horizontal");
        if (Mathf.Abs(turnAxis) > noKeyPressed) currentTurn = turnAxis;
        //Avoid flipping
        if (this.transform.rotation.ToEuler().z >= 3 && this.transform.rotation.ToEuler().z < 3.5f)
        {
            this.transform.rotation = new Quaternion(this.transform.rotation.x, this.transform.rotation.y, -0, 1);

        }

    }
    void FixedUpdate() {
        //moves the boat.
        RaycastHit hit;

        if (Mathf.Abs(currentImpulse) > 0)
        {
            boatRB.AddForce(transform.forward * currentImpulse);
        }

        // Turn
        if (currentTurn > 0)
        {
            boatRB.AddRelativeTorque(Vector3.up * currentTurn * turnStrength);
        }
        else if (currentTurn < 0)
        {
            boatRB.AddRelativeTorque(Vector3.up * currentTurn * turnStrength);

        }

        //Avoid the boat from climbing through the mountains.
        for (int j = 0; j < mountainAvoiders.Length; j++)
        {
            var rayPoint = mountainAvoiders[j];
            if (Physics.Raycast(rayPoint.transform.position, rayPoint.transform.forward, out hit, distanceTerrain, Mask))
            {
                boatRB.AddForceAtPosition(-rayPoint.transform.forward * hoverForce*500 * (1.0f - (hit.distance / distanceTerrain)),
                    rayPoint.transform.position);
            }

        }
    }
}
