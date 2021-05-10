using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCombo : MonoBehaviour
{
    GameObject m_comboTextPrefab;
    GameObject m_mainCanvas;

    private void Start()
    {
        m_comboTextPrefab = Resources.Load("Prefabs/Combo") as GameObject;
        m_mainCanvas = GameObject.Find("MainCanvas");
    }
    private void Update()
    {
        if (gameObject.GetComponent<Text>().color.a < 0.3f)
            Destroy(gameObject);
    }
    public void RequestComboText(int i)
    {
        GameObject newComboPrefab = Instantiate(m_comboTextPrefab, m_mainCanvas.transform);
        newComboPrefab.GetComponent<Text>().text = "Combo : " + i;
    }
}
