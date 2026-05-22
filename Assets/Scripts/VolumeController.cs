using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VolumeController : MonoBehaviour
{
    public static VolumeController Instance;
    private float volume = .5f;
    public float Volume
    {
        get
        {
            return volume;
        }
        set 
        {
            volume = value;
            OnVolumeChanged?.Invoke();
        }
    }
    public UnityEvent OnVolumeChanged;
    [SerializeField] Slider slider;
    // Start is called before the first frame update
    void Awake()
    {
        SetInstance();
    }

    private void Start()
    {
        UpdateVolume();
        slider.value = volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetInstance()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.Log("Volume Controller already exists");
            Destroy(this.gameObject);
        }
    }

    public void UpdateVolume()
    {
        Volume = slider.value;
    }
}
