using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float xmin = -9f;
    public float xmax = 9.6f;
    public float ymin = -3.5f;
    public float ymax = 3.74f;

    public float moveSpeed = 10f;

    private float originalRotationX;
    private float originalRotationY;
    private float originalRotationZ;
    private bool isRotating = false;

    public GameObject attackHitbox;

    private Vector3 targetPosition;
    private bool hasTarget = false;

    void Start()
    {
        Cursor.visible = false;

        Vector3 startRot = transform.rotation.eulerAngles;
        originalRotationX = startRot.x;
        originalRotationY = startRot.y;
        originalRotationZ = startRot.z;

        if (attackHitbox != null)
            attackHitbox.SetActive(false);

        targetPosition = transform.position;
        hasTarget = true;
    }

    void Update()
    {
        HandleInput();
        MoveToTarget();
    }

    void HandleInput()
    {
        Vector3 inputPos = Vector3.zero;
        bool inputDetected = false;

#if UNITY_EDITOR || UNITY_STANDALONE
        // PC: Dùng chuột để điều khiển
        inputPos = Input.mousePosition;
        inputDetected = true;

        // Click chuột trái để attack
        if (Input.GetMouseButtonDown(0) && !isRotating)
        {
            StartCoroutine(AttackAction(45f, 0.3f));
        }

#elif UNITY_ANDROID
        // Android: Tap để di chuyển
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                inputPos = touch.position;
                inputDetected = true;
            }
        }
#endif

        if (inputDetected)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(inputPos);
            worldPos.z = transform.position.z;

            float clampedX = Mathf.Clamp(worldPos.x, xmin, xmax);
            float clampedY = Mathf.Clamp(worldPos.y, ymin, ymax);
            Vector3 tappedPos = new Vector3(clampedX, clampedY, worldPos.z);

#if UNITY_ANDROID
            // Nếu đã tới gần vị trí đó rồi => thực hiện tấn công
            if (Vector3.Distance(transform.position, targetPosition) < 0.2f &&
                Vector3.Distance(tappedPos, targetPosition) < 0.5f &&
                !isRotating)
            {
                StartCoroutine(AttackAction(45f, 0.3f));
            }
            else
            {
                // Chỉ cập nhật targetPosition nếu chưa ở đó
                targetPosition = tappedPos;
                hasTarget = true;
            }
#else
            // PC: luôn cập nhật target theo chuột
            targetPosition = tappedPos;
            hasTarget = true;
#endif
        }
    }

    void MoveToTarget()
    {
        if (hasTarget)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    System.Collections.IEnumerator AttackAction(float angle, float duration)
    {
        isRotating = true;

        if (attackHitbox != null)
            attackHitbox.SetActive(true);

        transform.rotation = Quaternion.Euler(originalRotationX, originalRotationY, originalRotationZ + angle);
        yield return new WaitForSeconds(duration);

        transform.rotation = Quaternion.Euler(originalRotationX, originalRotationY, originalRotationZ);

        if (attackHitbox != null)
            attackHitbox.SetActive(false);

        isRotating = false;
    }
}
