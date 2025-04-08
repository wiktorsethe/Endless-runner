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
    [SerializeField] private GameObject deathMenu;

    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private Movement movement;
    [SerializeField] private ScoreManager scoreManager;

    private void Awake()
    {
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        levelMenu.SetActive(false);
        deathMenu.SetActive(false);
        Time.timeScale = 0f;
    }

    private void OnEnable()
    {
        Spikes.OnSpikeTouched += DeathMenu;
        Bullet.OnBulletTouched += DeathMenu;
    }

    private void OnDisable()
    {
        Spikes.OnSpikeTouched -= DeathMenu;
        Bullet.OnBulletTouched -= DeathMenu;
    }

    public void MainMenu()
    {
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        levelMenu.SetActive(false);
        deathMenu.SetActive(false);
        Time.timeScale = 0f;
        ResetMenu();
        VolumeManager.TriggerSound("UIClick");

    }

    public void PauseMenu()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(true);
        levelMenu.SetActive(false);
        deathMenu.SetActive(false);
        Time.timeScale = 0f;
        VolumeManager.TriggerSound("UIClick");
    }

    public void LevelMenu()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        levelMenu.SetActive(true);
        deathMenu.SetActive(false);
        Time.timeScale = 1f;
        VolumeManager.TriggerSound("UIClick");
    }

    private void DeathMenu()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        levelMenu.SetActive(false);
        deathMenu.SetActive(true);
        Time.timeScale = 0f;
        VolumeManager.TriggerSound("Death");

    }

    public void ResetMenu()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        levelMenu.SetActive(true);
        deathMenu.SetActive(false);
        Time.timeScale = 1f;
        mapGenerator.ResetLevel();
        movement.ResetSliders();
        scoreManager.ResetScore();
        GameObject.FindWithTag("Player").transform.position = new Vector3(-11.30f, 1, 0);
    }
}
