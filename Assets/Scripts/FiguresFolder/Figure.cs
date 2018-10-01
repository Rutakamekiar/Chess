using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FigColor { White, Black };
public abstract class Figure : MonoBehaviour {

    public FigColor color;
    public GameObject go;
    public Figure(GameObject figure, Vector2 coords, FigColor color)
    {
        this.color = color;
        go = SpawnFigure(figure, coords);
    }   
    public abstract bool Turn(Vector2 clickLocalPos);


    public GameObject SpawnFigure(GameObject obj, Vector2 coords)
    {
        GameObject o = Instantiate(obj, coords, Quaternion.identity);
        o.transform.SetParent(GameObject.Find("Game").transform);
        o.transform.localPosition = coords;
        return o;
    }

    public override string ToString()
    {
        return returnName() + " " + go.transform.localPosition + " " + color;
    }
    public string returnName()
    {
        return GetType().Name;
    }

    public Vector2 FigCoords
    {
        get
        {
            return go.transform.localPosition;
        }
        set
        {
            go.transform.localPosition = value;
        }
    }
    public List<Vector2> GetAllPossibleMoves()
    {
        List<Vector2> list = new List<Vector2>();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Turn(new Vector2(i, j)))
                {
                    list.Add(new Vector2(i, j));
                }
            }
        }
        return list;
    } 
}

