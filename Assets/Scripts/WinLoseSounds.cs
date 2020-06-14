using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseSounds : MonoBehaviour
{
    public AudioSource win;
    public AudioSource lose;

    public void Win()
    {
        win.Play();
    }

    public void Lose()
    {
        lose.Play();
    }
}
