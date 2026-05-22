using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextCloud : MonoBehaviour
{
    TextMeshPro textMeshPro;
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
        textMeshPro = GetComponent<TextMeshPro>();
        textMeshPro.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
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

        ObjectPoolManager.Instance.DespawnObject("FloatingTextCloud", this.gameObject);
        ResetObject();
    }

    public void ResetObject()
    {
        Color color = textMeshPro.color;
        color.a = 1f;
        textMeshPro.color = color;
    }
}
