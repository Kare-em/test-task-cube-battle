using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float minDamage;
    [SerializeField] private float maxDamage;

    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;

    [SerializeField] private float minDamagePerSecond;
    [SerializeField] private float maxDamagePerSecond;

    [SerializeField] private float bulletSpeed;

    private float damage;
    private float distance;
    private float damagePerSecond;

    public float Damage { get => damage; }
    public float Distance { get => distance; }
    public float DamagePerSecond { get => damagePerSecond; }

    private void Awake()
    {
        damage = Random.Range(minDamage, maxDamage);
        distance = Random.Range(minDistance, maxDistance);
        damagePerSecond = Random.Range(minDamagePerSecond, maxDamagePerSecond);
    }
    public void Shot()
    {
        GameObject bullet = Pool.SharedInstance.GetPooledObject();
        print(bullet);
        if (bullet != null)
        {
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;

            bullet.SetActive(true);

            var bulletProperties = bullet.GetComponent<Bullet>();

            bulletProperties.Velocity = bulletSpeed;
            bulletProperties.DistanceTraveled = 0f;



            bulletProperties.Damage = Damage;
            bulletProperties.MaxDistance = Distance;

            var velocity = transform.up * bulletSpeed;
            bullet.GetComponent<Rigidbody>().velocity = velocity;

            var parent = transform.parent.gameObject;
            bulletProperties.Parent = parent.GetComponent<Unit>();

        }

    }

}
