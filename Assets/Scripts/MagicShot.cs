using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 20f;
    public float fireRate = 0.5f; // 총알 발사 간격

    private float nextFire; // 다음 발사 시간

    void Update()
    {
        // 다음 발사 시간이 되면 자동으로 총알 발사
        if (Time.time > nextFire)
        {
            // 총알 발사
            Shoot();

            // 다음 발사 시간 갱신
            nextFire = Time.time + fireRate;
        }
    }



    // 총알 발사 시 호출되는 메서드
    public void Shoot()
    {
        // 총알 프리팹 생성
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // 총알 발사
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

        // 2초 뒤에 파괴
        Destroy(bullet, 2.0f);
    }
}


