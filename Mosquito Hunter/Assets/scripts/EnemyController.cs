using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public enum EnemyType { Enemy1, Enemy2, Enemy3, Boss }
    public EnemyType enemyType;

    public float speed = 2f;
    public float zigZagAmplitude = 1f;
    public float zigZagFrequency = 2f;
    public float sinAmplitude = 1f;
    public float sinFrequency = 3f;
    public int health;

    private Vector3 randomOffset;
    private float startTime;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isDead = false;

    public float yMin = 0.25f;
    public float yMax = 4.5f;
    private int yDirection = 1;
    public float ySpeed = 1f;

    public float approachX = -7.5f;
    public Vector3 finalTarget = new Vector3(7f, -1.5f, 0f);

    [Header("Drop Settings")]
    public GameObject healthPrefab;   // Prefab máu hồi
    [Range(0f, 1f)]
    public float dropChance = 0.2f;   // Tỉ lệ rơi (20%)

    [Header("Audio Settings")]
    public AudioClip deathSFX;        // Âm thanh khi chết
    private AudioSource audioSource;

    void Start()
    {
        startTime = Time.time;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        switch (enemyType)
        {
            case EnemyType.Enemy1:
                speed = 2f;
                health = 1;
                randomOffset = new Vector3(Random.Range(-2f, 2f), Random.Range(-1f, 1f), 0f);
                break;
            case EnemyType.Enemy2:
                speed = 4f;
                health = 2;
                break;
            case EnemyType.Enemy3:
                speed = 6f;
                health = 3;
                break;
            case EnemyType.Boss:
                speed = 3f;
                health = 5;
                break;
        }
    }

    void Update()
    {
        if (isDead) return;

        if (transform.position.x < approachX)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            Vector3 move = Vector3.zero;

            switch (enemyType)
            {
                case EnemyType.Enemy1:
                    move += Vector3.up * yDirection * ySpeed * Time.deltaTime;
                    break;

                case EnemyType.Enemy2:
                    float zigzag = Mathf.Sin((Time.time - startTime) * zigZagFrequency) * zigZagAmplitude;
                    move += Vector3.up * (zigzag * Time.deltaTime + yDirection * ySpeed * Time.deltaTime);
                    break;

                case EnemyType.Enemy3:
                    move += Vector3.zero;
                    break;

                case EnemyType.Boss:
                    move += Vector3.down * speed * Time.deltaTime;
                    break;
            }

            move += Vector3.right * speed * Time.deltaTime;
            transform.position += move;

            if (enemyType != EnemyType.Enemy3)
                CheckYBounds();
        }
    }

    void CheckYBounds()
    {
        if (transform.position.y >= yMax)
            yDirection = -1;
        else if (transform.position.y <= yMin)
            yDirection = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("children"))
        {
            ChildrenHealthManager healthManager = collision.GetComponent<ChildrenHealthManager>();
            if (healthManager != null)
                healthManager.TakeDamage();

            Destroy(gameObject);
        }

        if (collision.CompareTag("attack"))
            TakeDamage(1);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !isDead)
            StartCoroutine(DieEffect());
        else if (spriteRenderer != null)
            StartCoroutine(FlashRed());
    }

    private IEnumerator FlashRed()
    {
        float flashDuration = 0.5f;
        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.2f;
        }
        spriteRenderer.color = originalColor;
    }

    private IEnumerator DieEffect()
    {
        isDead = true;

        // Phát SFX khi chết
        if (audioSource != null && deathSFX != null)
            audioSource.PlayOneShot(deathSFX);

        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 originalPos = transform.position;

        while (elapsed < duration)
        {
            spriteRenderer.color = Color.black;
            transform.position = originalPos + (Vector3)(Random.insideUnitCircle * 0.1f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Xác suất rơi healthPrefab
        if (healthPrefab != null && Random.value < dropChance)
        {
            Instantiate(healthPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
