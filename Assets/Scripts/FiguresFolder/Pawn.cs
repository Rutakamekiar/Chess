using UnityEngine;
using System;
public class Pawn : Figure
{
    public Pawn(GameObject figure, Vector2 coords, FigColor color) 
        : base(figure, coords, color)
    {
    }
    public override bool Turn(Vector2 clickLocalPos)
    {
        Vector2 diff = clickLocalPos - FigCoords;
        diff = color == FigColor.White ? diff : -diff;
        if (diff.x==0)
        {
            if ((diff.y <= 2 && diff.y > 0) && IsNextFree() && //движение с начальной позиции на 2 клетки, проверка назад нельзя
               (FigCoords.y == 1 || FigCoords.y == 6) || diff.y == 1)//или движение на 1 клетку
                
            {
                return true;
            }
        }
        return CanAttack(diff, clickLocalPos);
    }

    private bool CanAttack(Vector2 diff, Vector2 clickLocalPos)
    {
        Figure f = Gameplay.keyValuePairs[(int)clickLocalPos.x, (int)clickLocalPos.y] as Figure;
        if (Math.Abs(diff.x) == 1 && diff.y == 1 && f is Figure && f.color != color)
        {
            return true;
        }
        return false;
    }

    private bool IsNextFree()
    {
        if (color == FigColor.White)
        {
            return !(Gameplay.keyValuePairs
                [(int)go.transform.localPosition.x, (int)go.transform.localPosition.y + 1] is Figure);
        }
        else
        {
            return !(Gameplay.keyValuePairs
                [(int)go.transform.localPosition.x, (int)go.transform.localPosition.y - 1] is Figure);
        }
    }
}

