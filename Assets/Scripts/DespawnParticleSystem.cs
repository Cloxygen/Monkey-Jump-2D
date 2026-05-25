using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DespawnParticleSystem : MonoBehaviour
{
    // Serialized because this is a generic script shared by multiple particle systems (BananaExplode, CloudExplode).
    // Distinct from specific scripts like Banana.cs which only live on a single prefab and can hardcode their tag.
    [Tooltip("The tag of the object pool to return this particle system to when it stops.")]
    [FormerlySerializedAs("PoolTag")]
    [SerializeField] string poolTag;

    ParticleSystem particles;
    WaitForSeconds waitLifetime;

    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        
        float totalLifetime = particles.main.duration + particles.main.startLifetime.constantMax;
        waitLifetime = new WaitForSeconds(totalLifetime);
    }

    void OnEnable()
    {
        particles.Clear();
        particles.Play();
        StartCoroutine(WaitAndDespawn());
    }

    IEnumerator WaitAndDespawn()
    {
        yield return waitLifetime;

        ObjectPoolManager.Instance.DespawnObject(poolTag, gameObject);
    }
}
