using UnityEngine;
using System.Collections;

public class generateNewWP : MonoBehaviour {

    private GameObject gameController;
    public AudioSource Bells;
    public GameObject ScoreCanvas;
    public GameObject Light;

    void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    void OnTriggerEnter(Collider other)
    {
        // if a player or an enemy collides it must modify the score and the controller must create a new buoy.
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            other.gameObject.GetComponent<ScoreManager>().addScore();
            gameController.GetComponent<GameController>().generateNewWP();
            float sonido = 1 - Vector3.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("MainCamera").transform.position) / 100;
            Bells.volume = sonido < 0 ? 0 : sonido;
            Bells.Play();

            animateBuoy();

            createPlusOne(other);


           
        }
    }


     void animateBuoy() {

        this.gameObject.GetComponent<FlotabilityController>().enabled = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        this.gameObject.GetComponent<SphereCollider>().enabled = false;

        Light.GetComponent<Renderer>().material.SetFloat("_EmissionScaleUI",0);

        Invoke("destroythis", 2f);



    }
    //creates the banner that show up on the one who got the point.
    void createPlusOne(Collider other) {
        ScoreCanvas = Instantiate(ScoreCanvas, new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 1, other.gameObject.transform.position.z), new Quaternion(0, 0, 0, 0)) as GameObject;
        ScoreCanvas.transform.SetParent(other.gameObject.transform);
    }

    void destroythis()
    {
        Destroy(this.gameObject.GetComponent<FlotabilityController>().twinHoverParent);
        foreach (GameObject twin in this.gameObject.GetComponent<FlotabilityController>().m_twinHoverPoints)
        {
            Destroy(twin);
        }
        Destroy(this.gameObject);
    }
}
