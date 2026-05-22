using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] Collider2D collider;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float flySpeed = 1f;
    [SerializeField] float fadeTime = .5f;
    bool isMovingRight = true;
    bool isBounced = false;
    Vector3 defaultScale;

    // Start is called before the first frame update
    void Awake()
    {
        defaultScale = transform.localScale;
        collider.isTrigger = true;
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

    // Update is called once per frame
    void Update()
    {
        if (!isBounced)
        {
            MoveHorizontal();
        }
    }

    void MoveHorizontal()
    {
        if (isMovingRight)
        {
            MoveRight();
        }
        if (!isMovingRight)
        {
            MoveLeft();
        }
    }

    void MoveRight()
    {
        Vector3 newPosition = transform.position;
        newPosition.x += flySpeed * Time.deltaTime;
        transform.position = newPosition;
    }

    void MoveLeft()
    {
        Vector3 newPosition = transform.position;
        newPosition.x -= flySpeed * Time.deltaTime;
        transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            this.collider.enabled = false;
            playerMovement.Bounce();
            ScoreManager.Instance.DoubleScore();
            StartCoroutine(BouncedAnimation());
            ObjectPoolManager.Instance.SpawnObject("CloudExplode", transform.position);
            ObjectPoolManager.Instance.SpawnObject("FloatingTextCloud", transform.position);
            isBounced = true;
            SoundManager.Instance.PlaySound("CloudBounce");
        }
        else if (other.CompareTag("LowerBounds"))
        {

            ObjectPoolManager.Instance.DespawnObject("Cloud", this.gameObject);
            ResetObject();
        }
        else
            SetDirection(!isMovingRight);
    }

    IEnumerator BouncedAnimation()
    {
        Color color = spriteRenderer.color;
        float newAlpha = 1f;
        Vector2 newPosition = transform.position;
        while (color.a > 0f)
        {
            newAlpha -= Time.deltaTime / fadeTime;
            color.a = newAlpha;
            spriteRenderer.color = color;

            yield return null;
        }



        ObjectPoolManager.Instance.DespawnObject("Cloud", this.gameObject);
        ResetObject();
    }

    public void ResetObject()
    {
        this.collider.enabled = true;

        Color color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;

        isBounced = false;
    }

    public void SetDirection(bool isMovingRight)
    {
        this.isMovingRight = isMovingRight;
        if (isMovingRight) 
        {
            transform.localScale = defaultScale;
        }
        else
        {
            transform.localScale = new Vector3(-defaultScale.x, defaultScale.y, defaultScale.z);
        }
    }

    void Despawn()
    {
        ObjectPoolManager.Instance.DespawnObject("Cloud", this.gameObject);
        ResetObject();
    }

    public void DespawnOnGameOver()
    {
        if (GameManager.Instance.CurrentState != GameManager.State.Playing)
            Despawn();
    }
}
