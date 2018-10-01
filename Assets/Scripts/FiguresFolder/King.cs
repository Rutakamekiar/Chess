using UnityEngine;
using System;
public class King : Figure
{
    public Rook r1, r2;
    public bool isCheck = false;
    public bool canCastling = true;
    public King(GameObject figure, Vector2 coords, FigColor color)
       : base(figure, coords, color)
    {
        r1 = Gameplay.keyValuePairs[0, (int)coords.y] as Rook;
        r2 = Gameplay.keyValuePairs[7, (int)coords.y] as Rook;
    }

    public override bool Turn(Vector2 clickLocalPos)
    {
        Vector2 diff = clickLocalPos - FigCoords;
        Vector2 absDiff = new Vector2(Math.Abs(diff.x), Math.Abs(diff.y));
        //ход не дальше 1 клетки
        if (absDiff.x < 2 && absDiff.y < 2)
        {
            Figure f = Gameplay.keyValuePairs[(int)clickLocalPos.x, (int)clickLocalPos.y] as Figure;
            if (f is Figure && f.color == color)
            {
                return false;
            }
            return true;
        }
        if (Castling(clickLocalPos))
        {
            return true;
        }
        return false;
    }
    public bool Castling(Vector2 clickLocalPos)
    {
        Vector2 diff = clickLocalPos - FigCoords;
        Vector2 absDiff = new Vector2(Math.Abs(diff.x), Math.Abs(diff.y));
        if (absDiff.x == 2 && diff.y == 0 && canCastling)
        {
            Rook castledRook = diff.x > 0 ? r2 : r1;
            if (castledRook is Rook && castledRook.canCastling && !isCheck)
            {
                int diffKingAndRook = (int)castledRook.FigCoords.x - (int)FigCoords.x;
                int absDiffKingAndRook = Math.Abs(diffKingAndRook);
                if (diffKingAndRook < 0)
                {
                    for (int i = 1; i < absDiffKingAndRook; i++)
                    {
                        if (Gameplay.keyValuePairs[(int)castledRook.FigCoords.x + i, (int)FigCoords.y] is Figure)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < absDiffKingAndRook; i++)
                    {
                        if (Gameplay.keyValuePairs[(int)castledRook.FigCoords.x - i, (int)FigCoords.y] is Figure)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }
        return false;
    }
}


