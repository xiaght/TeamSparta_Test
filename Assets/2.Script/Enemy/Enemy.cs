using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed;

    public float maxSpeed = 2f;
    public LayerMask obstacleLayer;  
    public bool hasClimbed = false;


    public int enemyGeneration;






    public float climbCooldown = 1f;
    public int maxhp;
    public int hp;

    public EnemyHealthUI hpui;


    public float attackCooldown = 2f; 
    public int damage = 10; 
    public bool isAttacking = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {

            Debug.Log("공격");
            Player temp = collision.gameObject.GetComponent<Player>();

            StartCoroutine(AttackPlayer(temp));
        }
    }


/*    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isAttacking)
        {

            Debug.Log("공격");
            Player temp = other.GetComponent<Player>();

            StartCoroutine(AttackPlayer(temp));
        }
    }*/

    IEnumerator AttackPlayer(Player player)
    {
        
        isAttacking = true;
        while (player != null && player.IsInvincible == false)
        {
            player.OnDamage(damage); // ✅ 플레이어에게 데미지 줌
            yield return new WaitForSeconds(attackCooldown); // ✅ 일정 시간마다 공격
        }
        isAttacking = false;
    }


    public void OnDamage(int damage) {

        hp -= damage;
        hpui.SetHealth(hp, maxhp);
        if (hp <= 0) {
            SingletonManager.Instance.enemyspwa.SetEnemyDead(this);
        }
    
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
   //     rb.gravityScale = 0.5f; 
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
  //      rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        maxhp = 5;


    }

    private void OnEnable()
    {
        hp = 5;
        hpui.SetHealth(hp, maxhp);
    }
    void Update()
    {
        MoveLeft();
        CheckForObstacle();
    }


    void MoveLeft()
    {


        Vector3 movement = new Vector2(-1, 0).normalized;
        Vector2 newPosition = transform.position + movement * moveSpeed * Time.deltaTime;
        transform.position = newPosition;

        //rb.velocity = new Vector2(-1, rb.velocity.y)*moveSpeed; 

        /*        rb.AddForce(Vector2.left * moveSpeed, ForceMode2D.Force);

                // ✅ 최대 속도 제한 (속도가 너무 빨라지지 않도록)
                if (rb.velocity.x < -maxSpeed)
                {
                    rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
                }*/
    }


    private void UpdateMoving(Vector3 direction)
    {
        direction = MultiplyMyPlayerMoveSpeed(direction);

        GetComponent<Rigidbody>().velocity = direction;
    }
     Vector3 MultiplyMyPlayerMoveSpeed(Vector3 direction)
    {
        Vector3 temp = new Vector2(-1, 0).normalized;
        return temp;
    }















    void CheckForObstacle()
    {
        if (hasClimbed) return;

        float dynamicRaycastDistance = 1f + enemyGeneration * 0.02f;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + Vector3.up * 0.5f, Vector2.left, dynamicRaycastDistance, obstacleLayer);

        RaycastHit2D aboveHit = Physics2D.Raycast(transform.position + Vector3.up * 1.5f, Vector2.left, 1f, obstacleLayer);


        int detectedEnemies = hits.Length;
        int requiredEnemies = 4 + enemyGeneration/2 ;


        Debug.DrawRay(transform.position + Vector3.up * 0.5f, Vector2.left * dynamicRaycastDistance, Color.red);
        Debug.DrawRay(transform.position + Vector3.up * 1.5f, Vector2.left * 1f, Color.blue); 


    //    Debug.Log($"적 세대 {enemyGeneration}, Raycast 거리 {dynamicRaycastDistance}, 필요 적 수 {requiredEnemies}");


        if (detectedEnemies >= requiredEnemies && aboveHit.collider == null)
        {
            StartCoroutine(SmoothClimb());
            StartCoroutine(ResetClimbCooldown()); // 
        }
    }

    IEnumerator SmoothClimb()
    {
        float duration = 0.2f; 
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + new Vector3(0, 1.5f, 0);

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
        yield return new WaitForSeconds(climbCooldown); 
        hasClimbed = false;
    }

}
