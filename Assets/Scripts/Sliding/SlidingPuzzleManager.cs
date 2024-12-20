using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class SlidingPuzzleManager : MonoBehaviour
{
    private bool hasGameFinished;

    [SerializeField] private PuzzleTile[] puzzleTiles;

    Dictionary<Vector2, int> initialMapPosData;

    private GapData puzzleGap;

    private GameObject destroyTile;

    [SerializeField] private Animator anim;

    void Start()
    {
        hasGameFinished = false;

        puzzleGap = new GapData();

        StoreInitialData();

        DestroyRandomTile();

        Shuffle();
    }

    void Update()
    {
        if (hasGameFinished) return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                var hitTile = hit.collider.gameObject.GetComponent<PuzzleTile>();

                if (CheckIfCanMove(hitTile))
                { 
                    // Store new gap data
                    Vector3 tempWPos = hitTile.targetPos;
                    Vector2 tempMatPos = hitTile.matrixPos;

                    //Move pressed puzzle
                    hitTile.targetPos = puzzleGap.currentPos;

                    //Store new data for pressed puzzle
                    hitTile.targetPos = puzzleGap.currentPos;
                    hitTile.matrixPos = puzzleGap.matrixPos;

                    //Update mapPosData
                    //mapPosData[puzzleGap.matrixPos] = hitTile.tileName;

                    UpdatePuzzleGap(tempWPos, tempMatPos);

                    CheckIfWin();
                }
            }
        }
    }

    private void UpdatePuzzleGap(Vector3 currentPos, Vector2 matrixPos)
    {
        puzzleGap.currentPos = currentPos;
        puzzleGap.matrixPos = matrixPos;
    }

    private void DestroyRandomTile()
    {
        UpdatePuzzleGap(puzzleTiles[15].targetPos, puzzleTiles[15].matrixPos);
        initialMapPosData[puzzleTiles[15].matrixPos] = -1;

        puzzleTiles[15].tileName = "gap";
        puzzleTiles[15].targetPos = new Vector3(0, 0, 0);
        puzzleTiles[15].matrixPos = new Vector2(0, 0);

        destroyTile = puzzleTiles[15].gameObject;

        //Destroy(puzzleTiles[15].gameObject);
        destroyTile.SetActive(false);
    }

    private bool CheckIfCanMove(PuzzleTile tile)
    {
        bool check = false;

        float xDiff = puzzleGap.currentPos.x - tile.targetPos.x;
        float yDiff = puzzleGap.currentPos.y - tile.targetPos.y;

        if (Mathf.Abs(xDiff) == 2.25 && yDiff == 0)
        {
            check = true;
        }
        else if (Mathf.Abs(yDiff) == 2.25 && xDiff == 0)
        {
            check = true;
        }

        return check;
    }

    private void StoreInitialData()
    {
        initialMapPosData = new Dictionary<Vector2, int>();

        int count = 0;

        for (int i = 1; i <= 4; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                puzzleTiles[count].matrixPos = new Vector2(i, j);
                puzzleTiles[count].index = count + 1;

                initialMapPosData.Add(puzzleTiles[count].matrixPos, puzzleTiles[count].index);

                count++;
            }
        }
    }

    private void Shuffle()
    {
        int invertion;

        do
        {
            for (int i = 0; i < puzzleTiles.Length - 1; i++)
            {
                if (puzzleTiles[i] != null)
                {
                    var tempPos = puzzleTiles[i].targetPos;
                    var tempMat = puzzleTiles[i].matrixPos;

                    int randomIndex = Random.Range(0, puzzleTiles.Length - 2);

                    puzzleTiles[i].targetPos = puzzleTiles[randomIndex].targetPos;
                    puzzleTiles[i].matrixPos = puzzleTiles[randomIndex].matrixPos;

                    puzzleTiles[randomIndex].targetPos = tempPos;
                    puzzleTiles[randomIndex].matrixPos = tempMat;

                    var tile = puzzleTiles[i];
                    puzzleTiles[i] = puzzleTiles[randomIndex];
                    puzzleTiles[randomIndex] = tile;
                }
            }
            invertion = GetInversion();
        }while(invertion % 2 != 0);
        
    }

    private int GetInversion()
    {
        int inversionSum = 0;

        for (int i = 0; i < puzzleTiles.Length; i++)
        {
            int sum = 0;
            for(int j = i; j < puzzleTiles.Length; j++)
            {
                if(puzzleTiles[j] != null)
                {
                    if(puzzleTiles[i].index > puzzleTiles[j].index)
                    {
                        sum++;
                    }
                }
            }
            inversionSum += sum;
        }

        return inversionSum;
    }

    private void CheckIfWin()
    {
        bool isWin = false;

        for (int i = 0; i < puzzleTiles.Length; i++)
        {
            if (initialMapPosData.TryGetValue(puzzleTiles[i].matrixPos, out int initialValue))
            {
                if (initialValue == puzzleTiles[i].index)
                {
                    isWin = true;
                }
                else
                {
                    isWin = false;
                    break;
                }
            }
        }

        if (isWin)
        {
            hasGameFinished = true;

            destroyTile.SetActive(true);

            Debug.Log("Win");

            StartCoroutine(WiningScene());
        }
    }

    private IEnumerator WiningScene()
    {
        anim.SetTrigger("Win");

        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

public class GapData
{
    public Vector3 currentPos;
    public Vector2 matrixPos;
}