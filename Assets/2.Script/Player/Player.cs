using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Scanner scan;

    public Bullet bullet;
    public Transform bulletPar;
    public float shotspeed = 1f;
    public List<Bullet> bulletPool;
    public List<Bullet> bulletDeadPool;

    private void Awake()
    {

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
}
