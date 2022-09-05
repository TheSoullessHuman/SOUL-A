using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiempoBala : MonoBehaviour
{
    public GameObject Panel;
    public bool lento;
    public float Cooldown;
    public float NextUse;
    public float speedy;
    public Animator animator;
    

    // Start is called before the first frame update
    void Start()
    {
        lento = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        Activar();
        
    }

    void Activar()
    {
        if (Time.time > NextUse)
        {
            if (Input.GetKey(KeyCode.Q) && lento == false)
            {
                Panel.SetActive(true);
                Time.timeScale = 0.5f;

                speedy = 10;
                animator.SetFloat("Multiply", 2);
                


                Debug.Log("Lento");
                Invoke("Casteo", 2.5f);
            }

            else if (lento == true)
            {
                Panel.SetActive(false);
                Time.timeScale = 1f;

                speedy = 5;
                animator.SetFloat("Multiply", 1);
                

                desactivar();
                Debug.Log("No Lento");

                NextUse = Time.time + Cooldown;
            }
        }
    }

    void Casteo()
    {
        lento = true;
    }

    void desactivar()
    {
        lento = false;
    }


}
