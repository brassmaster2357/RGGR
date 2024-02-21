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
    private bool playerManualPause = false;
    private bool gameOverScreen = false;
    public bool onMenu = false;
    private UnityEngine.InputSystem.Gamepad controller;

    public GameObject pauseIndicator;

    private void Awake()
    {
        controller = UnityEngine.InputSystem.Gamepad.current;
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            onMenu = true;
        }
        if (SceneManager.GetActiveScene().buildIndex >= 7)
        {
            gameOverScreen = true;
            if (SceneManager.GetActiveScene().buildIndex == 7)
            {
                GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>().text = ((int)playerStatCarryOver.cash).ToString();
            }
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
            if (controller.xButton.wasPressedThisFrame)
            {
                GameObject credits = GameObject.Find("Canvas").transform.GetChild(0).gameObject; // why public variable when can GetChild()
                if (credits.activeSelf)
                    credits.SetActive(false);
                else
                    credits.SetActive(true);
            }
        }
        if (playerManualPause)
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
        if (gameOverScreen)
        {
            if (controller.aButton.wasPressedThisFrame)
            {
                ReturnToMenu();
            }
            if (controller.xButton.wasPressedThisFrame)
            {
                GameObject credits = GameObject.Find("Canvas").transform.GetChild(0).gameObject; // why public variable when can GetChild()
                if (credits.activeSelf)
                    credits.SetActive(false);
                else
                    credits.SetActive(true);
            }
            if (controller.yButton.wasPressedThisFrame)
            {
                if (SceneManager.GetActiveScene().buildIndex == 7)
                {
                    SceneManager.LoadScene(2); // Don't overwrite the player, but this time on purpose!
                }
            }
        }
    }

    public void PressedPauseButton()
    {
        if (paused)
        {
            ResumeGame();
            playerManualPause = false;
        }
        else
        {
            PauseGame();
            playerManualPause = true;
        }
    }

    public void GameOver()
    {
        playerStatCarryOver.Reset();
        SceneManager.LoadScene(8);
        Time.timeScale = 1;
        paused = false;
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