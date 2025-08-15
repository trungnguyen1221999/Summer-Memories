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

    private bool reachedTargetX = false; // Đã đi ngang đến x = -7.5 chưa
    private float targetX = -7.5f;
    private Vector3 randomOffset;
    private float startTime;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isDead = false;

    // Giới hạn Y để đổi hướng
    public float yMin = 0.25f;
    public float yMax = 4.5f;
    private int yDirection = 1; // 1 = đi lên, -1 = đi xuống

    void Start()
    {
        startTime = Time.time;
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        // Bước 1: Di chuyển ngang về targetX
        if (!reachedTargetX)
        {
            MoveHorizontalToTarget();
        }
        else
        {
            // Bước 2: Di chuyển đặc trưng từng loại enemy
            switch (enemyType)
            {
                case EnemyType.Enemy1: MoveRandomSlow(); break;
                case EnemyType.Enemy2: MoveZigZag(); break;
                case EnemyType.Enemy3: MoveSin(); break;
                case EnemyType.Boss: MoveStraight(); break;
            }
        }
    }

    void MoveHorizontalToTarget()
    {
        Vector3 targetPos = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - targetX) < 0.01f)
            reachedTargetX = true;
    }

    void MoveRandomSlow()
    {
        Vector3 adjustedTarget = new Vector3(targetX, transform.position.y, transform.position.z) + randomOffset;
        transform.position = Vector3.MoveTowards(transform.position, adjustedTarget, speed * Time.deltaTime);
        CheckYBounds();
    }

    void MoveZigZag()
    {
        float zigzag = Mathf.Sin((Time.time - startTime) * zigZagFrequency) * zigZagAmplitude;
        Vector3 offset = new Vector3(0f, zigzag * yDirection, 0f);
        Vector3 direction = (new Vector3(transform.position.x, transform.position.y, 0f) + offset - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        CheckYBounds();
    }

    void MoveSin()
    {
        float t = Time.time - startTime;
        float sinY = Mathf.Sin(t * sinFrequency) * sinAmplitude * yDirection;
        transform.position += new Vector3(0f, sinY, 0f) * Time.deltaTime;
        CheckYBounds();
    }

    void MoveStraight()
    {
        transform.position += Vector3.down * speed * Time.deltaTime; // đi thẳng xuống
        CheckYBounds();
    }

    // Kiểm tra yMin/yMax, đổi hướng
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
            {
                healthManager.TakeDamage();
            }
            Destroy(gameObject);
        }

        if (collision.CompareTag("attack"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            StartCoroutine(DieEffect());
        }
        else
        {
            if (spriteRenderer != null)
                StartCoroutine(FlashRed());
        }
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

        Destroy(gameObject);
    }
}
