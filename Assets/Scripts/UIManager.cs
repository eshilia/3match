using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static UIManager s_instance;
    public static UIManager instance { get { return s_instance; } }

    PanelScore m_panelScore;
    PanelCombo m_panelCombo;

    

    private void Awake()
    {
        s_instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        m_panelScore = GameObject.Find("ScoreText").GetComponent<PanelScore>();
        m_panelCombo = GameObject.Find("Combo").GetComponent<PanelCombo>();

    }

    public void RequestScoreText(int i)
    {
        m_panelScore.GetComponent<Text>().text = "Score : " + i * 100;
    }

    public void RequestComboText(int i)
    {
        m_panelCombo.RequestComboText(i);
    }
}
