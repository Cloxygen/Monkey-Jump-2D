using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextBanana : MonoBehaviour
{
    TextMeshPro textMesh;
    [SerializeField] float fadeTime = 1f;

    private void OnEnable()
    {
        textMesh.text = ScoreManager.Instance.PointsPerBanana.ToString();
        StartCoroutine(FadeOut());
    }

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        textMesh.text = ScoreManager.Instance.PointsPerBanana.ToString();
    }

    IEnumerator FadeOut()
    {
        Color color = textMesh.color;
        float newAlpha = 1f;
        while (color.a > 0f)
        {
            newAlpha -= Time.deltaTime / fadeTime;
            color.a = newAlpha;
            textMesh.color = color;

            yield return null;
        }

        ObjectPoolManager.Instance.DespawnObject("FloatingTextBanana", gameObject);
        ResetObject();
    }

    public void ResetObject()
    {
        Color color = textMesh.color;
        color.a = 1f;
        textMesh.color = color;
    }
}
