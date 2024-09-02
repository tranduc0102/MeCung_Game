using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialSteps; // Các bước hướng dẫn
    private int check = -1;
    private int currentStepIndex = 0;

    void Start()
    {
        //Kiểm tra có phải level 1 không vì hướng dẫn này chỉ có ở level 1
        check = PlayerPrefs.GetInt(GameManager.Instance.session.nameSession, 0);
        if (check == 0)
        {
            ShowStep(currentStepIndex);
            // Gán sự kiện cho nút bấm  
        }
        else
        {
            EndTutorial();
        }
    }

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0 ) && check == 0)
        {
            OnClicked();
        }
    }

    void ShowStep(int stepIndex)
    {
        // Ẩn tất cả các bước hướng dẫn trước khi hiển thị bước hiện tại
        foreach (GameObject step in tutorialSteps)
        {
            step.SetActive(false);
        }

        // Hiển thị bước hiện tại
        if (stepIndex < tutorialSteps.Length)
        {
            tutorialSteps[stepIndex].SetActive(true);
        }
    }

    void OnClicked()
    {
        currentStepIndex++;

        if (currentStepIndex >= tutorialSteps.Length)
        {
            // Khi đã hoàn thành tất cả các bước hướng dẫn
            EndTutorial();
        }
        else
        {
            // Điều chỉnh cường độ ánh sáng theo bước hướng dẫn

            // Chuyển sang bước hướng dẫn tiếp theo
            ShowStep(currentStepIndex);
        }
    }

    void EndTutorial()
    {
        // Kết thúc hướng dẫn và bắt đầu gameplay
        foreach (GameObject step in tutorialSteps)
        {
            step.SetActive(false);
        }

        gameObject.GetComponent<Light2D>().intensity = 1f;
        GameManager.Instance.isPlay = true;
    }
}
