using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [HideInInspector] public static ObjectPoolManager Instance;
    [SerializeField] List<PoolSettings> poolSettings;
    [SerializeField] Transform parentObject;
    Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        SetInstance();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (PoolSettings settings in poolSettings)
        {
            CreatePool(settings);
        }
    }

    public GameObject SpawnObject(string tag, Vector3 position)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Object pool with tag " + tag + " doesn't exist.");
            return null;
        }

        if (poolDictionary[tag].Count < 1)
        {
            FillPool(tag);
        }

        GameObject spawnedObject = poolDictionary[tag].Dequeue();
        spawnedObject.SetActive(true);
        spawnedObject.transform.position = position;

        return spawnedObject;
    }

    public void DespawnObject(string tag, GameObject despawnedObject)
    {
        if (poolDictionary.ContainsKey(tag)) 
        {
            poolDictionary[tag].Enqueue(despawnedObject);
            despawnedObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Object pool with tag " + tag + " doesn't exist.");
        }
    }

    void CreatePool(PoolSettings settings)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < settings.startSize; i++)
        {
            objectPool.Enqueue(InstantiateNewPoolObject(settings));
        }

        poolDictionary.Add(settings.tag, objectPool);
    }

    void FillPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Object pool with tag " + tag + " doesn't exist.");
            return;
        }

        foreach (PoolSettings settings in poolSettings)
        {
            if (settings.tag == tag) 
            {
                for (int i = 0; i < settings.startSize; i++)
                {
                    poolDictionary[tag].Enqueue(InstantiateNewPoolObject(settings));
                }
                return;
            }
        }
    }

    private GameObject InstantiateNewPoolObject(PoolSettings settings)
    {
        GameObject newObject = Instantiate(settings.prefab, parentObject);
        newObject.SetActive(false);
        return newObject;
    }

    void SetInstance()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.Log("Object Pool Manager already exists");
            Destroy(this.gameObject);
        }
    }
}

[System.Serializable]
public class PoolSettings
{
    public string tag;
    public GameObject prefab;
    public int startSize;
}
