using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager s_instance;
    public static UIManager instance { get { return s_instance; } }

    PanelScore m_panelScore;

    private void Awake()
    {
        s_instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        m_panelScore = GameObject.Find("PanelScore").GetComponent<PanelScore>();
        m_panelScore.enabled = true;

    }

    public void RequestScoreText(string str)
    {
        m_panelScore.RequestScoreText(str);
    }
}
