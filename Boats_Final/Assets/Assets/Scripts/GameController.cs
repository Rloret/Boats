using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// Overall explanation:
/// the game controller itself is in charge of instantiating each game´s components such as the player,the Ia and the HUD,among other things.
/// </summary>
public class GameController : MonoBehaviour {

    public GameObject Boat;
    public GameObject enemyBoat;
    public GameObject enemyPath;
    public GameObject tessendorfOcean;
    public GameObject UserInterface;
    public GameObject playerImage;
    public GameObject Buoy = null;

    public float StartDelay = 3f;
    public float EndDelay = 3f;

    public Text MessageText;


   

    private bool restartGame=false;

    private Vector3 Podium;

    public Sprite markerBuoy;
    public Sprite markerEnemy;


    public Transform[] wayPoints;
    public Transform[] Ia_Spawn;

    private GameObject[] BoatsInScene = new GameObject[5];
    private GameObject minimap;
    private GameObject BuoyAux = null;
    private GameObject GameWinner;

    private float MaxDistance = 10000000000f;
    private float timeAtFinish;

    private List<GameObject> Boats = new List<GameObject>();
    private List<GameObject> BoatPaths = new List<GameObject>();

    private int currentWP;

	private WaitForSeconds StartWait;         
         
    void Start () {
		StartWait = new WaitForSeconds (StartDelay);

		InitializeComponents ();
		StartCoroutine (RoundStarting ());
        
	}

	void InitializeComponents(){
		createBuoy();
		createOcean();
		createBoat();
	}
    // Begins the countdown to start the round
	private IEnumerator RoundStarting(){
	
		yield return StartCoroutine (CountDown3());
		yield return StartCoroutine (CountDown2());
		yield return StartCoroutine (CountDown1());
		yield return StartCoroutine (StartTheRace());
		yield return StartCoroutine (EmptyText());
		yield return StartWait;
	}


	private IEnumerator CountDown3(){
		MessageText.text = "Ready";
		yield return new WaitForSeconds(1f);
	}
	private IEnumerator CountDown2(){
        MessageText.text = "Steady";

		yield return new WaitForSeconds(1f);
	}
	private IEnumerator CountDown1(){
		MessageText.text = "GO";
		yield return new WaitForSeconds(1f);
	}
	private IEnumerator StartTheRace(){
		createIA();
		createUI();
		Boat.GetComponent<ForceMovement> ().enabled = true;
		Boat.GetComponentInChildren<MovementAnimation> ().enabled = true;
		yield return new WaitForSeconds(1f);
	}
	private IEnumerator EmptyText(){
		MessageText.text = " ";
		yield return new WaitForSeconds(1f);
	}


	void Update () {

        if (Input.GetKey("escape")) { 
            Application.Quit();
        }
        //recollocates the winner in a position and shows the model to the player.
        if (restartGame)
        {
            GameWinner.transform.Rotate(Vector3.up * Time.deltaTime*50);
            if (Time.time - timeAtFinish >= 10)
            {
                restartLevel();
            }

        }
	
	}
    //instantiates tessendorfs Ocean
    void createOcean() {
        tessendorfOcean = Instantiate(tessendorfOcean, new Vector3(0,0,0), new Quaternion(0, 0, 0, 0)) as GameObject;
        tessendorfOcean.transform.parent = this.transform;
    }

    void createBoat() {
        Vector3 originPos = new Vector3(0 + 16, 2f, -58);
        Podium = originPos;
        Boat = Instantiate(Boat, originPos, new Quaternion(0, 0, 0, 0)) as GameObject;
        Boat.transform.parent = this.transform;
        Boat.GetComponent<ControlProximity>().nearColor = new Color(69 / 255.0f, 247 / 255f, 1, 1);
        Boat.GetComponent<ControlProximity>().configureDistances(BuoyAux.transform);
        BoatsInScene[0] = Boat;

        Boat.name = "Player";
		Boat.GetComponent<ForceMovement> ().enabled = false;
		Boat.GetComponentInChildren<MovementAnimation> ().enabled = false;

    }

    void createIA() {
        GameObject iaboat;
        GameObject pivot;

        for (int i = 0; i <Ia_Spawn.Length; i++) {
           //creates the GO that will have the pathfinding and behaviour
            pivot = Instantiate(enemyPath, new Vector3(Ia_Spawn[i].position.x, Ia_Spawn[i].position.y - 4.9f, Ia_Spawn[i].position.z), new Quaternion(0, 0, 0, 0)) as GameObject;
            //should be evaluateSituation
            pivot.name = "pivot_" + i;
            pivot.GetComponent<IAMovement>().setTarget(BuoyAux);
            pivot.GetComponent<NavMeshAgent>().speed += Random.Range(-1, 1);

            //creates the visual representation of the pivot and configures the minimap and the scripts
            iaboat = Instantiate(enemyBoat, new Vector3(Ia_Spawn[i].position.x, Ia_Spawn[i].position.y + 2, Ia_Spawn[i].position.z), new Quaternion(0, 0, 0, 0)) as GameObject;
            iaboat.active = true;
            //sets the reference so that it can manipulate;
            pivot.GetComponent<IAMovement>().boatReference = iaboat;

            iaboat.AddComponent<MapMarker>();
            iaboat.GetComponent<MapMarker>().markerSprite = markerEnemy;
            iaboat.GetComponent<MapMarker>().markerSize = 10;

            iaboat.GetComponent<IAFollowPivot>().targetTransform = pivot.transform;

            BoatsInScene[i + 1] = iaboat;


            BoatPaths.Add(pivot);
            Boats.Add(iaboat);

        }
        //assingns each IA color.
        Boats[0].name = "DarkBlue";
        Boats[0].GetComponent<ControlProximity>().nearColor =Color.blue;
        Boats[0].GetComponent<ControlProximity>().configureDistances(BuoyAux.transform);

        Boats[1].name = "Yellow";
        Boats[1].GetComponent<ControlProximity>().nearColor = Color.yellow;
        Boats[1].GetComponent<ControlProximity>().configureDistances(BuoyAux.transform);

        Boats[2].name = "Magenta";
        Boats[2].GetComponent<ControlProximity>().nearColor = Color.magenta;
        Boats[2].GetComponent<ControlProximity>().configureDistances(BuoyAux.transform);

        Boats[3].name = "Green";
        Boats[3].GetComponent<ControlProximity>().nearColor = Color.green;
        Boats[3].GetComponent<ControlProximity>().configureDistances(BuoyAux.transform);

    }

    void createUI() {
        UserInterface = Instantiate(UserInterface, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
        minimap = GameObject.FindGameObjectWithTag("Minimap");
        minimap.GetComponent<MapCanvasController>().playerTransform = Boat.transform;
        UserInterface.transform.SetParent(this.transform);
        int i = 0;
        foreach( Image im in UserInterface.GetComponent<UIManager>().images){
            //assigns each UI the color of the player it represents.
            im.color = BoatsInScene[i].GetComponent<ControlProximity>().nearColor;
            i++;
        }

        BuoyAux.AddComponent<MapMarker>();
        BuoyAux.GetComponent<MapMarker>().markerSprite = markerBuoy;

    }
    public void generateNewWP()
    {
        //create a new WP and make sure its not the same as the previous one
        int newCurrentWP = Random.Range(0, wayPoints.Length);
        while (newCurrentWP == currentWP) {
            newCurrentWP = Random.Range(0, wayPoints.Length);
        }
        currentWP = newCurrentWP;

        //set the destination nav for all the boats and the target for our boat afterwards
        GameObject target = wayPoints[currentWP].gameObject;

        //create a the new Buoy on the location
        BuoyAux = Instantiate(Buoy, target.transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;

        GameObject closestBoat = null;

        //decide which will be the one to contest the Buoy
        foreach (GameObject boatpath in BoatPaths)
        { 
            boatpath.GetComponent<IAMovement>().setTarget(BuoyAux);
            float dist = Vector3.Distance(boatpath.transform.position, BuoyAux.transform.position);
            
            if (dist < MaxDistance)
            {
                closestBoat = boatpath;
                MaxDistance = dist;
            }
            
        }

        //Asigns new Colors to all the boats in the scene depending on the distance to their objectives
        foreach (GameObject boat in Boats)
        {
            boat.GetComponent<ControlProximity>().configureDistances(BuoyAux.transform);
        }
        Boat.GetComponent<ControlProximity>().configureDistances(BuoyAux.transform);

        //Assigns new destinations to the contestant and to the tricksters
        if (closestBoat != null) 
        {
            foreach (GameObject boatpath in BoatPaths)
            {
                if (boatpath == closestBoat)
                {
                    boatpath.GetComponent<IAMovement>().setTarget(BuoyAux);
                    boatpath.GetComponent<IAMovement>().dropBomb = false;

                }
                else
                {
                    boatpath.GetComponent<IAMovement>().setTarget(determineBombArea());
                    boatpath.GetComponent<IAMovement>().dropBomb = true;
                }

            }
        }
        MaxDistance = 100000000;
        
       
    }

    void createBuoy() { // self-explanatory: creates the first Buoy;
        currentWP = Random.Range(0, wayPoints.Length);
        BuoyAux = Instantiate(Buoy, wayPoints[currentWP].gameObject.transform.position, Quaternion.Euler(-90,0,0)) as GameObject;
    }

    public void calculateScores() {
        int i = 0;
        foreach (Text tx in UserInterface.GetComponent<UIManager>().score)
        {
            tx.text ="X"+ BoatsInScene[i].GetComponent<ScoreManager>().Score;
            i++;
        }
    }

    //Calculates a new destination for the tricksters
    public GameObject determineBombArea()
    {
        int randomBuoy = Random.Range(0, wayPoints.Length);
        while (randomBuoy == currentWP) {
            randomBuoy = Random.Range(0, wayPoints.Length);
        }
        return wayPoints[randomBuoy].gameObject;
    }


	public void FinishGame(GameObject winner){
        GameWinner = winner;

		MessageText.text = "THE WINNER IS " + winner.name;
        GameObject.FindGameObjectWithTag("MultiPurposeCamera").GetComponent<UnityStandardAssets.Cameras.AutoCam>().Target = winner.transform;
        foreach (GameObject boat in Boats)
        {
            boat.GetComponent<IAFollowPivot>().enabled = false;
        }
        winner.transform.position = Podium;
        winner.GetComponent<ControlProximity>().configureDistances(winner.transform);

        foreach(GameObject mina in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Destroy(mina);
        }

        Boat.GetComponent<ForceMovement>().enabled = false;
        Boat.GetComponent<Shooting>().enabled = false;
        restartGame = true;
        timeAtFinish = Time.time;

    }
    public void restartLevel() {
        SceneManager.LoadScene ("NivelPrincipal");
        restartGame = false;
    }
}

