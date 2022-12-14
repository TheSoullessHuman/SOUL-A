using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Pausa;
    public GameObject GameOverScreen;
    public bool Pause;
    public static GameManager gameOverManager;

    // Start is called before the first frame update
    void Start()
    {
        Pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        Pausar();
    }

    void Pausar()
    {
        if (Input.GetKeyDown(KeyCode.P) && Pause == false)
        {
            Pausa.SetActive(true);
            Time.timeScale = 0f;
            Pause = true;

        }
        else if (Input.GetKeyDown(KeyCode.P) && Pause == true)
        {
            Pausa.SetActive(false);
            Time.timeScale = 1f;
            Pause = false;
        }
    }

    public void reanudar()
    {
        Pausa.SetActive(false);
        Time.timeScale = 1;
        Pause = false;
    }

    public void inicio()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
        GameOverScreen.SetActive(false);
        

    }
    public void Gameover()
    {
        //StartCoroutine(MyCoroutine());
        GameOverScreen.SetActive(true);
        Time.timeScale= 0.5f;
        
    }
    public void salir()
    {
        Application.Quit();
        Debug.Log("quiting game");
    }
    IEnumerator MyCoroutine()
    {
        yield return new WaitForSeconds(1.3f);
        Time.timeScale = 0f;

    }

}
