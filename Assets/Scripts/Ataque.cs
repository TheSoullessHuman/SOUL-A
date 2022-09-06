using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ataque : MonoBehaviour
{
    private Animator anim;
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {

        

        //cooldown time
        if (Time.time > nextFireTime)
        {
            // Check for mouse input
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();

            }
        }
    }

    void OnClick()
    {
        /*
        lastClickedTime = Time.time;
        noOfClicks++;
        if (noOfClicks == 1)
        {
            StartCoroutine(MyCoroutine());
            anim.SetBool("hit1", true);
        }
        */

        StartCoroutine(MyCoroutine());
        anim.SetBool("hit1", true);


    }
    IEnumerator MyCoroutine()
    {
        yield return new WaitForSeconds(1.3f);
        anim.SetBool("hit1", false);
        print("Después de 2 segundos llegamos a aquí");

    }
}
