using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // 积己且 利 橇府普
    public Transform spawnPoint; // 利捞 积己瞪 困摹
    public float spawnInterval = 2f; // 利 积己 埃拜
    public Transform parent;

    public List<Enemy> enemyPool;
    public List<Enemy> enemyDeadPool;


    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, parent);
    }
}
