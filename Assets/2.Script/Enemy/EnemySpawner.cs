using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab;
    public Transform spawnPoint;
    public Transform player; 
    public float spawnInterval = 2f;
    public Transform parent;

    public List<Enemy> enemyPool;
    public List<Enemy> enemyDeadPool;

    public float moveSpeed = 2f; 
    public float stopDistance = 3f;
    public int enemyGeneration;
    public int count;
    private bool hasStopped = false; 

    void Awake()
    {
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        if (!hasStopped)
        {
            MoveLeft();

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= stopDistance)
            {
                hasStopped = true; 
                SingletonManager.Instance.background.StopScrolling();
            }
        }
    }

    void MoveLeft()
    {
        Vector3 movement = new Vector2(-1, 0).normalized;
        Vector2 newPosition = transform.position + movement * moveSpeed * Time.deltaTime;
        transform.position = newPosition;
    }

    public void SetEnemyDead(Enemy temp)
    {
        temp.gameObject.SetActive(false);
        enemyPool.Remove(temp);
        enemyDeadPool.Add(temp);
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            count++;
            enemyGeneration=count/4;
               
            Enemy enemy;
            if (enemyDeadPool.Count > 0) 
            {
                enemy = enemyDeadPool[0];
                enemyDeadPool.Remove(enemy);
            }
            else // ✅ 새로운 적 생성
            {
                enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, parent);
            }


            enemy.enemyGeneration = enemyGeneration;
            enemy.transform.position = spawnPoint.position;
            enemy.gameObject.SetActive(true); 
            enemy.transform.SetParent(parent);
            enemyPool.Add(enemy);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
