using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Banana : MonoBehaviour
{
    [SerializeField] Collider2D bananaCollider;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float fallSpeed = .1f;
    [SerializeField] float fadeTime = .5f;
    [SerializeField] float bouncedFallSpeed = 1f;
    Vector3 defaultScale;

    void Awake()
    {
        defaultScale = transform.localScale;
        bananaCollider.isTrigger = true;
    }

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged.AddListener(DespawnOnGameOver);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged.RemoveListener(DespawnOnGameOver);
        }
    }

    void Update()
    {
        Fall();
    }

    void Fall()
    {
        Vector3 newPosition = transform.position;
        newPosition.y -= fallSpeed * Time.deltaTime;
        transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement playerMovement))
        {
            BouncePlayer(playerMovement);
        }
        else if (collision.CompareTag("BananaFloor"))
        {
            HandleFloorCollision();
        }
        else if (collision.CompareTag("LowerBounds"))
        {
            Despawn();
        }
    }

    private void BouncePlayer(PlayerMovement playerMovement)
    {
        bananaCollider.enabled = false;
        playerMovement.Bounce();
        StartCoroutine(BouncedAnimation());

        ObjectPoolManager.Instance.SpawnObject("BananaExplode", transform.position);
        ObjectPoolManager.Instance.SpawnObject("FloatingTextBanana", transform.position);
        SoundManager.Instance.PlaySound("BananaBounce");
        ScoreManager.Instance.IncrementScore();
    }

    private void HandleFloorCollision()
    {
        bananaCollider.enabled = false;
        StartCoroutine(FloorFadeAnimation());
    }
    
    IEnumerator BouncedAnimation()
    {
        Color color = spriteRenderer.color;
        float newAlpha = 1f;
        Vector2 newPosition = transform.position;
        while(color.a > 0f)
        {
            newAlpha -= Time.deltaTime / fadeTime;
            color.a = newAlpha;
            spriteRenderer.color = color;


            newPosition.y -= Time.deltaTime * bouncedFallSpeed;
            transform.position = newPosition;
            yield return null;
        }

        Despawn();
    }

    public void ResetObject()
    {
        bananaCollider.enabled = true;

        Color color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;

        transform.localScale = defaultScale;
    }

    void Despawn()
    {
        ResetObject();
        ObjectPoolManager.Instance.DespawnObject("Banana", this.gameObject);
    }

    public void DespawnOnGameOver()
    {
        if (GameManager.Instance.CurrentState != GameManager.State.Playing && this.isActiveAndEnabled)
            Despawn();
    }


    IEnumerator FloorFadeAnimation()
    {
        Color color = spriteRenderer.color;
        float newAlpha = 1f;
        while (color.a > 0f)
        {
            newAlpha -= Time.deltaTime / fadeTime;
            color.a = newAlpha;
            spriteRenderer.color = color;

            yield return null;
        }

        Despawn();
    }

}