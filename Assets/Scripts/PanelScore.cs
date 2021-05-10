using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelScore : MonoBehaviour
{
    GameObject m_scoreTextPrefab;
    private void Start()
    {
        m_scoreTextPrefab = Resources.Load("Prefabs/ScoreText") as GameObject;
    }
    public void RequestScoreText(int i)
    {
        GameObject newObject = Instantiate(m_scoreTextPrefab, transform);
        newObject.GetComponent<Text>().text = "Score" + i * 100;
        
    }
}
