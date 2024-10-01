using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JigsawManager : MonoBehaviour
{
    private bool hasGameFinished;

    [SerializeField] private Transform[] pointTransform;   
    Dictionary <int, Vector3> pointDatas;

    [SerializeField] private JigsawPuzzleTile[] jigsawTiles;

    private GameObject currentHoldingPuzzle;
    private bool isHolding;

    [SerializeField] private Animator anim;

    private void Start()
    {
        hasGameFinished = false;
        isHolding = false;

        pointDatas = new Dictionary<int, Vector3>();
        for (int i = 0; i < pointTransform.Length; i++) 
        {
          pointDatas[i] = pointTransform[i].position;
        }

        Scatter();
    }

    private void Update()
    {
        if (hasGameFinished) return;

        if (currentHoldingPuzzle != null)
        {
            if (isHolding)
            {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentHoldingPuzzle.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
                currentHoldingPuzzle.GetComponent<SpriteRenderer>().sortingOrder = 4;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                currentHoldingPuzzle = hit.collider.gameObject;

                if (!currentHoldingPuzzle.GetComponent<JigsawPuzzleTile>().isInPlace)
                {
                    isHolding = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (currentHoldingPuzzle != null)
            {
                var tile = currentHoldingPuzzle.GetComponent<JigsawPuzzleTile>();

                if (pointDatas.TryGetValue(tile.index, out Vector3 pos))
                {
                    float distance = Vector3.Distance(currentHoldingPuzzle.transform.position, pos);

                    if (distance < 0.5f)
                    {
                        tile.targetPos = pos;
                        tile.isInPlace = true;
                    }
                    else
                    {
                        tile.targetPos = currentHoldingPuzzle.transform.position;
                    }
                }
                else
                {
                    tile.targetPos = currentHoldingPuzzle.transform.position;
                }

                tile.GetComponent<SpriteRenderer>().sortingOrder = 3;
            }

            currentHoldingPuzzle = null;
            isHolding = false;

            checkWin();
        }
    }
    private void Scatter()
    {
        float offset = 1.0f;

        float orthoHeight = Camera.main.orthographicSize;
        float screenAspect = (float)Screen.width / Screen.height;
        float orthoWidth = (screenAspect * orthoHeight);

        for (int i = 0; i < jigsawTiles.Length; i++)
        {
            jigsawTiles[i].index = i;

            float x = Random.Range(-orthoWidth + offset, orthoWidth - offset);
            float y = Random.Range(-orthoHeight + offset, orthoHeight - offset);
            jigsawTiles[i].targetPos = new Vector3(x, y, 0);
            Debug.Log(i);
        }
    }

    private void checkWin()
    {
        bool check = false;

        for (int i = 0; i < jigsawTiles.Length; i++)
        {
            if (jigsawTiles[i] != null)
            {
                if (jigsawTiles[i].isInPlace)
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
        }

        if (check)
        {
            hasGameFinished = true;

            StartCoroutine(WiningScene());
        }
    }

    private IEnumerator WiningScene()
    {
        anim.SetTrigger("Win");
        
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
