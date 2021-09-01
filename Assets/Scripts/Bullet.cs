using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    private float maxdistance;
    private float velocity;
    private Unit parent;
    public float Damage { get => damage; set => damage = value; }
    public float MaxDistance { get => maxdistance; set => maxdistance = value; }
    public float Velocity { get => velocity; set => velocity = value; }
    public Unit Parent { get => parent; set => parent = value; }
    public float DistanceTraveled { get => distanceTraveled; set => distanceTraveled = value; }

    private float distanceTraveled;

    private void DeleteSelf()
    {
        gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (!GameControl.GameIsOver)
        {
            DistanceTraveled += Velocity * Time.fixedDeltaTime;
            if (DistanceTraveled > MaxDistance)
            {
                DeleteSelf();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag != "Bullet")
        {
            DeleteSelf();
        }
    }
}
