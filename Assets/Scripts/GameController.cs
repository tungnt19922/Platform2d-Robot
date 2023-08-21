using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Score Manager")]
    [SerializeField] public int score;


    // Start is called before the first frame update
    void Start()
    {
        score = 0;
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
    }

}
