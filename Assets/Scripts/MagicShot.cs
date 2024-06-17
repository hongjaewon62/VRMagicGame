using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 20f;
    public float fireRate = 0.5f; // �Ѿ� �߻� ����

    private float nextFire; // ���� �߻� �ð�

    void Update()
    {
        // ���� �߻� �ð��� �Ǹ� �ڵ����� �Ѿ� �߻�
        if (Time.time > nextFire)
        {
            // �Ѿ� �߻�
            Shoot();

            // ���� �߻� �ð� ����
            nextFire = Time.time + fireRate;
        }
    }



    // �Ѿ� �߻� �� ȣ��Ǵ� �޼���
    public void Shoot()
    {
        // �Ѿ� ������ ����
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // �Ѿ� �߻�
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

        // 2�� �ڿ� �ı�
        Destroy(bullet, 2.0f);
    }
}


