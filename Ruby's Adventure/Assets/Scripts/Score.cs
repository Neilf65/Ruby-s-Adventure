using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public static Score instance;
    public Text playerScore;

    public int currentScore = 0;
    public MenuController menuController;

    void Awake()
    {
        instance = this;
    }

    //Displays Score 
    void Start()
    {
        playerScore.text = currentScore.ToString() + "/5 Robots Fixed";
    }

    //Adds points whenever an enemy calls "Fix"
    public void AddPoints()
    {
        currentScore += 1;
        playerScore.text = currentScore.ToString() + "/5 Robots Fixed";
         if (currentScore >= 5)
         {
            menuController.WinGame();
         }
    }
}
