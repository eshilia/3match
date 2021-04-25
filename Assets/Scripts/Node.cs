using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Index index;
    public Vector2 position;
    public Piece piece;


    public Node(int indexX, int indexY, Vector2 position, Piece piece = null)
    {
        this.index = new Index(indexX, indexY);
        this.position = position;
        this.piece = piece;
    }
}
