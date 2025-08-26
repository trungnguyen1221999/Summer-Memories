using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [Header("BGM Settings")]
    public AudioClip bgmClip;       // Kéo nhạc nền vào đây
    [Range(0f, 1f)]
    public float volume = 0.5f;     // Âm lượng

    private AudioSource audioSource;

    void Awake()
    {
        // Kiểm tra nếu đã có bản nhạc nào chạy, không tạo bản mới
        if (FindObjectsOfType<BackgroundMusic>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        // Giữ BGM khi load scene mới


        // Thiết lập AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = bgmClip;
        audioSource.loop = true;       // Lặp nhạc liên tục
        audioSource.volume = volume;
        audioSource.playOnAwake = false;

        // Phát nhạc
        audioSource.Play();
    }

    // Hàm để bật/tắt nhạc từ các UI hoặc script khác
    public void SetBGMActive(bool isActive)
    {
        if (audioSource == null) return;

        if (isActive)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }

    // Hàm để điều chỉnh âm lượng runtime
    public void SetVolume(float newVolume)
    {
        if (audioSource != null)
            audioSource.volume = Mathf.Clamp01(newVolume);
    }
}
