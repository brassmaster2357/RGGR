using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public bool paused = false;

    public GameObject pauseIndicator;
    public GameObject resume;
    public GameObject mainMenu;
    public GameObject quit;

    public void PressedPauseButton()
    {
        if (paused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;

        pauseIndicator.SetActive(true);
        resume.SetActive(true);
        mainMenu.SetActive(true);
        quit.SetActive(true);

        paused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;

        pauseIndicator.SetActive(false);
        resume.SetActive(false);
        mainMenu.SetActive(false);
        quit.SetActive(false);

        paused = false;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        paused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        else
            ReturnToMenu();
    }

    public void loadLevel(int levelNumber)
    {
        if (levelNumber < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(levelNumber);

        else
            ReturnToMenu();
    }
}