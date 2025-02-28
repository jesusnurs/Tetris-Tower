using UnityEngine;
using UnityEngine.UI;

public class MusicSettingsUI : MonoBehaviour
{
	[SerializeField] private Button soundsButton;
	[SerializeField] private Button musicButton;

    [SerializeField] private GameObject soundsCrossLine;
    [SerializeField] private GameObject musicCrossLine;

    private void Start()
    {
        soundsButton.onClick.AddListener(ToggleSounds);
        musicButton.onClick.AddListener(ToggleMusic);
    }

    private void ToggleSounds()
    {
        AudioManager.Instance.ToggleSoundsMute();
        soundsCrossLine.SetActive(AudioManager.Instance.IsSoundMuted());
    }
    
    private void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusicMute();
        musicCrossLine.SetActive(AudioManager.Instance.IsMusicMuted());
    }
}
