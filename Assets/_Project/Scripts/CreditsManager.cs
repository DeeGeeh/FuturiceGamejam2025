using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CreditsManager : MonoBehaviour
{
    public GameObject creditsPanel; // Reference to the Credits Panel

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void BackButton()
    { 
        SceneManager.LoadScene("MenuScene");
    }
}
