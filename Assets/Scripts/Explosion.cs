using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float increaseScale;
    [SerializeField] private float disappierTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Disappier());
    }

    private IEnumerator Disappier()
    {
        yield return new WaitForSeconds(disappierTime);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale *= 1+increaseScale*Time.deltaTime;
    }
}
