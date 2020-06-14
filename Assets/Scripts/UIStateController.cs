using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIStateController : MonoBehaviour
{
    enum State {
        Build, Gameplay, GameOver, NextLevel
    }

    private State state;

    public int levelIndex = 1;
    public int timeForLevel = 30;
    public GameObject levelSign;
    public GameObject sign1;
    public GameObject sign2;
    public GameObject sign3;

    private float localTime = 0.0f;

    void Start()
    {
        levelSign.GetComponent<TextMeshProUGUI>().text = getLevelText(localTime);
    }

    // Update is called once per frame
    void Update()
    {
        localTime += Time.deltaTime;

        if (Mathf.Floor(localTime - Time.deltaTime) != Mathf.Floor(localTime))
        {
            levelSign.GetComponent<TextMeshProUGUI>().text = getLevelText(localTime);
        }
    }

    string getLevelText(float time)
    {
        return "LEVEL " + levelIndex + " :: " + (timeForLevel - Mathf.Floor(time)) + " SEC";
    }
}
