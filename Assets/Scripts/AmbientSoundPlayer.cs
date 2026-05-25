using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        SoundManager.Instance.LoopSound("AmbientSound", audioSource);
    }

    public void UpdateAudioSourceVolume()
    {
        audioSource.volume = VolumeController.Instance.Volume;
    }
}
