using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pool : MonoBehaviour
{

    [SerializeField] private GameObject objectToPool;
    [SerializeField] private float clearTime;
    [SerializeField] private int amountToPool;

    private List<GameObject> pooledObjects;

    public static Pool SharedInstance;

    protected void Awake()
    {
        SharedInstance = this;
        pooledObjects = new List<GameObject>();

        Expand();
        StartCoroutine(ClearPool());
    }

    private void Expand()
    {
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);

            tmp.SetActive(false);

            pooledObjects.Add(tmp);
        }
        print(pooledObjects.Count);
    }
    private IEnumerator ClearPool()
    {
        while (true)
        {
            yield return new WaitForSeconds(clearTime);
            var deleteObjects = pooledObjects.RemoveAll(x => x.activeInHierarchy == false);
        }
    }
    public virtual GameObject GetPooledObject()
    {
        foreach (var item in pooledObjects)
        {
            if (!item.activeInHierarchy)
            {
                return item;
            }
        }
        Expand();
        return null;
    }
}


