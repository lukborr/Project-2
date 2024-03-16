using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject spider;
    [SerializeField] GameObject scarecrow;
    [SerializeField] GameObject pumpkin;
    void Start()
    {
        StartCoroutine(WaveManager());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnEnemy(Vector2 position, GameObject enemy)
    {
        Instantiate(enemy, position, enemy.transform.rotation);
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

    private IEnumerator SpawnerRoutine(GameObject enemyGo, float timeBeetweenSpawns, int amount)
    {   while(amount  > 0)
        {
            yield return new WaitForSeconds(timeBeetweenSpawns);
            SpawnEnemy(GenerateRandomPosition(), enemyGo);
            amount--;
        }
       
    }

    private IEnumerator WaveManager()
    {
        StartCoroutine(SpawnerRoutine(spider, 2.5f, 10));
        yield return new WaitForSeconds(25f);
        StartCoroutine(SpawnerRoutine(pumpkin, 2.5f, 10));
    }
}
    