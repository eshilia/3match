using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Piece의 움직임 처리
public class PieceMoveHandler
{
    public Node targetNode;
    public Piece movePiece;
    public float moveSpeed;

    bool isEnd;

    public bool IsEnd() { return isEnd; }

    public PieceMoveHandler(Piece movePiece, Node targetNode, float moveSpeed = 16f)
    {
        this.movePiece = movePiece;
        this.targetNode = targetNode;
        this.moveSpeed = moveSpeed;

        this.isEnd = false;
    }
    public void UpdateMove()
    {
        if(movePiece == null)
        {
            isEnd = true;
            return;
        }

        movePiece.rectTransform.anchoredPosition =
            Vector2.Lerp(movePiece.rectTransform.anchoredPosition, targetNode.position, moveSpeed * Time.deltaTime);

        if((movePiece.rectTransform.anchoredPosition - targetNode.position).magnitude < 1f)
        {
            movePiece.rectTransform.anchoredPosition = targetNode.position;
            isEnd = true;
        }
    }
}


// Piece의 움직임 처리를 따로 해주게 되면 전체 움직임이 끝났을 경우에 대한 묶음 처리가 어려움
// 따라서 해당 event에 같은 속성의 Piece 움직임을 묶어서 등록


public class PieceMoveEvent
{
    //해당 PieceMovement들이 끝났을때 알림 이벤트
    public delegate void MoveEnd<T>(List<T> moveList);
    public event MoveEnd<PieceMoveHandler> EventMoveEnd;

    List<PieceMoveHandler> mHandlerList = new List<PieceMoveHandler>();


    public bool Update()
    {
        int endCount = 0;
        for (int i = 0; i < mHandlerList.Count; ++i)
        {
            if (mHandlerList[i].IsEnd()) ++endCount;
            else mHandlerList[i].UpdateMove();
        }

        if (mHandlerList.Count == endCount)
        {
            if (EventMoveEnd != null)
                EventMoveEnd(mHandlerList);
            return true;
        }
        return false;
    }


    public void AddMoveHandler(PieceMoveHandler moveHandler)
    {
        mHandlerList.Add(moveHandler);
    }
}
