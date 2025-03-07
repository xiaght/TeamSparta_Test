﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed;
    public LayerMask obstacleLayer;  
    private bool hasClimbed = false;
    private float climbCooldown = 2f;
    public int maxhp;
    public int hp;

    public EnemyHealthUI hpui;
    public void OnDamage(int damage) {

        hp -= damage;
        hpui.SetHealth(hp, maxhp);
        if (hp <= 0) {

            gameObject.SetActive(false);
        }
    
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.5f; // 중력을 낮춰서 부드럽게 착지
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        float ran = Random.Range(-0.3f, 0.3f);
        moveSpeed += ran;
        maxhp = 5;
        hp = 5;
    }
    void Update()
    {
        MoveLeft();
        CheckForObstacle();
    }

    // 🔹 왼쪽으로 이동하는 코드
    void MoveLeft()
    {


        Vector3 movement = new Vector2(-1, 0).normalized;
        Vector2 newPosition = transform.position + movement * moveSpeed * Time.deltaTime;
        transform.position = newPosition;


        //rb.velocity = new Vector2(-1, rb.velocity.y)*moveSpeed; // 왼쪽으로 이동
    }


/*    private int stackedEnemies = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // 다른 적 감지
        {
            stackedEnemies++; // 위에 있는 적 개수 증가
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            stackedEnemies--; // 떠난 적 개수 감소
        }
    }
*/







    // 🔹 장애물 감지 & 올라가기
    void CheckForObstacle()
    {
        if (hasClimbed) return;
        // ✅ 왼쪽 방향으로 Raycast 쏘기 (오른쪽이 아니라!)
 //       RaycastHit2D frontHit = Physics2D.Raycast(transform.position + Vector3.up * 0.5f, Vector2.left, 1f, obstacleLayer);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + Vector3.up * 1.5f, Vector2.left, 1f, obstacleLayer);

        // ✅ 위쪽 공간 체크
        RaycastHit2D aboveHit = Physics2D.Raycast(transform.position + Vector3.up * 3f, Vector2.left, 1f, obstacleLayer);

        // 🔥 Raycast 가시화
        Debug.DrawRay(transform.position + Vector3.up * 1.5f, Vector2.left * 1f, Color.red); // 적 감지용
        Debug.DrawRay(transform.position + Vector3.up * 3f, Vector2.left * 1f, Color.blue); // 위쪽 체크용

        int detectedEnemies = hits.Length; // 감지된 적 개수

        Debug.Log("앞에 있는 적 수: " + detectedEnemies);

        // ✅ 특정 개수 이상 적이 있을 때만 점프
        if (detectedEnemies >= 3 && aboveHit.collider == null)
        {
            StartCoroutine(SmoothClimb());
            StartCoroutine(ResetClimbCooldown()); // 
        }
    }

    // 🔹 부드럽게 위로 올라가는 코드
    IEnumerator SmoothClimb()
    {
        float duration = 0.2f; // 올라가는 시간
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + new Vector3(0, 4f, 0); // 위로 이동

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }

    IEnumerator ResetClimbCooldown()
    {
        hasClimbed = true;
        yield return new WaitForSeconds(climbCooldown); // ✅ 2초 동안 점프 금지
        hasClimbed = false;
    }

}
