using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteAudio : MonoBehaviour
{
    public GameObject soundButton;
    public GameObject noSoundButton;

    public void MuteAudioAndChangeButton()
    {
        AudioListener.volume = 0;
        soundButton.SetActive(false);
        noSoundButton.SetActive(true);
    }

    public void UnMuteAudioAndChangeButton()
    {
        AudioListener.volume = 1;
        noSoundButton.SetActive(false);
        soundButton.SetActive(true);
    }
}
