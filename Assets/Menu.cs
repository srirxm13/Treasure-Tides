using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject s1;
    public GameObject s2;

    public void StartGame()
    {
        mainMenu.SetActive(false);
        s1.SetActive(true);
        s2.SetActive(false);
    }

    public void Screen1()
    {
        mainMenu.SetActive(false);
        s1.SetActive(false);
        s2.SetActive(true);
    }

    public void Screen2()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
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
