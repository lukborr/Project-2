using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject spider;
    [SerializeField] GameObject scarecrow;
    [SerializeField] GameObject pumpkin;
    [SerializeField] GameObject spawnedEnemiesObject;
    [SerializeField] GameObject EnemiesParent;
    List<GameObject>[] spawnedEnemies = new List<GameObject>[3];
    //List<GameObject> spawnedEnemies= new List<GameObject>();
    private int enemiesLeft = 3;
    void Start()
    {
        // Initialization for every list with spawned enemies in array of lists
        for (int i = 0; i < spawnedEnemies.Length; i++)
        {
            spawnedEnemies[i] = new List<GameObject>();
        }
        //WaveManager(waveNumber);
        WaveManger(1);
    }

    private void OnEnable()
    {
        EventManager.OnGamePaused += StopSpawningEnemies;
       // EventManager.OnGameResumed += ResumeSpawningEnemies;
    }
    private void OnDisable()
    {
        EventManager.OnGamePaused -= StopSpawningEnemies;
        //EventManager.OnGameResumed += ResumeSpawningEnemies;
    }

    private void StopSpawningEnemies()
    {
        StopAllCoroutines();
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

    private void SpawnEnemies( GameObject enemyGo, int howMany, float timeToSpawn, int waveNumber)
    {
        for (int i = 0;i < howMany; i++)
        {
            var spawnedEnemy = Instantiate(enemyGo, GenerateRandomPosition(), enemyGo.transform.rotation, EnemiesParent.transform);
            spawnedEnemies[waveNumber - 1].Add(spawnedEnemy);
            spawnedEnemy.gameObject.SetActive(false);
        }
        StartCoroutine(ActivateEnemies(spawnedEnemies[waveNumber - 1],timeToSpawn));
    }

    private IEnumerator ActivateEnemies(List<GameObject> list, float timeToSpawn)
    {
        for (int i = 0; i < list.Count; i++)
        {
            yield return new WaitForSeconds(timeToSpawn);
            list[i].SetActive(true);
        }
    }
    private void WaveManger(int waveNumber)
    {
        switch (waveNumber)
        {
            case 1:
                SpawnEnemies(spider,2, 1f, 1);
                StartCoroutine(nextWave(10f, waveNumber + 1));
                break;
            case 2:
                SpawnEnemies(pumpkin, 10, 2f, 2);
                StartCoroutine(nextWave(10f, waveNumber + 1));
                break;
            case 3:
                SpawnEnemies(scarecrow, 10, 5f, 3) ;
                break;
        }
    }

    private IEnumerator nextWave(float timeToNewWave, int waveNumber)
    {
        yield return new WaitForSeconds(timeToNewWave);
        WaveManger(waveNumber);
    }



}
    