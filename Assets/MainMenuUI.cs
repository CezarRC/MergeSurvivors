using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }
    public void PlayGame()
    {
        AudioManager.Instance.Stop(SoundEffect.MENUTRACK);
        SceneManager.LoadScene("Game");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ChangeVolume(float v)
    {
        AudioManager.Instance.ChangeVolume(volumeSlider.value);
    }
}
