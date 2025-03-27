using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject levelMenu;

    [SerializeField] private MapGenerator mapGenerator;
    private void Awake()
    {
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        levelMenu.SetActive(false);
        Time.timeScale = 0f;
    }

    public void MainMenu()
    {
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        levelMenu.SetActive(false);
        Time.timeScale = 0f;
        mapGenerator.ResetLevel();
        GameObject.FindWithTag("Player").transform.position = new Vector3(0, 1, 0);
    }
    
    public void PauseMenu()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(true);
        levelMenu.SetActive(false);
        Time.timeScale = 0f;

    }
    
    public void LevelMenu()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        levelMenu.SetActive(true);
        Time.timeScale = 1f;

    }
}
