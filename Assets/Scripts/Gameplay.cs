using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
class Gameplay : MonoBehaviour
{
    public GameObject pawnW;
    public GameObject rookW;
    public GameObject knightW;
    public GameObject bishopW;
    public GameObject queenW;
    public GameObject kingW;
    public GameObject showPossibleMoves;
    public GameObject showThisPos;
    public GameObject pawnB;
    public GameObject rookB;
    public GameObject knightB;
    public GameObject bishopB;
    public GameObject queenB;
    public GameObject kingB;
    public Text winnerText;
    public GameObject winPanel;
    public Text nowTurnText;
    public static object[,] keyValuePairs = new object[8, 8];
    bool figActivation = false;
    FigColor nowTurn = FigColor.White;
    Figure activeFigure;
    King whiteKing, blackKing;
    List<Vector2> canMovesList = new List<Vector2>();
    private void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            keyValuePairs[i, 1] = new Pawn(pawnW, new Vector2(i, 1), FigColor.White);
            keyValuePairs[i, 6] = new Pawn(pawnB, new Vector2(i, 6), FigColor.Black);
        }
        for (int i = 0; i < 2; i++)
        {
            int iMul7 = i * 7;
            keyValuePairs[iMul7, 0] = new Rook(rookW, new Vector2(iMul7, 0), FigColor.White);
            keyValuePairs[iMul7, 7] = new Rook(rookB, new Vector2(iMul7, 7), FigColor.Black);
            iMul7 = i * 5 + 1;
            keyValuePairs[iMul7, 0] = new Knight(knightW, new Vector2(iMul7, 0), FigColor.White);
            keyValuePairs[iMul7, 7] = new Knight(knightB, new Vector2(iMul7, 7), FigColor.Black);
            iMul7 = i * 3 + 2;
            keyValuePairs[iMul7, 0] = new Bishop(bishopW, new Vector2(iMul7, 0), FigColor.White);
            keyValuePairs[iMul7, 7] = new Bishop(bishopB, new Vector2(iMul7, 7), FigColor.Black);
        }
        keyValuePairs[3, 0] = new Queen(queenW, new Vector2(3, 0), FigColor.White);
        keyValuePairs[3, 7] = new Queen(queenB, new Vector2(3, 7), FigColor.Black);
        keyValuePairs[4, 0] = new King(kingW, new Vector2(4, 0), FigColor.White);
        whiteKing = keyValuePairs[4, 0] as King;
        keyValuePairs[4, 7] = new King(kingB, new Vector2(4, 7), FigColor.Black);
        blackKing = keyValuePairs[4, 7] as King;
    }

    private void Update()
    {
        //левый клик мыши
        if (Input.GetMouseButtonDown(0))
        {
            //координаты клика
            Vector2 clickWorldPos
                = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            //перевод клика в локальные координаты!!!!!
            Vector2 clickLocalPos = GameObject.Find("Game")
                .transform.InverseTransformPoint(clickWorldPos);
            //выравнивание clickLocalPos
            clickLocalPos.y = Convert.ToInt32(clickLocalPos.y);
            clickLocalPos.x = Convert.ToInt32(clickLocalPos.x);
            //если нет активной фигуры
            if (!figActivation)
            {
                //находим клетку на доске с координатами клика
                Figure clickedFigure = GetFigureByCoords(clickLocalPos);
                //проверяем содержит ли данная клетка фигуру
                if (clickedFigure is Figure)
                {
                    //проверяем на поочередность фигуры
                    if (clickedFigure.color == nowTurn)
                    {
                        activeFigure = clickedFigure;
                        //записываем в лист возможные хода данной фигуры
                        canMovesList = Aa(activeFigure, ReturnKingByColor(nowTurn));
                        //вывод возможных ходов
                        ShowPossibleMoves(canMovesList);
                        //если данной фигурой можно походить--активация фигуры
                        if (canMovesList.Count != 0)
                        {
                            //делаем выбраную фигуру активной
                            Vector3 thisPosVector = new Vector3(activeFigure.FigCoords.x, activeFigure.FigCoords.y,1);
                            print(thisPosVector);
                            Instantiate(showThisPos, 
                                GameObject.Find("Game").transform.TransformPoint(thisPosVector),
                                Quaternion.identity);
                            figActivation = true;
                        }
                    }
                }
            }
            //если активная фигура есть
            else
            {
                //деактивация фигуры при повторном нажатии на неё
                if (clickLocalPos == activeFigure.FigCoords)
                {
                    //очистка списка с возможными ходами
                    canMovesList.Clear();
                    figActivation = false;
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("showMoves"))
                    {
                        Destroy(obj);
                    }
                }
                //проверяет есть ли в списке возможных ходов выбранная клетка
                if (canMovesList.Contains(clickLocalPos))
                {
                    //если на выбранной клетке есть вражеская фигура, уничтожить её
                    if (GetFigureByCoords(clickLocalPos) is Figure)
                    {
                        Destroy(GetFigureByCoords(clickLocalPos).go);
                    }
                    if (activeFigure is King)
                    {
                        if ((activeFigure as King).Castling(clickLocalPos))
                        {
                            int diff = (int)clickLocalPos.x - (int)activeFigure.FigCoords.x;
                            Rook castledRook = diff > 0 ?
                                (activeFigure as King).r2 : (activeFigure as King).r1;
                            int rookPos = diff > 0 ?
                                (int)clickLocalPos.x - 1 : (int)clickLocalPos.x + 1;
                            //обновляем матрицу 
                            keyValuePairs[(int)castledRook.FigCoords.x,
                                (int)castledRook.FigCoords.y] = null;
                            castledRook.FigCoords = new Vector2(rookPos, castledRook.FigCoords.y);
                            keyValuePairs[(int)castledRook.FigCoords.x,
                                (int)castledRook.FigCoords.y] = castledRook;
                        }
                        (activeFigure as King).canCastling = false;
                    }
                    if (activeFigure is Rook)
                    {
                        (activeFigure as Rook).canCastling = false;
                    }

                    //обновляем матрицу 
                    keyValuePairs[(int)clickLocalPos.x, (int)clickLocalPos.y] = activeFigure;
                    keyValuePairs[(int)activeFigure.FigCoords.x,
                        (int)activeFigure.FigCoords.y] = null;
                    //меняем координаты выбраной фигуры
                    activeFigure.FigCoords = clickLocalPos;
                    if (activeFigure is Pawn && (activeFigure.FigCoords.y == 7 || activeFigure.FigCoords.y == 0))
                    {
                        Destroy(activeFigure.go);

                        keyValuePairs[(int)activeFigure.FigCoords.x, (int)activeFigure.FigCoords.y] =
                            new Queen(queenW, activeFigure.FigCoords, activeFigure.color);
                    }
                    //деативируем фигуру
                    figActivation = false;
                    //удаление возможных ходов
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("showMoves"))
                    {
                        Destroy(obj);
                    }
                    //проверка на шах
                    CheckCheck(whiteKing);
                    CheckCheck(blackKing);
                    if (CheckCheckMate(whiteKing))
                    {
                        winnerText.text = "Black win";
                        winPanel.SetActive(true);
                    }
                    else if (CheckCheckMate(blackKing))
                    {
                        winnerText.text = "White win";
                        winPanel.SetActive(true);
                    }
                    //смена очередности фигур
                    if (nowTurn == FigColor.White)
                    {
                        nowTurn = FigColor.Black;
                        nowTurnText.text = "Black Turn";
                    } else
                    {
                        nowTurn = FigColor.White;
                        nowTurnText.text = "White Turn";
                    }
                    //очистка списка с возможными ходами
                    canMovesList.Clear();
                }

            }
        }
    }
    /// <summary>
    /// метод, который проверяет шах данному королю
    /// </summary>
    /// <param name="king">данный король</param>
    private void CheckCheck(King king)
    {
        king.isCheck = false;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                //находит каждую фигуру на клетке
                if (keyValuePairs[i, j] is Figure)
                {
                    //атака каждой фигурой на клетку короля
                    if ((keyValuePairs[i, j] as Figure).Turn(king.FigCoords))
                    {
                        print("шах " + king.FigCoords);
                        king.isCheck = true;
                        return;
                    }
                }
            }
        }
    }
    private King ReturnKingByColor(FigColor color)
    {
        return color == FigColor.White ? whiteKing : blackKing;
    }
    private List<Vector2> Aa(Figure figure, King king)
    {
        List<Vector2> lst = new List<Vector2>();
        Figure enemy;
        foreach (Vector2 v2 in figure.GetAllPossibleMoves())
        {
            bool kingCheck = king.isCheck;
            Vector2 helpV = figure.FigCoords;
            figure.FigCoords = v2;
            //обновляем матрицу 
            enemy = keyValuePairs[(int)v2.x, (int)v2.y] as Figure;
            keyValuePairs[(int)v2.x, (int)v2.y] = figure;
            keyValuePairs[(int)helpV.x, (int)helpV.y] = null;
            //проверить есть ли шах
            CheckCheck(king);
            if (!king.isCheck)
            {
                //заполнение списка возможных ходов
                lst.Add(v2);
                print("список ходов" + v2);
            }
            figure.FigCoords = helpV;
            //откатываем матрицу 
            keyValuePairs[(int)v2.x, (int)v2.y] = enemy;
            keyValuePairs[(int)helpV.x, (int)helpV.y] = figure;
            king.isCheck = kingCheck;
        }
        if (figure is King)
        {
            if (!lst.Contains(new Vector2(figure.FigCoords.x - 1, figure.FigCoords.y)))
            {
                lst.Remove(new Vector2(figure.FigCoords.x - 2, figure.FigCoords.y));
            }
            if (!lst.Contains(new Vector2(figure.FigCoords.x + 1, figure.FigCoords.y)))
            {
                lst.Remove(new Vector2(figure.FigCoords.x + 2, figure.FigCoords.y));
            }
        }
        return lst;
    }
    public bool CheckCheckMate(King king)
    {
        if (king.isCheck == true)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (keyValuePairs[i, j] is Figure &&
                        GetFigureByCoords(new Vector2(i, j)).color == king.color &&
                        Aa(GetFigureByCoords(new Vector2(i, j)), king).Count != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    }
    public Figure GetFigureByCoords(Vector2 coords)
    {
        return keyValuePairs[(int)coords.x, (int)coords.y] as Figure;
    }

    public void ShowPossibleMoves(List<Vector2> possibleMoves)
    {
        foreach (Vector2 v in possibleMoves)
        {
            Vector2 showPossibleMovesPos = GameObject.Find("Game")
                    .transform.TransformPoint(v);//перевод клика в локальные координаты!!!!!
            Instantiate(showPossibleMoves, showPossibleMovesPos, Quaternion.identity);
        }
    }
}
