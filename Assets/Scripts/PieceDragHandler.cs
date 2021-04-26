using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



// Piece 드래그처리 해줄 구조체

public struct PieceDragHandler
{
    const float maxDistance = 12f;

    public Piece targetPiece;
    public Vector2 originPosition;
    public Node originNode;


    public bool IsNull()
    {
        return targetPiece == null;
    }


    public void Reset()
    {
        targetPiece = null;
        originPosition = Vector2.zero;
    }


    public void Set(Piece piece, Node node)
    {
        if (piece == null)
            return;
        targetPiece = piece;
        originPosition = piece.rectTransform.position;
        originNode = node;
    }


    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - originPosition;

        Vector2 absDirection = new Vector2();
        absDirection.x = Mathf.Abs(direction.x);
        absDirection.y = Mathf.Abs(direction.y);

        Vector2 normalizeDirection = direction.normalized;

        Vector2 finalDirection;
        if(absDirection.x > absDirection.y)
        {
            finalDirection.x = absDirection.x > maxDistance ? maxDistance : direction.x;
            finalDirection.y = 0f;
            if(direction.x < 0f)
            {
                finalDirection.x *= -1f;
            }
        }
        else
        {
            finalDirection.x = 0f;
            finalDirection.y = absDirection.y > maxDistance ? maxDistance : direction.y;
            if(direction.y < 0f)
            {
                finalDirection.y *= -1f;
            }
        }
        targetPiece.transform.position = originPosition + finalDirection;
    }

    public Vector2 GetDragDirection()
    {
        Vector2 piecePos = new Vector2(targetPiece.rectTransform.position.x, targetPiece.rectTransform.position.y);
        return (piecePos - originPosition).normalized;
    }
}
