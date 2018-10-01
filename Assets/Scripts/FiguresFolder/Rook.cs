using System;
using UnityEngine;

public class Rook : Figure
{
    public bool canCastling = true;
    public Rook(GameObject figure, Vector2 coords, FigColor color)
       : base(figure, coords, color) { }
    public override bool Turn(Vector2 clickLocalPos)
    {
        Vector2 diff = clickLocalPos - FigCoords;
        if (diff.x == 0 || diff.y == 0)//движение прямо по х или у
        {
            Figure f = Gameplay.keyValuePairs[(int)clickLocalPos.x, (int)clickLocalPos.y] as Figure;
            if (f is Figure && f.color == color)
            {
                return false;
            }
            //движение вверх
            if (diff.y > 0)
            {
                for (int i = 1; i < diff.y; i++)
                {
                    if (Gameplay.keyValuePairs[(int)FigCoords.x, (int)FigCoords.y + i] is Figure)
                    {
                        return false;
                    }
                }
            }
            else if (diff.y < 0)
            {
                for (int i = 1; i < Math.Abs(diff.y); i++)
                {
                    if (Gameplay.keyValuePairs[(int)FigCoords.x, (int)FigCoords.y - i] is Figure)
                    {
                        return false;
                    }
                }
            }
            else if (diff.x > 0)
            {
                for (int i = 1; i < Math.Abs(diff.x); i++)
                {
                    if (Gameplay.keyValuePairs[(int)FigCoords.x + i, (int)FigCoords.y] is Figure)
                    {
                        return false;
                    }
                }
            }
            else if (diff.x < 0)
            {
                for (int i = 1; i < Math.Abs(diff.x); i++)
                {
                    if (Gameplay.keyValuePairs[(int)FigCoords.x - i, (int)FigCoords.y] is Figure)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    }
}
