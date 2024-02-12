using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerStatCarryOver playerStatCarryOver;
    public bool paused = false;
    private bool playerDidThePause = false;
    public bool onMenu = false;
    private UnityEngine.InputSystem.Gamepad controller;

    public GameObject pauseIndicator;

    private void Start()
    {
        controller = UnityEngine.InputSystem.Gamepad.current;
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            onMenu = true;
        }
    }

    private void Update()
    {
        if (controller == null)
        {
            controller = UnityEngine.InputSystem.Gamepad.current;
        }
        if (onMenu)
        {
            if (controller.aButton.wasPressedThisFrame)
            {
                StartGame();
            }
            if (controller.bButton.wasPressedThisFrame)
            {
                QuitGame();
            }
            if (controller.xButton.wasPressedThisFrame)
            {
                //show options menu
            }
            if (controller.yButton.wasPressedThisFrame)
            {
                GameObject credits = GameObject.Find("Credits");
                if (credits.activeSelf)
                    credits.SetActive(false);
                else
                    credits.SetActive(true);
            }
        }
        if (playerDidThePause)
        {
            if (controller.aButton.wasPressedThisFrame)
            {
                PressedPauseButton();
            }
            if (controller.bButton.wasPressedThisFrame)
            {
                ReturnToMenu();
            }
            if (controller.xButton.wasPressedThisFrame)
            {
                QuitGame();
            }
        }
    }

    public void PressedPauseButton()
    {
        if (paused)
        {
            ResumeGame();
            playerDidThePause = false;
        }
        else
        {
            PauseGame();
            playerDidThePause = true;
        }
    }

    public void PauseNoMenu()
    {
        Time.timeScale = 0;
        paused = true;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;

        pauseIndicator.SetActive(true);
        paused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;

        pauseIndicator.SetActive(false);
        paused = false;
    }

    public void ReturnToMenu()
    {
        playerStatCarryOver.Reset();
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        paused = false;
    }

    public void QuitGame()
    {
        playerStatCarryOver.Reset();
        Application.Quit();
    }

    public void StartGame()
    {
        playerStatCarryOver.Reset();
        SceneManager.LoadScene(1);
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        else
            ReturnToMenu();
    }

    public void LoadLevel(int levelNumber)
    {
        if (levelNumber < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(levelNumber);

        else
            ReturnToMenu();
    }
}