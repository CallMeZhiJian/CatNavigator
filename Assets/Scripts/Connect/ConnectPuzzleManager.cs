using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectPuzzleManager : MonoBehaviour
{
    private bool hasGameFinished;
    private Camera m_Camera;

    [SerializeField] private Pipe[] pipes;

    private void Start()
    {
        m_Camera = Camera.main;

        checkFill();
    }

    private void Update()
    {
        if (hasGameFinished) return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(m_Camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                Pipe thisPipe = hit.collider.gameObject.GetComponent<Pipe>();

                thisPipe.RotateSelf();

                thisPipe.checkRot();

                checkFill();

                checkWin();
            }
        }
    }

    private void checkFill()
    {
        if (pipes[0].isInPlace)
        {
            pipes[0].isFilled = true;
        }
        else
        {
            pipes[0].isFilled = false;
        }

        for (int i = 1; i < pipes.Length; i++)
        {
            if (pipes[i].isInPlace)
            {
                if (pipes[i - 1] != null)
                {
                    if(pipes[i - 1].isFilled)
                    {
                        pipes[i].isFilled = true;
                    }
                    else
                    {
                        pipes[i].isFilled = false;
                    }
                }
                else
                {
                    pipes[i].isFilled = true;
                }
            }
            else
            {
                pipes[i].isFilled= false;
            }
        }
    }

    private void checkWin()
    {
        bool check = false;

        for (int i = 0; i < pipes.Length; i++)
        {
            if (pipes[i].isInPlace)
            {
                check = true;
                continue;
            }
            else
            {
                check = false;
                break;
            }
        }

        if (check)
        {
            Debug.Log("win");
        }
    }
}