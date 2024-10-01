using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_Cutscene : MonoBehaviour
{
    [SerializeField] private Animator cutsceneAnim;
    [SerializeField] private Animator mainTitleAnim;
    [SerializeField] private GameObject mainTilePanel;

    public void StartGame()
    {
        StartCoroutine(ClosePanel());
    }

    private IEnumerator ClosePanel()
    {
        if(AudioManager.instance != null)
        {
            AudioManager.instance.PlayButtonSFX();
        }
        
        mainTitleAnim.SetTrigger("StartGame");

        yield return new WaitForSeconds(1.0f);

        mainTilePanel.SetActive(false);

        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        yield return null;

        cutsceneAnim.SetTrigger("Begin");

        yield return new WaitForSeconds(15.0f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
