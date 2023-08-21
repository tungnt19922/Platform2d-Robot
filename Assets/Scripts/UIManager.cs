using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject gameoverPanel;

    public TMPro.TMP_Text scoreText;

    public void SetScoreText(string txt)
    {
        if (scoreText != null)
        {
            scoreText.text = txt;
        }
    }

    public void ShowGameoverPanel(bool isShow)
    {
        gameoverPanel.SetActive(isShow);
    }

}
