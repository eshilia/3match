using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    Index m_index;
    Sprite m_sprite;
    Transform m_effectTransform;
    [Header("Resource")]
    [SerializeField] Sprite[] m_sprireResources;

    GameObject m_PrefabEffect;

    static Effect s_instance = null;
    public static Effect instance {get { return s_instance; } }

    public Index index { get { return m_index; } set { m_index = value; } }

    public void Awake()
    {
        s_instance = this;
    }

    //void start()
    //{
    //    m_effectTransform = GetComponent<Transform>();
    //}
    //void Update()
    //{
    //    Vector3 MoveAmount = Vector3.back * Time.deltaTime;
    //    m_effectTransform.Translate(MoveAmount);
    //}


    private void Update()
    {

    }

    public void CreatEffect(Transform a)
    {
        //m_PrefabEffect = Resources.Load("Prefabs/Smoke") as GameObject;
        //GameObject obj = Instantiate(m_PrefabEffect, a.position, Quaternion.Euler(0f,0f,1f));

        //Debug.Log("터져요!");
    }

    //void InitPosition()
    //{
    //    m_effectTransform.position = new Vector3(Random.Range(-60.0f, 60.0f), 60.0f, 0.0f);
    //}

}
