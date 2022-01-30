using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        Debug.Log("Loading scene: Playground");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Playground");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
