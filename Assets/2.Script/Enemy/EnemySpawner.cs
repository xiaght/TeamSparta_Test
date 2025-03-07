using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab; 
    public Transform spawnPoint;
    public float spawnInterval = 2f; 
    public Transform parent;

    public List<Enemy> enemyPool;
    public List<Enemy> enemyDeadPool;


    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    public void SetEnemyDead(Enemy temp) {

        temp.gameObject.SetActive(false);
        enemyPool.Remove(temp);
        enemyDeadPool.Add(temp);
    }

    IEnumerator SpawnEnemies()
    {


        while (true)
        {
            Enemy enemy;
            if (enemyDeadPool.Count > 0) // ✅ 풀에서 적 재사용
            {
                enemy = enemyDeadPool[0];
                enemyDeadPool.Remove(enemy);
            }
            else // ✅ 새로운 적 생성
            {
                enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, parent);
            }

            enemy.transform.position = spawnPoint.position; // ✅ 위치 설정
            enemy.gameObject.SetActive(true); // ✅ 적 활성화
            enemy.transform.SetParent(parent); // ✅ 부모 설정
            enemyPool.Add(enemy);


            //            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, parent);

            //           SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, parent);
    }
}
