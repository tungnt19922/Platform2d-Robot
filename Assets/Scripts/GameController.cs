using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private UIManager UImng;

    [Header("UI Manager")]
    [SerializeField] private int score;
    [SerializeField] private bool isGameover;


    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        UImng = FindObjectOfType<UIManager>();
        UImng.SetScoreText("SCORE: " + score);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetScore(int value)
    {
        score = value;
    }

    public int GetScore()
    {
        return score;
    }

    public void ScoreIncrement()
    {
        score++;
        UImng.SetScoreText("SCORE: " + score);
    }

}
