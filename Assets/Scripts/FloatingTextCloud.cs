using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextCloud : MonoBehaviour
{
    TextMeshPro textMesh;
    [SerializeField] float fadeTime = 1f;
    [SerializeField] string text = "x2";
    [SerializeField] Vector3 moveDirection = Vector3.zero;
    [SerializeField] float speed = 2f;
    [SerializeField] float rotationSpeed = 180f;

    private void OnEnable()
    {
        StartCoroutine(FadeOut());
    }

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        textMesh.text = text;
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
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

        ObjectPoolManager.Instance.DespawnObject("FloatingTextCloud", gameObject);
        ResetObject();
    }

    public void ResetObject()
    {
        Color color = textMesh.color;
        color.a = 1f;
        textMesh.color = color;
    }
}
