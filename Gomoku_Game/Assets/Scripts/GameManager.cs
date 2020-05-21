using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

    public List<Player> Player = new List<Player>();

    private void Awake()
    {
        int player1 =PlayerPrefs.GetInt("Player1"); //黑旗
        int player2 =PlayerPrefs.GetInt("Player2");//白棋

        //PlayerPrefs.SetInt("Double", 1);
        for (int i = 0; i < Player.Count; i++)
        {
            if (player1 == i)
            {
                Player[i].chessColor = chessState.Black;
            }
            else if (player2 == i)
            {
                Player[i].chessColor = chessState.White;
            }
            else
            {
                Player[i].chessColor = chessState.Watch;

            }
        }
    }

    public void SetPlayer1(int i)
    {
        PlayerPrefs.SetInt("Player1",i);
    }

    public void SetPlayer2(int i)
    {
        PlayerPrefs.SetInt("Player2", i);

    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayNetGame()
    {
        SceneManager.LoadScene(2);
    }

    public void ChangeChessColor()
    {
        for (int i = 0; i < Player.Count; i++)
        {
            if (Player[i].chessColor == chessState.Black)
            {
                SetPlayer2(i);
            }
            else if ((Player[i].chessColor == chessState.White))
            {
                SetPlayer1(i);
            }
            else
            {
                Player[i].chessColor = chessState.Watch;
            }
        }
        SceneManager.LoadScene(1);
    }

    public void DoubleMode()
    {
        
        PlayerPrefs.SetInt("Double", 10);
    }

    public void OnReturnBtn()
    {
        PlayerPrefs.SetInt("Double", 1);
        SceneManager.LoadScene(0);
    }

    public void OnRelayBtn()
    {
        SceneManager.LoadScene(1);
    }

    public void ReturnBtn()
    {
        SceneManager.LoadScene(0);

    }
}
