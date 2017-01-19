using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
    public Canvas start;

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 2000f))
            {
                if (hit.collider.gameObject.name == "StartButton") { beginGame(); }
            }
        }
    }
	void OnTriggerEnter() {
            beginGame();
    }

    public void beginGame() {
        SceneManager.LoadScene("NivelPrincipal");

    }
}
