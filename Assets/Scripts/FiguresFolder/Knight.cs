using System;
using UnityEngine;

public class Knight : Figure
{
    public Knight(GameObject figure, Vector2 coords, FigColor color)
       : base(figure, coords, color)
    {
    }

    public override bool Turn(Vector2 clickLocalPos)
    {
        Vector2 diff = clickLocalPos - FigCoords;
        Vector2 absDiff = new Vector2(Math.Abs(diff.x), Math.Abs(diff.y));
        //движение вверх/вниз на 2 клетки и влево/вправо на 1 ИЛИ влево/вправо на 2 и вверх/вниз на 1
        if ((absDiff.x == 1 && absDiff.y == 2) || (absDiff.x == 2 && absDiff.y == 1))
        {
            Figure f = Gameplay.keyValuePairs[(int)clickLocalPos.x, (int)clickLocalPos.y] as Figure;
            if (f is Figure && f.color == color)
            {
                return false;
            }
            return true;
        }
        return false;
    }
}

