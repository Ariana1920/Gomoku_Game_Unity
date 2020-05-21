using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum chessState
{
    Watch,
    Black,
    White
}

public class ChessBoard : MonoBehaviour {

    static ChessBoard instance;

    /// <summary>
    /// Chess Position, check who win, check win condition, Game Over state
    /// </summary>
    /// 
    #region
    public static ChessBoard Instacne //單例
    {
        get
        {
            return instance;
        }
    }

    #endregion

    public int[,] grid;
    public float timer = 0;
    public bool gameStart = false;

    public GameObject[] prefabs;
    public Text winner;
    public Stack<Transform> chessStack = new Stack<Transform>();
    public chessState whoTurn;

    Transform storeChess;

    private void Awake()
    {
        if(Instacne == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        whoTurn = chessState.Black;
        storeChess = GameObject.FindWithTag("Parent").transform;
        grid = new int[15, 15];
        gameStart = true;
    }
    private void FixedUpdate()
    {
        timer += Time.deltaTime; 
    }

    public bool ChessPos(int[] pos)
    {
        if (!gameStart)
        {
            return false;
        }
        //Camera position(0,0),but to the board is actually (7,7);
        //So the x is restricted in (0,14), the same to the y Pos;
        pos[0] = Mathf.Clamp(pos[0], 0, 14);
        pos[1] = Mathf.Clamp(pos[1], 0, 14);

        //float width = board.GetComponent<RectTransform>().rect.width;
        //float height = board.GetComponent<RectTransform>().rect.height;
        //Vector3 startPos = board.transform.position;
        //startPos.x -= width * canvas.transform.localScale.x / 2;
        //startPos.y += height * canvas.transform.localScale.y / 2;

        //if is Black turn, we spawn black prefabs in gird[7,7], but actually is woldPos(0,0);

        if (whoTurn == chessState.Black)
        {   
            GameObject blackChess=  Instantiate(prefabs[0], new Vector3(pos[0] - 7, pos[1] - 7), Quaternion.identity);
            //set balck as 1
            grid[pos[0], pos[1]] = 1;

            //add this transform into our stack
            chessStack.Push(blackChess.transform);

            //store blackChess prefabs
            blackChess.transform.SetParent(storeChess);

            if (hasWinner(pos))
            {
                GameOver();
            }
            whoTurn = chessState.White;
            
        }
        else if(whoTurn == chessState.White)
        {
            GameObject whiteChess = Instantiate(prefabs[1], new Vector3(pos[0] - 7, pos[1] - 7), Quaternion.identity);
            //set white as 2
            grid[pos[0], pos[1]] = 2;

            //add this transform into our stack
            chessStack.Push(whiteChess.transform);

            //store whiteChess prefabs
            whiteChess.transform.SetParent(storeChess);
            
            if (hasWinner(pos))
            {
                GameOver();
            }
            whoTurn = chessState.Black;
        }

        return true;
    }


    public bool hasWinner(int[] pos)
    {
        // check horizontal, vertical, left diagonal , right diagonal have five same color chess
        if (checkFive(pos, new int[2] { 1, 0 }))
            return true;
        if (checkFive(pos, new int[2] { 0, 1 }))
            return true;
        if (checkFive(pos, new int[2] { 1, 1 }))
            return true;
        if (checkFive(pos, new int[2] { 1, -1 }))
            return true;
        return false;
    }

    public bool checkFive(int[] pos, int[] offset)
    {
        //if reach 5 means that it win
        int Num = 1;

        //i presents x axis offest, j prensents y axis offset ;
        // when check position, x.pos+ offset should be restricted in (0,15) , the same to y.pos +offset 
        for (int i = offset[0], j = offset[1];
            (pos[0] + i >= 0 && pos[0] + i < 15) &&
            pos[1] + j >= 0 && pos[1] + j < 15;
            i += offset[0], j += offset[1])
        {
            //check right side first
            if (grid[pos[0] + i, pos[1] + j] == (int)whoTurn)
            {

                Num++;
            }
            else
            {
                break;
            }
        }
        
        for (int i = -offset[0], j = -offset[1]; (pos[0] + i >= 0 && pos[0] + i < 15) &&
            pos[1] + j >= 0 && pos[1] + j < 15; i -= offset[0], j -= offset[1])
        {
            //check left side first
            if (grid[pos[0] + i, pos[1] + j] == (int)whoTurn)
            {
                Num++;
            }
            else
            {
                break;
            }
        }

        if (Num > 4) return true;

        return false;
    }

    void GameOver()
    {
        winner.transform.parent .parent.gameObject.SetActive(true);
        switch (whoTurn)
        {
            case chessState.Watch:
                break;
            case chessState.Black:
                winner.text = "Black Win！";
                break;
            case chessState.White:
                winner.text = "White Win！";
                break;
            default:
                break;
        }       
        gameStart = false;
    }
  
    public void RetractChess()
    {
        if (chessStack.Count > 1)
        {
            Transform pos = chessStack.Pop();
            grid[(int)(pos.position.x + 7), (int)(pos.position.y + 7)] = 0;
            Destroy(pos.gameObject);
            pos = chessStack.Pop();
            grid[(int)(pos.position.x + 7), (int)(pos.position.y + 7)] = 0;
            Destroy(pos.gameObject);
        }
    }

}






