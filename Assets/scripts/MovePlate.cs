using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Controller;
    GameObject reference = null;


    int matrixX;
    int matrixY;

    public bool attack = false;

    public void Start()
    {
        GameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    }

    public void OnMouseUp() {
        {
            Controller = GameObject.FindGameObjectWithTag("GameController");
            if(attack)
            {
                GameObject cp = Controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

                Destroy(cp);
            }

            Controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<ChessMan>().GetXBoard(),reference.GetComponent<ChessMan>().GetYBoard());

            reference.GetComponent<ChessMan>().SetXBoard(matrixX);
            reference.GetComponent<ChessMan>().SetYBoard(matrixY);
            reference.GetComponent<ChessMan>().SetCoords();

            Controller.GetComponent<Game>().SetPosition(reference);

            reference.GetComponent<ChessMan>().DestroyMovePlates();
        }
    }
    public void SetCoords(int x, int y)
        {
            matrixX = x;
            matrixY = y;
        }

        public void SetReference(GameObject obj)
        {
            reference = obj;
        }

        public GameObject GetReference()
        {
            return reference;
        }
}
