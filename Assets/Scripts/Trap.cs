using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trap : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Knock back!");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
