using System;
using UnityEngine;

public class Queen : Figure
{
    public Queen(GameObject figure, Vector2 coords, FigColor color)
       : base(figure, coords, color) { }


    public override bool Turn(Vector2 clickLocalPos)
    {
        Figure rook = new Rook(go,FigCoords,color);
        Figure bishop = new Bishop(go, FigCoords, color);
        bool canMove=false;
        if(rook.Turn(clickLocalPos) //походить как ладья или 
            || bishop.Turn(clickLocalPos))//походить как слон    
        {
            canMove = true;
        }
        Destroy(rook.go);
        Destroy(bishop.go);
        return canMove;
    }
}
