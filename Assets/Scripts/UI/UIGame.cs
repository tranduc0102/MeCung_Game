using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGame : Singleton<UIGame>
{
    public Button movePlayer;
    public GameObject notification;
    public TextMeshProUGUI txtLive;
    [Header("--------Losse---------")]
    public GameObject panelLosse;
    public Button rePlay;
    public Button home;

    [Header("Notification")] public TextMeshProUGUI txtNoti;
    [Header("Notification")] public TextMeshProUGUI txtLevel;
    void Start()
    {
        movePlayer.onClick.AddListener(()=>EventDispatcher.Instance.PostEvent(EventID.OnPlayerMove));
        rePlay.onClick.AddListener(()=>Replay());
        home.onClick.AddListener(()=>Home());
        
        EventDispatcher.Instance.RegisterListener(EventID.OnNotification,param=>TextWhenNoPath());
        EventDispatcher.Instance.RegisterListener(EventID.UpdateLive,param => UpdateLive());
        EventDispatcher.Instance.RegisterListener(EventID.OnColliderWithEnemy, param => TextWhenColliderEnemy());
    }

    private void UpdateLive()
    {
        txtLive.text = (Convert.ToInt32(txtLive.text) -1).ToString();
        int lives = Convert.ToInt32(txtLive.text);
        if (lives <= 0)
        {
            panelLosse.SetActive(true);
            GameManager.Instance.isPlay = false;
        }
    }

    private void TextWhenNoPath()
    {
        notification.GetComponent<Animator>().SetTrigger("Notifi");
        txtNoti.text = "Không có đường để đi đến đích";
    }

    private void TextWhenColliderEnemy()
    {
        notification.GetComponent<Animator>().SetTrigger("Notifi");
        txtNoti.text = "Bạn đã bị tên trộm đánh về vị trí ban đầu";
    }

    public void TxtNotiLevel(string level)
    {
        txtLevel.text = level;
        txtLevel.alpha = 1f;
        StartCoroutine(FadeOutText());
    }
    private IEnumerator FadeOutText()
    {
        float duration = 2f;
        float startAlpha = txtLevel.alpha;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            txtLevel.alpha = Mathf.Lerp(startAlpha, 0f, normalizedTime);
            yield return null;
        }

        txtLevel.alpha = 0f;
    }
    private void Replay()
    {
        MusicManager.Instance.PlaySFX(MusicManager.Instance.musicClickButton);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    private void Home()
    {
        MusicManager.Instance.PlaySFX(MusicManager.Instance.musicClickButton);
        SceneManager.LoadSceneAsync("Lobby");
    }

}
