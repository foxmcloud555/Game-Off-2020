using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TentaclesBehaviour : MonoBehaviour
{
    public GameObject browser;
    public GameObject background;
    public GameObject tentacles;

    public static bool tentaclesPlayed;

    public void ActivateTentacles()
    {
        browser.SetActive(false);
        background.SetActive(false);
        tentacles.SetActive(true);
        StartCoroutine(LoadHomeAgain());
    }


    private IEnumerator LoadHomeAgain()
    {
        yield return new WaitForSeconds(15);
        tentaclesPlayed = true;
        SceneManager.LoadScene("Home");
    }
}
