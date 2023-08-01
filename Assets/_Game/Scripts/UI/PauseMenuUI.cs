using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] GameObject pauseOverlay;
    public void Pause()
    {
        pauseOverlay.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        pauseOverlay.SetActive(false);
    }
    public void MainMenu()
    {
        AudioManager.Instance.Stop(SoundEffect.TRACK1);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
