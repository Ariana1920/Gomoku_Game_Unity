using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaxNode 
{
    //node
    public List<MiniMaxNode> childNode;
    //score
    public float value;
    //color
    public int chess;
    //position
    public int[] pos;
}

public class AILevelThree : Player
{

    Dictionary<string, float> chessScore = new Dictionary<string, float>();
    
    #region
    protected override void Start()
    {
        //condition one for two chess
        //simple two chess condition
        chessScore.Add("aa___", 100);
        chessScore.Add("a_a__", 100);
        chessScore.Add("___aa", 100);
        chessScore.Add("__a_a", 100);
        chessScore.Add("a__a_", 100);
        chessScore.Add("_a__a", 100);
        chessScore.Add("a___a", 100);

        //condition two for two chess
        //more flexible, havee more chance to become condition four or five
        //so have higer scores
        chessScore.Add("__aa__", 500);
        chessScore.Add("_a_a_", 500);
        chessScore.Add("_a__a_", 500);
        chessScore.Add("_aa__", 500);
        chessScore.Add("__aa_", 500);


        //conditon three for three chess
        //one step to become condition six
        chessScore.Add("a_a_a", 1000);
        chessScore.Add("aa__a", 1000);
        chessScore.Add("_aa_a", 1000);
        chessScore.Add("a_aa_", 1000);
        chessScore.Add("_a_aa", 1000);
        chessScore.Add("aa_a_", 1000);
        chessScore.Add("aaa__", 1000);

        //condition four
        //there is an empty betwen two same color chess
        chessScore.Add("_aa_a_", 9000);
        chessScore.Add("_a_aa_", 9000);

        //condition five
        //more flexible, one step to become condition conditon six
        //so have higer scores
        chessScore.Add("_aaa_", 10000);

        //condition six
        //only have one position to reach five
        chessScore.Add("a_aaa", 15000);
        chessScore.Add("aaa_a", 15000);
        chessScore.Add("_aaaa", 15000);
        chessScore.Add("aaaa_", 15000);
        chessScore.Add("aa_aa", 15000);

        //condition seven
        //have two position to become five chess
        //only one step can win the game
        chessScore.Add("_aaaa_", 1000000);

        //win condition
        chessScore.Add("aaaaa", float.MaxValue);

    }

    #endregion

    #region put chess according to MinMax Algorithm

    public override void putChess()
    {
        if (ChessBoard.Instacne.chessStack.Count == 0)
        {
            if (ChessBoard.Instacne.ChessPos(new int[2] { 7, 7 }))
                ChessBoard.Instacne.timer = 0;
            return;
        }

        MiniMaxNode node = null;
        foreach (var Node in GetList(ChessBoard.Instacne.grid, (int)chessColor, true))
        {
            CreateTree(Node, (int[,])ChessBoard.Instacne.grid.Clone(), 3, false);

            float a = float.MinValue;
            float b = float.MaxValue;

            Node.value += AlphaBeta(Node, 3, false, a, b);

            if (node != null)
            {
                if (node.value < Node.value)
                    node = Node;
            }
            else
            {
                node = Node;
            }
        }
        // put the chess in this position with minmum value
        if (ChessBoard.Instacne.ChessPos(node.pos))
        {
            ChessBoard.Instacne.timer = 0;
        }
    }

    #endregion

    #region MinMax Algorithm
    List<MiniMaxNode> GetList(int[,] grid, int chess, bool mySelf)
   
    {
        List<MiniMaxNode> nodeList = new List<MiniMaxNode>();
        MiniMaxNode node;

        //loop through all the position in the board
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                // this postion have chess then skip
                int[] pos = new int[2] { i, j };
                if (grid[pos[0], pos[1]] != 0) continue;

                //add a node
                node = new MiniMaxNode();
                node.pos = pos;
                node.chess = chess;
               
                if (mySelf)
                {
                    //for human player
                    //look for max value
                    node.value = Score(grid, pos);
                }
                 
                else
                {
                    //for AI player
                    //look for min value
                    node.value = -Score(grid, pos);
                }

                //four directions to put chess
                //if less then four we add node to it
                if (nodeList.Count < 4)
                {
                    nodeList.Add(node);
                }

                //if number of nodes more then four we need to compare it
                else
                {
                    //check each node in nodelist
                    //to compare which one should keep or remove
                    foreach (var item in nodeList)
                    {
                        if (mySelf)
                        {
                            if (node.value > item.value)
                            {
                                nodeList.Remove(item);
                                nodeList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            if (node.value < item.value)
                            {
                                nodeList.Remove(item);
                                nodeList.Add(node);
                                break;
                            }
                        }
                    }
                }
            }
        }

        return nodeList;
    }
    #endregion

    #region
    public void CreateTree(MiniMaxNode node, int[,] grid, int deep, bool mySelf)
    {
        //if five chess together we can stop creating tree
        if (deep == 0 || node.value == float.MaxValue)
        {
            return;
        }

        grid[node.pos[0], node.pos[1]] = node.chess;
                                           
        node.childNode = GetList(grid, node.chess, !mySelf);
        foreach (var item in node.childNode)
        {
            //every child node continu to create tree
            //use clone to copy and save every gird
            CreateTree(item, (int[,])grid.Clone(), deep - 1, !mySelf);
        }
    }
    #endregion

    #region Alpha-Beta Pruning Algorithm
    public float AlphaBeta(MiniMaxNode node, int deep, bool mySelf, float alpha, float beta)
    {

        if (deep == 0 || node.value == float.MaxValue || node.value == float.MinValue)
        {
            return node.value;
        }
        //if is the player's turn, we need to find the max value position
        if (mySelf)
        {
            foreach (var child in node.childNode)
            {
                //look for max value,  comparing to the Alpha value
                alpha = Mathf.Max(alpha, AlphaBeta(child, deep - 1, !mySelf, alpha, beta));

                //alpha
                if (alpha >= beta)
                {
                    return alpha;
                }

            }
            return alpha;
        }

        // AI turn, look for the min vlaue position
        else
        {
            foreach (var child in node.childNode)
            {
                beta = Mathf.Min(beta, AlphaBeta(child, deep - 1, !mySelf, alpha, beta));

                //beta
                if (alpha >= beta)
                {
                    return beta;
                }

            }
            return beta;
        }
    }
    #endregion

    #region Score Form
    public float Score(int[,] grid, int[] pos)
    {
        float score = 0;
        //we don' know who would be the first one so
        //we need to calculate black and white condition at the same time
        score += checkFive(grid, pos, new int[2] { 1, 0 }, 1);
        score += checkFive(grid, pos, new int[2] { 1, 1 }, 1);
        score += checkFive(grid, pos, new int[2] { 1, -1 }, 1);
        score += checkFive(grid, pos, new int[2] { 0, 1 }, 1);

        score += checkFive(grid, pos, new int[2] { 1, 0 }, 2);
        score += checkFive(grid, pos, new int[2] { 1, 1 }, 2);
        score += checkFive(grid, pos, new int[2] { 1, -1 }, 2);
        score += checkFive(grid, pos, new int[2] { 0, 1 }, 2);

        return score;
    }
    #endregion

    #region
    public float checkFive(int[,] grid, int[] pos, int[] offset, int chess)

    {
        float score = 0;
        bool leftside = true, leftStop = false, rightStop = false;
        int Num = 1;
        string str = "a";
        int ri = offset[0], rj = offset[1];
        int li = -offset[0], lj = -offset[1];
        while (Num < 7 && (!leftStop || !rightStop))
        {
            if (leftside)
            {
                //leftside
                if ((pos[0] + li >= 0 && pos[0] + li < 15) &&
            pos[1] + lj >= 0 && pos[1] + lj < 15 && !leftStop)
                {
                    if (grid[pos[0] + li, pos[1] + lj] == chess)
                    {
                        Num++;
                        str = "a" + str;

                    }
                    else if (grid[pos[0] + li, pos[1] + lj] == 0)
                    {
                        Num++;
                        str = "_" + str;
                        if (!rightStop) leftside = false;
                    }
                    else
                    {
                        leftStop = true;
                        if (!rightStop) leftside = false;
                    }
                    li -= offset[0]; lj -= offset[1];
                }
                else
                {
                    leftStop = true;
                    if (!rightStop) leftside = false;
                }
            }
            else
            {
                if ((pos[0] + ri >= 0 && pos[0] + ri < 15) &&
          pos[1] + rj >= 0 && pos[1] + rj < 15 && !leftside && !rightStop)
                {
                    if (grid[pos[0] + ri, pos[1] + rj] == chess)
                    {
                        Num++;
                        str += "a";

                    }
                    else if (grid[pos[0] + ri, pos[1] + rj] == 0)
                    {
                        Num++;
                        str += "_";
                        if (!leftStop) leftside = true;
                    }
                    else
                    {
                        rightStop = true;
                        if (!leftStop) leftside = true;
                    }
                    ri += offset[0]; rj += offset[1];
                }
                else
                {
                    rightStop = true;
                    if (!leftStop) leftside = true;
                }
            }
        }

        string cmpStr = "";
        foreach (var keyInfo in chessScore)
        {
            if (str.Contains(keyInfo.Key))
            {
                if (cmpStr != "")
                {
                    if (chessScore[keyInfo.Key] > chessScore[cmpStr])
                    {
                        cmpStr = keyInfo.Key;
                    }
                }
                else
                {
                    cmpStr = keyInfo.Key;
                }
            }
        }

        if (cmpStr != "")
        {
            score += chessScore[cmpStr];
        }
        return score;
    }
    #endregion
}