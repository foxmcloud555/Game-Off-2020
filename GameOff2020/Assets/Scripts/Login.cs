using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public AudioSource loginSound;
    public void LogPlayerIn()

    {
        //loginSound.Play();
        SceneManager.LoadScene("Home");
    }
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
