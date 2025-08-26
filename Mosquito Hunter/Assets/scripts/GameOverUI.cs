using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    // Gọi hàm này khi bấm nút Restart
    public void RestartGame()
    {
        Time.timeScale = 1f; // Bỏ pause
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Load lại scene hiện tại
    }

    // Gọi hàm này khi bấm nút Exit
    public void ExitGame()
    {
        Time.timeScale = 1f; // Bỏ pause (có thể bỏ nếu muốn)
        Application.Quit();
        Debug.Log("Thoát game! (sẽ chỉ hoạt động sau khi build)");
    }
    public void PlayGame()
    {
        Time.timeScale = 1f; // Đảm bảo game không bị pause
        SceneManager.LoadScene(1); // Load scene có index = 1
    }
}
