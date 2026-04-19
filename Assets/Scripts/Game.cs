using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game I;
    public GameObject gameOverModal;
    public float speed = 5;
    public ObstacleSpawner obstacleSpawner;
    bool gameIsOver = false;

    private void Awake()
    {
        I = this;
        Events.GameOverEvent.AddListener(OnGameOver);
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        // obstacleSpawner.StartSpawnObstacles();
    }

    private void OnGameOver()
    {
        if(gameIsOver) return;
        Sound.I.soundEnabled = false;
        gameIsOver = true;
        obstacleSpawner.StopSpawnObstacles();
        gameOverModal.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
