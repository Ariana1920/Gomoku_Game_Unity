using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public chessState chessColor = chessState.Black;

    Button retractBtn;

    protected virtual void Start()
    {
        retractBtn = GameObject.FindGameObjectWithTag("RetractButton").GetComponent<Button>();
    }

    public virtual void Update()
    {
        if (chessColor == ChessBoard.Instacne.whoTurn && ChessBoard.Instacne.timer > 0.3f)
            putChess();
    }

    public virtual void putChess() 
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //change the point from screen to world
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //centre is (0,0) in worldpos, but is (7,7) to the chessboard
            if (ChessBoard.Instacne.ChessPos(new int[2] { (int)(pos.x + 7.5f), (int)(pos.y + 7.5f) }))
            {
                ChessBoard.Instacne.timer = 0;
            }
        }
    }

    protected virtual void changeInteract()
    {
        if (chessColor == chessState.Watch)
            return;
        if (ChessBoard.Instacne.whoTurn == chessColor)
            retractBtn.interactable = true;
        else
            retractBtn.interactable = false;
    }
}
