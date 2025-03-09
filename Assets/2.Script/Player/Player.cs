using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Scanner scan;

    public Bullet bullet;
    public Transform bulletPar;
    public float shotspeed = 1f;
    public List<Bullet> bulletPool;
    public List<Bullet> bulletDeadPool;

    public LayerMask enemyLayer;
    public int raycastDistance;

    public Image hpSlider;
    public int maxhp = 100;
    public  int currenthp;
    public bool IsInvincible { get; private set; } = false;
    public float invincibilityDuration = 1f; 
    public Image screenEffect;

    private SpriteRenderer spriteRenderer;

    

    void Start()
    {
      
    }

    public void OnDamage(int damage)
    {
        if (IsInvincible) return;

        currenthp -= damage;
        SetPlayerHp();
        if (currenthp <= 0)
        {
            Die();
            return;
        }

        

        StartCoroutine(DamageEffect()); 
        StartCoroutine(Invincibility()); 
    }

    IEnumerator DamageEffect()
    {
        if (screenEffect != null)
        {
            screenEffect.color = new Color(1, 0, 0, 0.5f); 
        }
        spriteRenderer.color = new Color(1, 0, 0, 1); 

        yield return new WaitForSeconds(0.2f);

        if (screenEffect != null)
        {
            screenEffect.color = new Color(1, 0, 0, 0); 
        }
        spriteRenderer.color = Color.white;
    }

    IEnumerator Invincibility()
    {
        IsInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration); 
        IsInvincible = false;
    }

    void Die()
    {
        SingletonManager.Instance.ui.OnDieUi();
        // 사망 처리 (게임 오버 UI, 리스폰 등 추가 가능)
    }


    public void SetPlayerHp()
    {
        hpSlider.fillAmount = (float)currenthp / maxhp;
   //     Debug.Log("현재 체력:"+currenthp / maxhp);

    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currenthp = maxhp;
        StartShotBullet();
    }
    public void StartShotBullet()
    {
        StartCoroutine(ShotBullet());
    }
    public void SetBulletDead(Bullet temp) {
    
        bulletPool.Remove(temp);
        bulletDeadPool.Add(temp);

    }

    IEnumerator ShotBullet()
    {
        if (!scan.nearestTarget)
        {
            yield return new WaitForSecondsRealtime(shotspeed);
            StartCoroutine(ShotBullet());
        }
        else
        {

            Bullet temp;
            if (bulletDeadPool.Count >= 1)
            {

                temp = bulletDeadPool[0];
                temp.gameObject.SetActive(true);
                bulletDeadPool.Remove(temp);
                bulletPool.Add(temp);
            }
            else
            {
                temp = Instantiate(bullet);
                bulletPool.Add(temp);
            }

            Vector3 targetPos = scan.nearestTarget.position;
            Vector3 dir = targetPos - transform.position;

            dir = dir.normalized;

            //     Bullet temp = Instantiate(bullet);
            temp.transform.position = transform.position;
            temp.transform.parent = bulletPar;
            temp.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);


            yield return new WaitForSecondsRealtime(shotspeed);

            StartCoroutine(ShotBullet());
        }



    }

    void Update()
    {
        BlockEnemyJump();


    }

    void BlockEnemyJump()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, raycastDistance, enemyLayer);

        Debug.DrawRay(transform.position, Vector2.right * raycastDistance, Color.red); 

        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log(hit.collider.name);
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.hasClimbed = true; 
                }
            }
        }
    }
}
