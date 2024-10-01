using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCutScene : MonoBehaviour
{
    [SerializeField] private Animator thanksPanelAnim;

    void Start()
    {
        StartCoroutine(WaitCutsceneToEnd());
    }

    private IEnumerator WaitCutsceneToEnd()
    {
        yield return new WaitForSeconds(6.0f);

        thanksPanelAnim.SetTrigger("PopUp");
    }

    public void ReturnToMainTitle()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayButtonSFX();
        }

        SceneManager.LoadScene(0);
    }
}
