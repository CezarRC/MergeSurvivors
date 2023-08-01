using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float score { get; private set; } = 0;
    public float timeSurvived { get; private set; } = 0;

    float startTime = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }
    public void StartGame()
    {
        score = 0;
        timeSurvived = 0;
        Time.timeScale = 1;
        startTime = Time.time;
        AudioManager.Instance.Play(SoundEffect.TRACK1, true);
        StartCoroutine(WinGame());
    }
    IEnumerator WinGame()
    {
        yield return new WaitForSeconds(300);
        AudioManager.Instance.Stop(SoundEffect.TRACK1);
        AudioManager.Instance.Play(SoundEffect.WINEFFECT);
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.Play(SoundEffect.WINGAME);
        Time.timeScale = 0f;
        GameOverUI goUI = GameObject.FindObjectOfType<GameOverUI>(true);
        goUI.gameObject.SetActive(true);
        goUI.UpdateUI(true, (int)score, (int)timeSurvived);
    }
    public void AddScore(int amount)
    {
        score += amount;
    }
    public void GameOver()
    {
        StopCoroutine(WinGame());
        AudioManager.Instance.Stop(SoundEffect.TRACK1);
        timeSurvived = Time.time - startTime;
        Time.timeScale = 0;
        GameOverUI goUI = GameObject.FindObjectOfType<GameOverUI>(true);
        goUI.gameObject.SetActive(true);
        goUI.UpdateUI(false, (int)score, (int)timeSurvived);
        AudioManager.Instance.Play(SoundEffect.GAMEOVER);
    }
    public void Retry()
    {
        AudioManager.Instance.Stop(SoundEffect.MENUTRACK);
        SceneManager.LoadScene("Game");
    }
}
