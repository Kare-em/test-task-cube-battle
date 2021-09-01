using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float minHealth = 50f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float minSpeed = 50f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private int numberOfTeam;
    [SerializeField] private float speedHeal;
    [SerializeField] private float explosionDistance;
    [SerializeField] private float explosionDamage;

    [SerializeField] private GameObject explosionEffect;

    private float health;
    private float murders;
    private float speed;

    private float startHealth;
    private float damagescore;
    private float experience;

    private Weapon myWeapon;
    private Unit me;
    private Unit target = null;

    

    private string nickname;

    private bool readyForShot = true;

    public int NumberOfTeam { get => numberOfTeam; }
    public string Nickname { get => nickname; }
    public float Health
    {
        get => health;
        set
        {
            health = value;
            if (health < 0)
                throw new System.NotImplementedException();
        }
    }
    public float Murders
    {
        get => murders;
        set
        {
            murders = value;
            if (murders < 0)
                throw new System.NotImplementedException();
        }
    }
    public float Experience
    {
        get => experience;
        set
        {
            experience = value;
            if (experience < 0)
                throw new System.NotImplementedException();
        }
    }

    public float DamageScore
    {
        get => damagescore;
        set
        {
            damagescore = value;
            if (damagescore < 0)
                throw new System.NotImplementedException();
        }
    }

    public float StartHealth { get => startHealth; }
    public float ExplosionDistance { get => explosionDistance; }
    public float ExplosionDamage { get => explosionDamage; }

    private void Awake()
    {
        Murders = 0;
        me = GetComponent<Unit>();
        Health = Random.Range(minHealth, maxHealth);
        speed = Random.Range(minSpeed, maxSpeed);
        myWeapon = GetComponentInChildren<Weapon>();
    }
    private void Start()
    {
        startHealth = Health;
        nickname = GetInstanceID().ToString();

    }


    private void Die()
    {


        UnitManager.SharedInstance.Teams[NumberOfTeam].Remove(me);

        gameObject.SetActive(false);
        Explode();
    }



    private void Move(Vector3 differenceVector)
    {
        transform.position += differenceVector.normalized * speed * Time.deltaTime;
    }


    private IEnumerator reloadingWeapon()
    {
        readyForShot = false;
        yield return new WaitForSeconds(1 / myWeapon.DamagePerSecond);
        readyForShot = true;
    }

    private void Attack(Unit targetUnit)
    {
        Instantiate(explosionEffect, myWeapon.transform);
        myWeapon.Shot();
        StartCoroutine(reloadingWeapon());

    }

    private void GetDamage(Unit unit, float damage)
    {

        if (Health < damage)
        {
            unit.Murders++;
            unit.Experience += Health + StartHealth;
            Die();
        }
        else
        {
            Health -= damage;
            unit.Experience += damage;
            unit.DamageScore += damage;
        }
    }

    private void Explode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        var theNearest = UnitManager.SharedInstance.FindInTheDistance(me);
        foreach (var unit in theNearest)
        {
            unit.GetDamage(me, ExplosionDamage);
        }

    }

    private void RotateTowardTarget()
    {
        Vector3 moveDir = target.transform.position;

        transform.LookAt(moveDir);
    }


    private void Heal()
    {
        var addHealth = speedHeal * Time.deltaTime;
        if (health + addHealth < StartHealth)
            health += speedHeal * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            var bullet = other.gameObject.GetComponent<Bullet>();
            if (bullet.Parent.NumberOfTeam != NumberOfTeam)
            {
                Instantiate(explosionEffect, bullet.transform.position, Quaternion.identity);

                GetDamage(bullet.Parent, bullet.Damage);
            }


        }
    }

    private void Update()
    {
        if (!GameControl.GameIsOver)
        {
            Heal();
            if (target != null && target.gameObject.activeInHierarchy)
            {
                RotateTowardTarget();
                Vector3 differenceVector;
                differenceVector = target.transform.position - transform.position;
                float currentDistance = differenceVector.magnitude;
                if (myWeapon.Distance > currentDistance)
                {
                    if (readyForShot)
                        Attack(target);
                }
                else
                    Move(differenceVector);
            }
            else
                target = UnitManager.SharedInstance.FindClosestEnemy(me);
        }
    }
}