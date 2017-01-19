using UnityEngine;
using System.Collections;

public class BombsController : MonoBehaviour {

	public float explosionForce=100f;
	public float radius=10f;

    public GameObject ExplosionParticle;
    public AudioSource bomb;

    public AudioSource Bum;

	private Rigidbody rb;
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private GameObject go;

    private bool componetsEnabled = false;


    // Use this for initialization
    void Start () {
		rb = this.GetComponent<Rigidbody> ();

        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");

        float sonido = 1 - Vector3.Distance(this.transform.position, cam.transform.position) / 100;
        bomb.volume = sonido < 0 ? 0 : sonido/2;
        bomb.Play();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c){
        //if someone collides the mine must explode and sink the gameobject
		if (c.tag == "Player" || c.tag == "Enemy") {
			go = c.gameObject;

            float sonido = 1-Vector3.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("MainCamera").transform.position) / 100;
            Bum.volume = sonido < 0 ? 0 : sonido;
            Bum.Play();
            Animate();

		}
	}

   //reproduces the sinking and the particle effects
    void Animate() {
        lastPosition = go.transform.position;
        lastRotation = go.transform.rotation;
        Explosion();
        setComponents();
    }

    // changes the collider´s gameobject variables so it can respawn or be deactivated properly
    void setComponents() {
        go.GetComponent<BoxCollider>().enabled = componetsEnabled;
        go.GetComponent<Rigidbody>().detectCollisions = componetsEnabled;
        if (go.tag == "Enemy")
        {
            NavMeshAgent currnav = go.GetComponent<IAFollowPivot>().targetTransform.gameObject.GetComponent<NavMeshAgent>();
            currnav.enabled = componetsEnabled;
        }

        else if (go.tag == "Player")
        {
            go.GetComponent<ForceMovement>().enabled = componetsEnabled;
            go.GetComponent<Shooting>().enabled = componetsEnabled;
        }
        go.GetComponent<FlotabilityController>().enabled = componetsEnabled;
        componetsEnabled = !componetsEnabled;


    }

	void Explosion(){
        Instantiate(ExplosionParticle, this.transform.position, this.transform.rotation);
		Invoke("Respawn",2f);
        this.GetComponentInChildren<MeshRenderer>().enabled = false;
        this.GetComponent<SphereCollider>().enabled = false;
		Destroy (this.gameObject, 2.1f);

	}

	void Respawn(){
		if (go.tag == "Enemy") {
			go.GetComponent<IAFollowPivot> ().targetTransform.position = new Vector3 (lastPosition.x,
				go.GetComponent<IAFollowPivot> ().targetTransform.position.y, lastPosition.z);
		}

        go.transform.position = new Vector3(lastPosition.x, 2f, lastPosition.z) ;
        go.transform.rotation = new Quaternion(0,lastRotation.y,0,lastRotation.w);

        setComponents();

	}
}
