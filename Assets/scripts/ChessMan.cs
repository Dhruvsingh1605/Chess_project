using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMan : MonoBehaviour
{
    // Reference
    public GameObject Controller;
    public GameObject movePlate;

    // position
    private int xBoard = -1;
    private int yBoard = -1;

    // variable to keep track "white" player or "black" player
    private string player;

    // Reference to all sprites that chesspiece cab be
    public Sprite  black_bishop, black_king, black_queen, black_pawn, black_knight, black_rook;
    public Sprite  white_bishop, white_king, white_queen, white_pawn, white_knight, white_rook; 

    public void Activate()
    {
        Controller = GameObject.FindGameObjectWithTag("GameController");
        

        SetCoord();

        switch(this.name)
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; break;

            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; break;
        }
    }

    public void SetCoord()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x, y,-0.1f);
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    private void OnMouseUp()
    {
        DestroyMovePlates();

        InitiateMovePlates();
    }


    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectWithTag("MovePlate");
        for (int i=0; i< movePlate.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1,0);
                LineMovePlate(0, -1);
                LineMovePlate(-1,-1);
                LineMovePlate(-1,1);
                LineMovePlate(1,-1);
                break;
            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;
            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1,1);
                LineMovePlate(1,-1);
                LineMovePlate(-1,1);
                LineMovePlate(-1, -1);
                break;
            case "black_king":
            case "white_king":
                SurroundMovePlate();
                break;
            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
            case "black_pawn":
                PawnMovePlate(xBoard, yBoard, -1);
                break;
            case "white_pawn":
                PawnMovePlate(xBoard, yBoard, +1);
                break;

        }
    }
    public void LineMovePlate(int xIncrement, int yIncrement)
        {
            Game sc = Controller.GetComponent<Game>();

            int x = xBoard + xIncrement;
            int y = yBoard + yIncrement;

            while (sc.PositionOnBoard(x,y) && sc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y);
                x += xIncrement;
                y += yIncrement;
            }
            if(sc.PositionOnBoard(x,y) && sc.GetComponent(x, y).GetComponent<ChessMan>().player != player)
            {
                MovePlateSpawn(x, y);
            }
        }
    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 2);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }
    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard - 0);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 0);
        PointMovePlate(xBoard + 1, yBoard + 1);
    }

    public void PointMovePlate(int x, int y)
    {
        Game sc = Controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x,y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if(cp == null)
            {
                MovePlateSpawn(x, y);
            } else if(cp.GetComponent<ChessMan>().player != player)
            {
                MoveAttackSwap(x, y);
            }
        }
    }

    public void PawnMovePlate(int x, int y)
    {
        Game sc = Controller.GetComponent<Game>();
        if(sc.PositionOnBoard(x, y))
        {
            if(sc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y);
            }

            if(sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) ! = null && sc.GetPosition(x - 1, y).GetComponent<ChessMan>().player != player)
            {
                MoveAttackSwap(x - 1, y);
            }
        }
    }
    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        myScript.SetReference(gameObject);
        myScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY, bool isattack = false)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        myScript.attack = true;
        myScript.SetReference(gameObject);
        myScript.SetCoords(matrixX, matrixY);
    }
}
