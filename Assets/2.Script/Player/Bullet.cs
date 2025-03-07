using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public enum BulletType
    {
        Ranged, range
    }

    public BulletType type;
    public int damage;
    public int duration;
    public float speed;


    public int level = 1;

    private void OnEnable()
    {

/*        if (type == BulletType.range)
        {
            StartCoroutine(Deadtime());

        }
        else if (type == BulletType.Ranged) {
          //  startc
        
        }*/
        StartCoroutine(Deadtime());
    }

    IEnumerator Deadtime() {

        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);

//        SingletonManager.Instance.playerweapon.bulletPool.Remove(this);
//        SingletonManager.Instance.playerweapon.bulletDeadPool.Add(this);

    }

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) {

            Enemy temp = collision.GetComponent<Enemy>();
            temp.OnDamage(damage);
            //  temp.gameObject.SetActive(false);
            SingletonManager.Instance.player.SetBulletDead(this);
            gameObject.SetActive(false);
        }
    }
    public void LevelUp()
    {
        level++;
        damage = Mathf.RoundToInt(damage * 1.15f);
        speed *= 1.1f;
        if (level % 3 == 0)
        {
            // 탄환 수 증가 로직 추가 필요 시 구현
        }
    }


}
