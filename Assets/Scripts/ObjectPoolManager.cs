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
    void Start()
    {

    }

    public GameObject SpawnObject(string tag, Vector3 position)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Object pool with tag " + tag + " doesn't exist.");
            return null;
        }
        if (poolDictionary[tag].Count < 1)
            FillPool(tag);

        GameObject gameObject = poolDictionary[tag].Dequeue();

        gameObject.SetActive(true);
        gameObject.transform.position = position;

        return gameObject;
        
    }

    public void DespawnObject(string tag, GameObject gameObject)
    {
        if (poolDictionary.ContainsKey(tag)) 
        {
            poolDictionary[tag].Enqueue(gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Object pool with tag " + tag + " doesn't exist.");
        }
    }

    void CreatePool(PoolSettings poolSettings)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < poolSettings.startSize; i++)
        {
            GameObject gameObject = Instantiate(poolSettings.prefab, parentObject);
            gameObject.SetActive(false);

            objectPool.Enqueue(gameObject);
        }

        poolDictionary.Add(poolSettings.tag, objectPool);
    }

    void FillPool(string tag)
    {
        Debug.Log("filling pools");
        if (poolDictionary.ContainsKey(tag))
        {
            foreach(PoolSettings settings in poolSettings)
            {
                if (settings.tag == tag) 
                {
                    for (int i = 0; i < settings.startSize; i++)
                    {
                        GameObject gameObject = Instantiate(settings.prefab, parentObject);
                        gameObject.SetActive(false);

                        poolDictionary[tag].Enqueue(gameObject);
                    }
                    return;
                }
            }
        }
        else
        {
            Debug.LogWarning("Object pool with tag " + tag + " doesn't exist.");
        }
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
