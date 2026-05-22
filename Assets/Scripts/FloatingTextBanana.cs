using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextBanana : MonoBehaviour
{
    TextMeshPro textMeshPro;
    [SerializeField] float fadeTime = 1f;

    private void OnEnable()
    {
        textMeshPro.text = ScoreManager.Instance.PointsPerBanana.ToString();
        StartCoroutine(FadeOut());
    }

    void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        textMeshPro.text = ScoreManager.Instance.PointsPerBanana.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeOut()
    {
        Color color = textMeshPro.color;
        float newAlpha = 1f;
        while (color.a > 0f)
        {
            newAlpha -= Time.deltaTime / fadeTime;
            color.a = newAlpha;
            textMeshPro.color = color;

            yield return null;
        }

        ObjectPoolManager.Instance.DespawnObject("FloatingTextBanana", this.gameObject);
        ResetObject();
    }

    public void ResetObject()
    {
        Color color = textMeshPro.color;
        color.a = 1f;
        textMeshPro.color = color;
    }
}
