using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyCounter;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject fastEnemyPrefab;
    [SerializeField] private GameObject bossEnemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private int maxEnemies = 20;
    [SerializeField] private int enemiesOnScreen = 2;
    [SerializeField] private GameObject winPanel;

    private int enemiesSpawned = 0;
    private int enemiesKilled = 0;

    private List<Enemy> activeEnemies = new List<Enemy>();

    private void Start()
    {
        UpdateCounter();

        for (int i = 0; i < enemiesOnScreen; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemiesSpawned >= maxEnemies)
            return;

        Transform spawnPoint =
            spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject prefabToSpawn;

        if (enemiesSpawned == maxEnemies - 1)
        {
            prefabToSpawn = bossEnemyPrefab;
        }
        else
        {
            if (Random.value < 0.7f)
            {
                prefabToSpawn = enemyPrefab;
            }
            else
            {
                prefabToSpawn = fastEnemyPrefab;
            }
        }

        GameObject enemyObject = Instantiate(
            prefabToSpawn,
            spawnPoint.position,
            Quaternion.identity
        );

        Enemy enemy = enemyObject.GetComponent<Enemy>();

        if (enemy == null)
            return;

        activeEnemies.Add(enemy);
        enemiesSpawned++;

        enemy.OnEnemyDeath += EnemyDied;
    }

    private void EnemyDied(Enemy enemy)
    {
        enemiesKilled++;
        UpdateCounter();

        activeEnemies.Remove(enemy);

        enemy.OnEnemyDeath -= EnemyDied;

        if (enemiesKilled >= maxEnemies)
        {
            WinGame();
            return;
        }

        if (activeEnemies.Count < enemiesOnScreen)
        {
            SpawnEnemy();
        }
    }

    private void UpdateCounter()
    {
        enemyCounter.text = enemiesKilled + "/" + maxEnemies;
    }

    private void WinGame()
    {
        winPanel.SetActive(true);
    }
}