using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();
    public List<GameObject> obstacles;
    bool spawnObstacles = false;
    public float timeBetweenObstacles = 2f;
    public List<GameObject> enemyOnHorsePrefabs;
    public List<Transform> backSpawnPoints;
    public void SpawnObstacle()
    {
        GameObject obstacle = Instantiate(
            obstacles[Random.Range(0, obstacles.Count)],
            spawnPoints[Random.Range(0, spawnPoints.Count)].position,
            Quaternion.identity
        );
    }
    
    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(
            enemyOnHorsePrefabs[Random.Range(0, enemyOnHorsePrefabs.Count)],
            backSpawnPoints[Random.Range(0, backSpawnPoints.Count)].position,
            Quaternion.identity
        );
    }
    

    public void StartSpawnObstacles()
    {
        spawnObstacles = true;
        StartCoroutine(SpawnRoutine());
    }

    public void StopSpawnObstacles()
    {
        spawnObstacles = false;
        StopAllCoroutines();
    }

    IEnumerator SpawnRoutine()
    {
        while (spawnObstacles)
        {
            yield return new WaitForSeconds(timeBetweenObstacles);
            SpawnObstacle();
            if (GameObject.FindGameObjectsWithTag("Enemy").Length < 2)
            {
                SpawnEnemy();                
            }
        }
    }
}