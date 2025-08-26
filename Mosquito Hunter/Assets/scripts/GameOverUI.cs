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
        Time.timeScale = 1f; // Bỏ pause (nếu không muốn thì có thể bỏ dòng này)
        Application.Quit();
        Debug.Log("Thoát game! (sẽ chỉ hoạt động sau khi build)");
    }
}
