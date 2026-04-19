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
    public GameObject startScreen;
    public Boss boss;
    public Player player;

    private void Awake()
    {
        I = this;
        Events.GameOverEvent.AddListener(OnGameOver);
    }

    private void Start()
    {
    }

    public void StartGame()
    {
        boss.Init();
        player.inputEnabled = true;
        startScreen.SetActive(false);
    }

    private void OnGameOver()
    {
        if(gameIsOver) return;
        Sound.I.soundEnabled = false;
        player.inputEnabled = false;
        gameIsOver = true;
        obstacleSpawner.StopSpawnObstacles();
        gameOverModal.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
