using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILevelOne : Player
{
    //use dictionary to store all types of condition and score
    protected Dictionary<string, float> chessScore = new Dictionary<string, float>();
    protected float[,] score = new float[15, 15];

    #region
    protected override void Start()
    {
        // add all the type of score into dictionary so that we can calculate score later
        //"_aa_" has more chance so the score is higher then other
        chessScore.Add("aa_", 50);
        chessScore.Add("_aa", 50);
        chessScore.Add("_aa_", 100);
      
        chessScore.Add("_aaa_", 1000);
        chessScore.Add("aaa_", 500);
        chessScore.Add("_aaa", 500);

        chessScore.Add("_aaaa_", 10000);
        chessScore.Add("aaaa_", 5000);
        chessScore.Add("_aaaa", 5000);

        chessScore.Add("aaaaa", float.MaxValue);
        chessScore.Add("aaaaa_", float.MaxValue);
        chessScore.Add("_aaaaa", float.MaxValue);
        chessScore.Add("_aaaaa_", float.MaxValue);
    }
    #endregion

    public override void putChess()
    {
        //check who go fist ? AI or human ?
        //if it is AI then we simple just put the chess in the ceter

        if (ChessBoard.Instacne.chessStack.Count == 0)
        {
            if (ChessBoard.Instacne.ChessPos(new int[2] { 7, 7 }))
            {
                ChessBoard.Instacne.timer = 0;
            }
            return;
        }

        float maxScore = 0;

        int[] maxPos = new int[2] { 0, 0 };

        //loop through all the position in the board to check which position
        //have the chance to gain highest score

        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                if (ChessBoard.Instacne.grid[i, j] == 0)
                {
                    //check every position of the possible scores
                    //to see which position have the highest score
                    SetScore(new int[2] { i, j });
                    //print(maxScore);
                    if (score[i, j] >= maxScore)
                    {
                        //this is the position AI would put the chess
                        maxPos[0] = i;
                        maxPos[1] = j;
                        maxScore = score[i, j];
                    }
                }
            }
        }
        //print(timer);
        //timer = 0;

        // find the maxPos thats the position AI will put the chess
        if (ChessBoard.Instacne.ChessPos(maxPos))
        {
            ChessBoard.Instacne.timer = 0;
        } 
    }

    //check (pos.x+offset,pos.y+offset) to see if there is any possible situation to have those
    //string we claimed in dictionary
    public void SetScore(int[] pos)
    {
        //reset score first
        //but we need to calculate white scores and black scores
        //because we do not who would be the first one
        score[pos[0], pos[1]] = 0;

        checkFive(pos, new int[2] { 1, 0 }, 1);
        checkFive(pos, new int[2] { 1, 1 }, 1);
        checkFive(pos, new int[2] { 1, -1 }, 1);
        checkFive(pos, new int[2] { 0, 1 }, 1);

        checkFive(pos, new int[2] { 1, 0 }, 2);
        checkFive(pos, new int[2] { 1, 1 }, 2);
        checkFive(pos, new int[2] { 1, -1 }, 2);
        checkFive(pos, new int[2] { 0, 1 }, 2);
    }


    public virtual void checkFive(int[] pos, int[] offset, int chess)
    {
        /// </summary>
        /// use string "a" to present one chess and check which position have more scores chances
        /// 0 and 1 present chessColor, 
        /// </summary>
        
        string line = "a";

        //check right side first
        for (int i = offset[0], j = offset[1]; (pos[0] + i >= 0 && pos[0] + i < 15) &&
            pos[1] + j >= 0 && pos[1] + j < 15; i += offset[0], j += offset[1])
        {
            if (ChessBoard.Instacne.grid[pos[0] + i, pos[1] + j] == chess)
            {
                //color same add one "a" means add one same color chess here
                line += "a";
            }
            else if (ChessBoard.Instacne.grid[pos[0] + i, pos[1] + j] == 0)
            {
                line += "_";
                break;
            }
            else
            {
                break;
            }
        }

        //then check left side
        for (int i = -offset[0], j = -offset[1]; (pos[0] + i >= 0 && pos[0] + i < 15) &&
            pos[1] + j >= 0 && pos[1] + j < 15; i -= offset[0], j -= offset[1])
        {
            if (ChessBoard.Instacne.grid[pos[0] + i, pos[1] + j] == chess)
            {
                //color same add one "a" means add one same color chess here
                line = "a" + line;

            }
            else if (ChessBoard.Instacne.grid[pos[0] + i, pos[1] + j] == 0)
            {
                line = "_" + line;
                break;
            }
            else
            {
                break;
            }
        }

        ///</summary>
        ///after check left and right side if there is any string that same to
        ///those we claimed before in dictionary, if one position can have many same string conditions
        ///then this positon will accumulate score
        ///</summary>
       
        if (chessScore.ContainsKey(line))
        {
            score[pos[0], pos[1]] += chessScore[line];
        }

    }

}
