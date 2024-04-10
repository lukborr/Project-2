using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject spider;
    [SerializeField] GameObject scarecrow;
    [SerializeField] GameObject pumpkin;
    [SerializeField] GameObject spawnedEnemiesObject;
    private int waveNumber = 0;
    private int enemiesLeft = 3;
    void Start()
    {
        WaveManager(waveNumber);
    }

    private void OnEnable()
    {
        EventManager.OnGamePaused += StopSpawningEnemies;
        EventManager.OnGameResumed += ResumeSpawningEnemies;
    }
    private void OnDisable()
    {
        EventManager.OnGamePaused -= StopSpawningEnemies;
        EventManager.OnGameResumed += ResumeSpawningEnemies;
    }


    private void StopSpawningEnemies()
    {
        StopAllCoroutines();
    }

    private void ResumeSpawningEnemies()
    {
        WaveManager(waveNumber);
    }



    private void SpawnEnemy(Vector2 position, GameObject enemy)
    {
        Instantiate(enemy, position, enemy.transform.rotation, spawnedEnemiesObject.transform);
    }

    private Vector2 GenerateRandomPosition()
    {
        float x;
        float y;
        int randomChoice = Random.Range(0, 2);
        if(randomChoice == 0)
        {
            x = -20.0f;
            y= Random.Range(-6f, 9.1f);
        }
        else
        {
            int randY = Random.Range(0, 2);
            if(randY == 0) 
            {
                y = -6;
            }
            else
            {
                y = 9;
            }
            x = Random.Range(-19f, 0f);
        }
        Vector2 position = new Vector2(x, y);
        return position;
    }

    private IEnumerator SpawnerRoutine(GameObject enemyGo, float timeBeetweenSpawns)
    { 
            while (enemiesLeft > 0)               
            {
                yield return new WaitForSeconds(timeBeetweenSpawns);
                SpawnEnemy(GenerateRandomPosition(), enemyGo);
                enemiesLeft--;
            }
        waveNumber++;
        switch (waveNumber)
        {
            case 1:
                enemiesLeft = 5;
                break;
            case 2:
                enemiesLeft = 8;
                break;
        }
        WaveManager(waveNumber);
    }

    private void WaveManager(int waveNumber)
    {
        switch (waveNumber)
        {
            case 0:              
                StartCoroutine(SpawnerRoutine(spider, 2.5f));               
                break;
            case 1:              
                StartCoroutine(SpawnerRoutine(pumpkin, 2.5f));
                break;
        }
             
    }
}
    