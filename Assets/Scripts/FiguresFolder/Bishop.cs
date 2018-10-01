using System;
using UnityEngine;

class Bishop : Figure
{
    public Bishop(GameObject figure, Vector2 coords, FigColor color)
       : base(figure, coords, color) { }
    public override bool Turn(Vector2 clickLocalPos)
    {
        Vector2 diff = clickLocalPos - FigCoords;
        Vector2 absDiff = new Vector2(Math.Abs(diff.x), Math.Abs(diff.y));
        //движение по диагонали
        if (absDiff.x == absDiff.y)
        {
            Figure f = Gameplay.keyValuePairs[(int)clickLocalPos.x, (int)clickLocalPos.y] as Figure;
            if (f is Figure && f.color == color)
            {
                return false;
            }
            //движение вверх
            if (diff.y > 0)
            {
                //движение вверх вправо
                if (diff.x > 0)
                {
                    for (int i = 1; i < (int)absDiff.x; i++)
                    {
                        if (Gameplay.keyValuePairs[(int)FigCoords.x + i, (int)FigCoords.y + i] is Figure)
                        {
                            return false;
                        }
                    }
                }
                //движение вверх влево
                else if (diff.x < 0)
                {
                    for (int i = 1; i < (int)absDiff.x; i++)
                    {
                        if (Gameplay.keyValuePairs[(int)FigCoords.x - i, (int)FigCoords.y + i] is Figure)
                        {
                            return false;
                        }
                    }
                }
            }
            //движение вниз
            else if (diff.y < 0)
            {
                //движение вниз вправо
                if (diff.x > 0)
                {
                    for (int i = 1; i < (int)absDiff.x; i++)
                    {
                        if (Gameplay.keyValuePairs[(int)FigCoords.x + i, (int)FigCoords.y - i] is Figure)
                        {
                            return false;
                        }
                    }
                }
                //движение вниз влево
                else if (diff.x < 0)
                {
                    for (int i = 1; i < (int)absDiff.x; i++)
                    {
                        if (Gameplay.keyValuePairs[(int)FigCoords.x - i, (int)FigCoords.y - i] is Figure)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }
}
