using UnityEngine;
using System.Collections;

public class ChildrenHealthManager : MonoBehaviour
{
    public int health = 5; // Tổng máu
    public Sprite[] damageSprites; // 4 sprite biểu hiện trạng thái (0 = bình thường, 3 = gần hỏng)
    private SpriteRenderer spriteRenderer;
    private Vector3 originalPos;
    public bool isGameOver = false;

    [Header("Game Over Settings")]
    public GameObject gameOverPanel; // Kéo GameOverPanel vào đây trong Inspector

    [Header("Audio Settings")]
    public AudioClip damageSFX;   // Âm khi mất máu
    public AudioClip gameOverSFX; // Âm khi GameOver
    private AudioSource audioSource;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalPos = transform.position; // Lưu vị trí ban đầu

        audioSource = GetComponent<AudioSource>();

        // Đảm bảo sprite ban đầu là sprite bình thường
        if (damageSprites.Length > 0)
            spriteRenderer.sprite = damageSprites[0];

        // Ẩn GameOver panel ban đầu
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void TakeDamage()
    {
        if (health <= 0) return;

        health--;

        // Phát SFX mất máu
        if (audioSource != null && damageSFX != null)
            audioSource.PlayOneShot(damageSFX);

        // Đổi sprite dựa trên lượng máu còn lại
        int spriteIndex = Mathf.Clamp(5 - health, 0, damageSprites.Length - 1);
        spriteRenderer.sprite = damageSprites[spriteIndex];

        // Hiệu ứng rung
        StartCoroutine(ScreenShake());

        // Khi hết máu thì game over
        if (health <= 0)
        {
            Debug.Log("GAME OVER!");
            GameOver();
            isGameOver = true;
        }
    }

    private void GameOver()
    {
        // Hiện GameOver panel
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Phát SFX GameOver
        if (audioSource != null && gameOverSFX != null)
            audioSource.PlayOneShot(gameOverSFX);

        // Hiện con trỏ chuột
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Dừng toàn bộ game
        Time.timeScale = 0f;
    }

    private IEnumerator ScreenShake()
    {
        if (!isGameOver)
        {
            float duration = 0.3f; // thời gian rung
            float elapsed = 0f;
            float magnitude = 0.2f; // độ mạnh rung

            while (elapsed < duration)
            {
                Vector3 randomOffset = (Vector3)(Random.insideUnitCircle * magnitude);
                transform.position = originalPos + randomOffset;

                elapsed += Time.unscaledDeltaTime; // dùng unscaledDeltaTime để vẫn rung khi Time.timeScale = 0
                yield return null;
            }

            // Trả về vị trí ban đầu
            transform.position = originalPos;
        }
    }

    public void Heal(int amount)
    {
        if (health <= 0) return; // Đã chết thì không hồi

        health = Mathf.Min(health + amount, 5); // không vượt quá 5 máu

        // Cập nhật sprite dựa trên máu hiện tại
        int spriteIndex = Mathf.Clamp(5 - health, 0, damageSprites.Length - 1);
        spriteRenderer.sprite = damageSprites[spriteIndex];
    }
}
