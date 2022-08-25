using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Pausa;
    public bool Pause;

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
    public void inicio()
    {
        SceneManager.LoadScene(1);

    }
}
