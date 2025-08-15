using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Giới hạn di chuyển
    public float xmin = -9f;
    public float xmax = 9.6f;
    public float ymin = -3.5f;
    public float ymax = 3.74f;

    public float moveSpeed = 10f; // tốc độ lerp

    private float originalRotationX;
    private float originalRotationY;
    private float originalRotationZ;
    private bool isRotating = false;

    public GameObject attackHitbox; // hitbox tấn công (là con của player)

    void Start()
    {
        // Ẩn con trỏ chuột
        Cursor.visible = false;

        // Lưu góc ban đầu
        Vector3 startRot = transform.rotation.eulerAngles;
        originalRotationX = startRot.x;
        originalRotationY = startRot.y;
        originalRotationZ = startRot.z;

        // Ban đầu ẩn hitbox
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }

    void Update()
    {
        MovePlayer();

        // Nhấn chuột trái => Attack
        if (Input.GetMouseButtonDown(0) && !isRotating)
        {
            StartCoroutine(AttackAction(45f, 0.3f));
        }
    }

    void MovePlayer()
    {
        // Lấy vị trí chuột
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;

        // Giới hạn
        float clampedX = Mathf.Clamp(mousePos.x, xmin, xmax);
        float clampedY = Mathf.Clamp(mousePos.y, ymin, ymax);
        Vector3 targetPos = new Vector3(clampedX, clampedY, mousePos.z);

        // Di chuyển mượt
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    System.Collections.IEnumerator AttackAction(float angle, float duration)
    {
        isRotating = true;

        // Bật hitbox tấn công
        if (attackHitbox != null)
            attackHitbox.SetActive(true);

        // Xoay sang +angle (giữ nguyên X, Y)
        transform.rotation = Quaternion.Euler(originalRotationX, originalRotationY, originalRotationZ + angle);
        yield return new WaitForSeconds(duration);

        // Trả về góc ban đầu (giữ nguyên X, Y)
        transform.rotation = Quaternion.Euler(originalRotationX, originalRotationY, originalRotationZ);

        // Ẩn hitbox tấn công
        if (attackHitbox != null)
            attackHitbox.SetActive(false);

        isRotating = false;
    }
}
