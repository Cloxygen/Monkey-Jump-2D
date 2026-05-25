using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [Header("Settings")]
    [SerializeField] float preSpawnDistance = 10f;
    [SerializeField] float startHeight = 5f;
    [SerializeField] float distanceBetween = 5f;
    [SerializeField] float startingHorizontalVariance = 1f;
    [SerializeField] float maxHorizontalVariance = 10f;
    [SerializeField] float varianceRamp = .25f;
    [SerializeField] float maxXPosition = 6f;
    [SerializeField] float minXPosition = -6f;
    [SerializeField] int cloudFrequency = 20;
    [SerializeField] float minBananaScale = .3f;
    [SerializeField] float bananaScaleRamp = -0.02f;
    [SerializeField] float cameraHeightToSpawnClouds = 10f;
    float currentHorizontalVariance;
    float currentBananaScale = 1f;
    int spawnCount = 0;
    Transform lastObjectSpawned;
    bool isActive = false;
    bool hasInitialized = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            return;
        }

        if (cameraTransform.position.y + preSpawnDistance > lastObjectSpawned.position.y)
        {
            if (spawnCount % cloudFrequency == 0 && cameraTransform.position.y > cameraHeightToSpawnClouds)
            {
                SpawnCloud();
            }
            else
            {
                SpawnBanana();
            }
        }
    }

    void SpawnBanana()
    {
        Vector3 spawnPosition = Vector3.zero;
        spawnPosition.y = CalculateNextSpawnHeight();
        spawnPosition.x = GetRandomHorizontalPositionAroundCenter();

        UpdateHorizontalVariance();

        lastObjectSpawned = ObjectPoolManager.Instance.SpawnObject("Banana", spawnPosition).transform;
        lastObjectSpawned.localScale *= currentBananaScale;

        UpdateBananaScale();
        spawnCount++;
    }

    float CalculateNextSpawnHeight()
    {
        if (lastObjectSpawned == null)
        {
            return startHeight;
        }
        return lastObjectSpawned.position.y + distanceBetween;
    }

    void UpdateHorizontalVariance()
    {
        if (currentHorizontalVariance < maxHorizontalVariance)
        {
            currentHorizontalVariance += varianceRamp;
            if (currentHorizontalVariance > maxHorizontalVariance)
            {
                currentHorizontalVariance = maxHorizontalVariance;
            }
        }
    }

    void UpdateBananaScale()
    {
        if (currentBananaScale > minBananaScale)
        {
            currentBananaScale += bananaScaleRamp;
            currentBananaScale = Mathf.Clamp(currentBananaScale, minBananaScale, 1f);
        }
    }

    void SpawnCloud()
    {
        Vector3 spawnPosition = Vector3.zero;
        spawnPosition.y = CalculateNextSpawnHeight();
        spawnPosition.x = Random.Range(minXPosition, maxXPosition);

        lastObjectSpawned = ObjectPoolManager.Instance.SpawnObject("Cloud", spawnPosition).transform;
        if (lastObjectSpawned.TryGetComponent(out Cloud cloud))
        {
            cloud.SetDirection(RandomBool());
        }

        spawnCount++;
    }

    float GetRandomHorizontalPositionAroundCenter()
    {
        float screenCenter = 0f;
        float minimumSpawnRange = screenCenter - currentHorizontalVariance;
        float maximumSpawnRange = screenCenter + currentHorizontalVariance;

        float clampedMinimumSpawnRange = Mathf.Clamp(minimumSpawnRange, minXPosition, maxXPosition);
        float clampedMaximumSpawnRange = Mathf.Clamp(maximumSpawnRange, minXPosition, maxXPosition);

        return Random.Range(clampedMinimumSpawnRange, clampedMaximumSpawnRange);
    }

    bool RandomBool()
    {
        return Random.value >= 0.5f;
    }


    public void Initialize()
    {
        currentBananaScale = 1f;
        spawnCount = 0;
        lastObjectSpawned = null;
        currentHorizontalVariance = startingHorizontalVariance;

        SpawnBanana();

        while (cameraTransform.position.y + preSpawnDistance >= lastObjectSpawned.position.y)
        {
            SpawnBanana();
        }
    }

    public void CheckStateAndStartSpawner()
    {
        if (GameManager.Instance.CurrentState != GameManager.State.Playing)
        {
            isActive = false;
            hasInitialized = false;
        }
        else
        {
            if (!hasInitialized) 
            {
                Initialize();
                hasInitialized = true;
            }
            isActive = true;
        }
    }
}
