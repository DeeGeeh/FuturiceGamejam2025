using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        // Loads the next scene in the build index
        Debug.Log("Play");
        SceneManager.LoadScene("GameplayScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    
    public void OpenCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void Win()
    {
        SceneManager.LoadScene("WinScene");
    }
}