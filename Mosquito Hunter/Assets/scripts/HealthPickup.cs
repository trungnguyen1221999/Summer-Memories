using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Player nhặt
        {
            // Tìm Children (có thể gắn Tag cho Children là "children")
            ChildrenHealthManager children = FindObjectOfType<ChildrenHealthManager>();
            if (children != null)
            {
                children.Heal(1); // Hồi 1 máu
            }

            Destroy(gameObject); // Xóa vật phẩm
        }
    }
}
