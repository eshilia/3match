using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour 
{
    [SerializeField] public int m_name;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Vector3 punchScale = new Vector3(0.1f, 0.1f, 0.1f);

        transform.DOPunchScale(punchScale, 1f);
    }


}
