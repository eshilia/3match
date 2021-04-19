using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public struct TileLine
{
    private bool m_isVertical;
    private int m_lineIndex;
    private int m_startIndex;
    private int m_endIndex;
}


public class MatchManager : MonoBehaviour
{


    public void MakeTile(ref int[,] _tileArray)
    {

    }
    // 모든 타일에 대해 매칭 판별하는 함수
    // 매칭에 해당하는 모든 리스트를 반환한다.
    public List<TileLine> CheckAllMatch(int[,] _tileArray)
    {
        return null;
    }

    // 매칭된 타일을 제거하는 함수
    public void RemoveMatchTile(ref int[,] _tileArray, List<TileLine> _tileLines)
    {

    }

    // 제거된 타일의 빈공간을 채우는 함수.
    public void RefillTile(ref int[,] _tileArray, List<TileLine> _tileLines)
    {

    }

    // 채워진 타일이 매칭 가능한 패널이 존재하는지 확인.
    // 존재하지 않는다면 타일 생성을 다시해야한다.
    public bool CheckCanMatch(int[,] _tileArray)
    {
        return false;
    }

    // 특정 위치에 있는 타일에 대해 4방향으로 매칭이 있는지 확인하는 함수.
    // 유저가 swap 시 swap된 두 타일에 대해 사용한다.
    public List<TileLine> CheckOneMatch(int[,] _tileArray, int _x, int _y)
    {
        return null;
    }
}


