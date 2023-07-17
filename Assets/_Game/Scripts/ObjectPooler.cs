using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    public List<GameObject> pooledObjects;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
    }
    public GameObject GetPooledObject(GameObject go)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].name.Contains(go.name))
            {
                return pooledObjects[i];
            }
        }
        GameObject tmp = Instantiate(go);
        tmp.SetActive(false);
        pooledObjects.Add(tmp);
        return tmp;
    }
}
