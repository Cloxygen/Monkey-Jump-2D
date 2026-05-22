using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.LoopSound("AmbientSound", audioSource);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAudioSourceVolume()
    {
        audioSource.volume = VolumeController.Instance.Volume;
    }
}
