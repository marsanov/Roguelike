using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] float waitToLoad = 1f;
    [SerializeField] string nextLevel;

    private void Awake()
    {
        instance = this;
    }

    //Public methods
    public IEnumerator LevelEnd()
    {
        AudioManager.instance.PlayLevelWin();
        UIController.instance.StartFadeIn();
        PlayerController.instance.canMove = false;
        yield return new WaitForSeconds(waitToLoad);
        SceneManager.LoadScene(nextLevel);
    }
}
