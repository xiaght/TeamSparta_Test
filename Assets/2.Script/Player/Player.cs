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
    public float invincibilityDuration = 1f; // ✅ 피격 무적 시간
    public Image screenEffect; // ✅ UI 효과 (흐릿함 or 빨간색 효과)

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

        

        StartCoroutine(DamageEffect()); // ✅ 피격 효과 실행
        StartCoroutine(Invincibility()); // ✅ 일정 시간 무적
    }

    IEnumerator DamageEffect()
    {
        // ✅ 피격 효과 (빨간색 또는 흐릿한 효과)
        if (screenEffect != null)
        {
            screenEffect.color = new Color(1, 0, 0, 0.5f); // 빨간색 효과
        }
        spriteRenderer.color = new Color(1, 0, 0, 1); // 캐릭터 색 변환

        yield return new WaitForSeconds(0.2f); // ✅ 0.2초간 유지

        if (screenEffect != null)
        {
            screenEffect.color = new Color(1, 0, 0, 0); // 원래대로
        }
        spriteRenderer.color = Color.white; // 캐릭터 원래 색상 복구
    }

    IEnumerator Invincibility()
    {
        IsInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration); // ✅ 무적 시간 유지
        IsInvincible = false;
    }

    void Die()
    {
     //   Debug.Log("플레이어 사망");
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

        Debug.DrawRay(transform.position, Vector2.right * raycastDistance, Color.red); // 🔥 디버깅용

        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log(hit.collider.name);
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.hasClimbed = true; // ✅ 모든 감지된 적 점프 금지!
                }
            }
        }
    }
}
