using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public AudioSource myFx;
    public AudioClip Hover;

    public void HoverSound()
    {
        myFx.PlayOneShot (Hover);
    }
}
