using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnParticleSystem : MonoBehaviour
{
    [SerializeField] string PoolTag;
    ParticleSystem particleSystem;

    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        particleSystem.Clear();
        particleSystem.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!particleSystem.IsAlive())
        {
            ObjectPoolManager.Instance.DespawnObject(PoolTag, this.gameObject);
        }
    }

}
