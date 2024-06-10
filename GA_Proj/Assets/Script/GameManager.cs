using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public BallGenerator ballGenerator;

    public GameObject startPanel;
    public GameObject playPanel;
    public GameObject clearPanel;

    public GameObject etcPanels;
    bool isEtcOn;

    public GameObject explainPanel1;
    public GameObject explainPanel2;
    public GameObject explainPanel3;
    public GameObject explainPanel4;
    public GameObject explainPanel5;
    public GameObject explainPanel6;

    void Start()
    {
        Time.timeScale = 0;
        startPanel.SetActive(true);
        playPanel.SetActive(false);
        clearPanel.SetActive(false);
        etcPanels.SetActive(false);
        isEtcOn = false;

        explainPanel1.SetActive(false);
        explainPanel2.SetActive(false);
        explainPanel3.SetActive(false);
        explainPanel4.SetActive(false);
        explainPanel5.SetActive(false);
        explainPanel6.SetActive(false);
    }

    public void ProjStart()
    {
        Time.timeScale = 1;
        startPanel.SetActive(false);
        playPanel.SetActive(true);

    }

    public void EtcPanels()
    {
        if (isEtcOn)
        {
            isEtcOn = false;
            etcPanels.SetActive(false);
        }
        else
        {
            isEtcOn = true;
            etcPanels.SetActive(true);
        }
    }

    public void Continue()
    {
        playPanel.SetActive(true);
        clearPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    //------------------------------------------------------------------

    public void Explain_1()
    {
        // 패널의 현재 활성화 상태를 체크하고 상태를 반대로 변경
        explainPanel1.SetActive(!explainPanel1.activeSelf);
    }

    public void Explain_2()
    {
        explainPanel2.SetActive(!explainPanel2.activeSelf);
    }

    public void Explain_3()
    {
        explainPanel3.SetActive(!explainPanel3.activeSelf);
    }

    public void Explain_4()
    {
        explainPanel4.SetActive(!explainPanel4.activeSelf);
    }

    public void Explain_5()
    {
        explainPanel5.SetActive(!explainPanel5.activeSelf);
    }

    public void Explain_6()
    {
        explainPanel6.SetActive(!explainPanel6.activeSelf);
    }
}
