using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private List<SoundEntry> soundList = new List<SoundEntry>();
    public Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();
    private AudioSource oneShotAudioSource;

    void Awake()
    {
        SetInstance();
        PopulateSoundDictionary();
        oneShotAudioSource = gameObject.AddComponent<AudioSource>();
    }

    void SetInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Sound Manager already exists");
            Destroy(gameObject);
        }
    }

    private void PopulateSoundDictionary()
    {
        soundDictionary.Clear();

        foreach (var soundEntry in soundList)
        {
            if (!soundDictionary.ContainsKey(soundEntry.name))
            {
                soundDictionary.Add(soundEntry.name, soundEntry.clip);
            }
            else
            {
                Debug.LogWarning("Sound clip with name '" + soundEntry.name + "' already exists!");
            }
        }
    }

    public void PlaySound(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            if (oneShotAudioSource != null && VolumeController.Instance != null)
            {
                oneShotAudioSource.PlayOneShot(clip, VolumeController.Instance.Volume);
            }
        }
        else
        {
            Debug.LogWarning("Sound clip with name '" + soundName + "' not found!");
        }
    }

    public void LoopSound(string soundName, AudioSource audioSource)
    {
        if (soundDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            audioSource.loop = true;
            audioSource.clip = clip;
            audioSource.volume = VolumeController.Instance.Volume;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Sound clip with name '" + soundName + "' not found!");
        }
    }
}

[System.Serializable]
public class SoundEntry
{
    public string name;
    public AudioClip clip;
}
