using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab;
    public Transform spawnPoint;
    public Transform player; // ✅ 플레이어 위치 참조
    public float spawnInterval = 2f;
    public Transform parent;

    public List<Enemy> enemyPool;
    public List<Enemy> enemyDeadPool;

    public float moveSpeed = 2f; // ✅ 스포너 이동 속도
    public float stopDistance = 3f; // ✅ 이동이 멈출 거리
    public int enemyGeneration;
    public int count;
    private bool hasStopped = false; // ✅ 멈춘 상태 체크

    void Awake()
    {
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        if (!hasStopped) // ✅ 멈추지 않았을 때만 이동
        {
            MoveLeft();

            // ✅ 플레이어와의 거리 계산
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= stopDistance)
            {
                hasStopped = true; // ✅ 멈추기
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
            if (enemyDeadPool.Count > 0) // ✅ 풀에서 적 재사용
            {
                enemy = enemyDeadPool[0];
                enemyDeadPool.Remove(enemy);
            }
            else // ✅ 새로운 적 생성
            {
                enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, parent);
            }


            enemy.enemyGeneration = enemyGeneration;
            enemy.transform.position = spawnPoint.position; // ✅ 위치 설정
            enemy.gameObject.SetActive(true); // ✅ 적 활성화
            enemy.transform.SetParent(parent); // ✅ 부모 설정
            enemyPool.Add(enemy);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
