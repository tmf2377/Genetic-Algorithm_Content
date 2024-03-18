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

    void Start()
    {
        Time.timeScale = 0;
        startPanel.SetActive(true);
        playPanel.SetActive(false);
        clearPanel.SetActive(false);
        etcPanels.SetActive(false);
        isEtcOn = false;
    }

    public void ProjStart()
    {
        Time.timeScale = 1;
        startPanel.SetActive(false);
        playPanel.SetActive(true);

        /*
        // ���� ���� �� ȣ��Ǵ� �Լ�
        int encodingLength = ballGenerator.stringLength;
        string filePath = Application.dataPath + "/../EncodingLength_" + encodingLength + ".txt";

        Debug.Log("File saved to: " + filePath);

        if (!File.Exists(filePath))
        {
            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.WriteLine("Encoding Length: " + encodingLength);
                // �ʿ��� �⺻ ������ ���⿡ �߰��մϴ�.
            }
        }*/
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
}
