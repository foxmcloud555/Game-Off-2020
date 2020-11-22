using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(GameObject button)
    {
        SceneManager.LoadScene(button.name);
    }

    public static TwineParser.CharacterStats PlayerStats;
    

}
