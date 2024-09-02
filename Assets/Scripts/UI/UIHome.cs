using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHome : MonoBehaviour
{
    public Button btnPlay;
    public Button btnExit;
    public GameObject panelSession;

    [Header("----------Panel Session--------")]
    public Button closeTag;

    public List<TextMeshProUGUI> txtSession;  // Sử dụng TextMeshProUGUI cho UI Text
    public List<Button> btnSession;

    void Start()
    {
        btnPlay.onClick.AddListener(() => SetActivePanelTrue());
        btnExit.onClick.AddListener(() =>test());
        closeTag.onClick.AddListener(() => SetActivePanelFalse());
        txtSession[0].text = $"{PlayerPrefs.GetInt("Session_1", 0)}/5";
        btnSession[0].onClick.AddListener(() => LoadScene("Session_1"));
    }

    private void SetActivePanelTrue()
    {
        MusicManager.Instance.PlaySFX(MusicManager.Instance.musicClickButton);
        panelSession.SetActive(true);
    }
    private void SetActivePanelFalse()
    {
        MusicManager.Instance.PlaySFX(MusicManager.Instance.musicClickButton);
        panelSession.SetActive(false);
    }

    private void LoadScene(string name)
    {
        MusicManager.Instance.PlaySFX(MusicManager.Instance.musicClickButton);
        SceneManager.LoadSceneAsync(name);
        MusicManager.Instance.PlayAudioSource(MusicManager.Instance.musicGame);
    }

    void test()
    {
        PlayerPrefs.SetInt("Session_1", 0);
        PlayerPrefs.Save();
    }
}