using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Items : MonoBehaviour
{
    public int items;
    public int maxitems = 5;
    public Text textointerface;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.name == "player")
        {
            items++;
            textointerface.text = items.ToString() + " de un total de " + maxitems.ToString();
            Debug.Log("item tomado");

        }
        
        
        if (items > maxitems)
            SceneManager.LoadScene(0);      
        
    }

}
