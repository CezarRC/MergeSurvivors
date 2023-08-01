using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text matchInfoText;
    [SerializeField] TMPro.TMP_Text UILabel;
    public void UpdateUI(bool victory, int score, int timeSurvived)
    {
        System.TimeSpan ts = System.TimeSpan.FromSeconds(timeSurvived);
        string time = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
        UILabel.text = victory ? "CONGRATULATIONS!" : "GAME OVER";
        matchInfoText.text = $"SCORE: {score}\nTIME SURVIVED: {time}";
    }
    public void ResetGame()
    {
        AudioManager.Instance.Play(SoundEffect.CONFIRMBUTTON);
        GameManager.Instance.Retry();
    }
    public void MainMenu()
    {
        AudioManager.Instance.Stop(SoundEffect.GAMEOVER);
        AudioManager.Instance.Stop(SoundEffect.WINGAME);
        SceneManager.LoadScene("MainMenu");
    }
}
