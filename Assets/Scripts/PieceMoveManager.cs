using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMoveManager : MonoBehaviour
{

    static PieceMoveManager s_instance;
    public static PieceMoveManager Instance { get { return s_instance; } }

    List<PieceMoveEvent> mEventList = new List<PieceMoveEvent>();


    private void Awake()
    {
        s_instance = this;
    }


    private void Update()
    {
        for(int i = 0; i < mEventList.Count; ++i)
        {
            if(mEventList[i].Update())
            {
                mEventList.RemoveAt(i--);
            }
        }
    }

    public void AddMoveEvent(PieceMoveEvent moveEvent)
    {
        mEventList.Add(moveEvent);
    }

    public bool HasMoveEvent()
    {
        return mEventList.Count != 0;
    }
}
