using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILevelTwo : AILevelOne
{

    #region
    protected override void Start()
    {
        //condition one for two chess
        //simple two chess condition
        chessScore.Add("aa___", 100);                      
        chessScore.Add("a_a__", 100);
        chessScore.Add("a__a_", 100);
        chessScore.Add("a___a", 100);
        chessScore.Add("_a__a", 100);
        chessScore.Add("__a_a", 100);
        chessScore.Add("___aa", 100);
    

        //condition two for two chess
        //more flexible, havee more chance to become condition four or five
        //so have higer scores
        chessScore.Add("__aa__", 500);                     
        chessScore.Add("_a_a_", 500);
        //chessScore.Add("_a__a_", 500);

        chessScore.Add("_aa__", 500);
        chessScore.Add("__aa_", 500);


        //conditon three for three chess
        chessScore.Add("aaa__", 1000);
        chessScore.Add("aa__a", 1000);
        chessScore.Add("aa_a_", 1000);
        chessScore.Add("a_a_a", 1000);
        chessScore.Add("a_aa_", 1000);
        chessScore.Add("_aa_a", 1000);
        chessScore.Add("_a_aa", 1000);                   

        //condition four
        chessScore.Add("_aa_a_", 9000);                    
        chessScore.Add("_a_aa_", 9000);

        //condition five
        //more flexible, have more chance to become condition six or seven
        //so have higer scores
        chessScore.Add("_aaa_", 10000);                  

        //condition six                  
        chessScore.Add("aaaa_", 15000);
        chessScore.Add("aaa_a", 15000);
        chessScore.Add("aa_aa", 15000);
        chessScore.Add("a_aaa", 15000);                            
        chessScore.Add("_aaaa", 15000);

        //condition seven
        //only one step can win the game
        chessScore.Add("_aaaa_", 1000000);                 

        //win condition
        chessScore.Add("aaaaa", float.MaxValue);           


        if (chessColor != chessState.Watch)
            Debug.Log(chessColor + "AILevelTwo");
    }
    #endregion

    // funtion to override base on CheckFive in AIPlayOne
    // to check which position have the highest score
    public override void checkFive(int[] pos, int[] offset, int chess)
    {
        bool leftSide = true, leftStop = false, rightStop = false;
        int maxNum = 1; 
        string str = "a";
        int ri = offset[0], rj = offset[1];
        int li = -offset[0], lj = -offset[1];
        while (maxNum < 7 && (!leftStop || !rightStop))
        {
            if (leftSide)
            {
                if ((pos[0] + li >= 0 && pos[0] + li < 15) &&
            pos[1] + lj >= 0 && pos[1] + lj < 15 && !leftStop)
                {
                    if (ChessBoard.Instacne.grid[pos[0] + li, pos[1] + lj] == chess)
                    {
                        maxNum++;
                        str = "a" + str;

                    }
                    else if (ChessBoard.Instacne.grid[pos[0] + li, pos[1] + lj] == 0)
                    {
                        maxNum++;
                        str = "_" + str;
                        if (!rightStop) leftSide = false;
                    }

                    // if is not same color stone, not an empty position
                    // then we collide different color stone
                    // stop checking
                    else
                    {
                        leftStop = true;
                        if (!rightStop) leftSide = false;
                    }
                    li -= offset[0]; lj -= offset[1];
                }
                else
                {
                    leftStop = true;
                    if (!rightStop) leftSide = false;
                }
            }
            else
            {
                if ((pos[0] + ri >= 0 && pos[0] + ri < 15) &&
          pos[1] + rj >= 0 && pos[1] + rj < 15 && !leftSide && !rightStop)
                {
                    if (ChessBoard.Instacne.grid[pos[0] + ri, pos[1] + rj] == chess)
                    {
                        maxNum++;
                        str += "a";

                    }
                    else if (ChessBoard.Instacne.grid[pos[0] + ri, pos[1] + rj] == 0)
                    {
                        maxNum++;
                        str += "_";
                        if (!leftStop) leftSide = true;
                    }

                    // if is not same color stone, not an empty position
                    // then we collide different color stone
                    // stop checking
                    else
                    {
                        rightStop = true;
                        if (!leftStop) leftSide = true;
                    }
                    ri += offset[0];
                    rj += offset[1];
                }
                else
                {
                    rightStop = true;
                    if (!leftStop) leftSide = true;
                }
            }
        }

        string Str = "";

        //loop through all the keyInfo in the chessScore
        foreach (var keyInfo in chessScore)
        {
            if (str.Contains(keyInfo.Key))
            {
                if (Str != "")
                {
                    if (chessScore[keyInfo.Key] > chessScore[Str])
                    {
                        Str = keyInfo.Key;
                    }
                }
                else
                {
                    Str = keyInfo.Key;
                }
            }
        }

        if (Str != "")
        {
            score[pos[0], pos[1]] += chessScore[Str];
        }
    }
}
