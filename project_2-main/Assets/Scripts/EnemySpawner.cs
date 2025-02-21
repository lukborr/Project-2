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
    [SerializeField] GameObject rat;
    List<GameObject>[] spawnedEnemies = new List<GameObject>[5];
    private List<float> timeBetweenSpawns = new List<float>();
    private int waveNumber = 1;
    private int waveMax = 5;
    private bool isPaused = false;
    void Start()
    {
        // Initialization for every list with spawned enemies in array of lists
        for (int i = 0; i < spawnedEnemies.Length; i++)
        {
            spawnedEnemies[i] = new List<GameObject>();
        }      
        WaveManger(0);
        timeBetweenSpawns.Add(2f);
        timeBetweenSpawns.Add(3f);
        timeBetweenSpawns.Add(5f);
        timeBetweenSpawns.Add(6f);
        timeBetweenSpawns.Add(7f);
    }

    private void OnEnable()
    {
        EventManager.OnGamePaused += PauseGame;
        EventManager.OnPreviousWaveUnfreezed += NextWaveActivation;
        EventManager.OnGameResumed += UnPauseGame;
    }

    private void OnDisable()
    {
        EventManager.OnGamePaused -= PauseGame;
        EventManager.OnPreviousWaveUnfreezed -= NextWaveActivation;
        EventManager.OnGameResumed += UnPauseGame;
    }

    private void UnPauseGame()
    {
        isPaused = false;
    }

    private void PauseGame()
    {
        isPaused = true;
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

    private List<GameObject> SpawnEnemies( GameObject enemyGo, int howMany, int waveNumber)
    {
        for (int i = 0;i < howMany; i++)
        {
            var spawnedEnemy = Instantiate(enemyGo, GenerateRandomPosition(), enemyGo.transform.rotation, EnemiesParent.transform);
            spawnedEnemies[waveNumber - 1].Add(spawnedEnemy);
            spawnedEnemy.gameObject.SetActive(false);
        }
        return spawnedEnemies[waveNumber-1];
    }
    private void ActivateEnemies(List<GameObject> list, float timeToSpawn, int waveNumber)
    {
        if (spawnedEnemies[waveNumber-1] != null)
        StartCoroutine(ActivateEnemiesRoutine(list, timeToSpawn, waveNumber));
    }

    private void NextWaveActivation(int waveNumber)
    {
        float time = timeBetweenSpawns[waveNumber - 1];
        ActivateEnemies(spawnedEnemies[waveNumber - 1], time, waveNumber);
    }  

    private IEnumerator ActivateEnemiesRoutine(List<GameObject> list, float timeToSpawn, int waveNumber)
    {
       if(waveNumber != 1)
        {
            yield return new WaitForSeconds(2f);
        }
        for (int i = 0; i < list.Count; i++)
        {
            while (isPaused)
            {
                yield return null;
            }
            yield return new WaitForSeconds(timeToSpawn);
            list[i].SetActive(true);         
            if(i == list.Count - 1)
            {
                waveNumber++;
                if (waveNumber <= waveMax)
                {
                    EventManager.CallOnPreviousWaveUnfrezzedEvent(waveNumber);
                }               
            }
        }
    }
    private void WaveManger(int levelNumber)
    {
        switch (levelNumber)
        {
            case 0:
               var spiders = SpawnEnemies(spider, 50,1);
                StartCoroutine(ActivateEnemiesRoutine(spiders, 0.6f, waveNumber));
                var pumpkins = SpawnEnemies(pumpkin, 40, 2);
                var scarecrows = SpawnEnemies(scarecrow, 12, 3);
                var rats = SpawnEnemies(rat, 15, 4);
                var spiders2 = SpawnEnemies(spider, 60, 5);
                break;
        }       
    }




}
    