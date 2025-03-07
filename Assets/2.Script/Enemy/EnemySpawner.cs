using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // ������ �� ������
    public Transform spawnPoint; // ���� ������ ��ġ
    public float spawnInterval = 2f; // �� ���� ����
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
