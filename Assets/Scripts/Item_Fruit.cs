using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Fruit : MonoBehaviour
{
    private Animator anim;
    private GameController gameController;

    private void Start()
    {
        anim = GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameController.ScoreIncrement();
            Destroy(gameObject);
        }
    }
}
