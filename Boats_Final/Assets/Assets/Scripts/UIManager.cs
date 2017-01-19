using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Image[] images;
    public Text[] score;


	// Use this for initialization
	void Start () {
        for (int i = 0; i < score.Length; i++) {
            score[i].text = "X0";
        }
	}

}
