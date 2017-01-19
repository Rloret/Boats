using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {
    public int Score = 0;

    private int maxScore=7;
    private GameObject gameController;
    // Use this for initialization

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");

    }
    public void addScore() {
        Score += 1;
        if (Score == maxScore) {
           //GameController fin del juego
			gameController.GetComponent<GameController>().FinishGame(this.gameObject);
        }
        else
        {
            gameController.GetComponent<GameController>().calculateScores();
        }
    }
}
