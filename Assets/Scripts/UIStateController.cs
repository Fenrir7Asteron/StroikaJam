using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIStateController : MonoBehaviour
{
    public string nextScene; 
    public GameObject destMark;
    public GameObject buildbtn;

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

    public GameObject repeatBtn;
    public GameObject finishBtn;

    public GameObject finishText;

    public GameObject bigStar1;
    public GameObject bigStar2;
    public GameObject bigStar3;

    private float localTime = 0.0f;

    void Start()
    {
        levelSign.GetComponent<TextMeshProUGUI>().text = getLevelText(localTime);
        finishText.GetComponent<TextMeshProUGUI>().alpha = 0;
        bigStar1.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        bigStar2.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        bigStar3.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        repeatBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        repeatBtn.GetComponent<Button>().interactable = false;

        finishBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        finishBtn.GetComponent<Button>().interactable = false;

        if (sign1)
            sign1.GetComponent<SignTracker>().active = false;
        if (sign2)
            sign2.GetComponent<SignTracker>().active = false;
        if (sign3)
            sign3.GetComponent<SignTracker>().active = false;
    }

    void Update()
    {
        if (state == State.Gameplay)
            GameplayUpdate();
    }
    // Update is called once per frame
    void GameplayUpdate()
    {
        
        localTime += Time.deltaTime;

        if (Mathf.Floor(localTime - Time.deltaTime) != Mathf.Floor(localTime))
        {
            levelSign.GetComponent<TextMeshProUGUI>().text = getLevelText(localTime);
        }

        if (localTime >= timeForLevel)
        {
            FinishLevel();
        }
    }

    public void StartLevel()
    {
        state = State.Gameplay;
        destMark.GetComponent<MoveToClick>().active = true;
        Destroy(buildbtn);
        localTime = 0.0f;
        
        if (sign1)
            sign1.GetComponent<SignTracker>().active = true;
        if (sign2)
            sign2.GetComponent<SignTracker>().active = true;
        if (sign3)
            sign3.GetComponent<SignTracker>().active = true;
    }

    // This is the greatest amount of copypaste i've ever done :( 
    public void FinishLevel()
    {
        state = State.GameOver;
        destMark.GetComponent<MoveToClick>().active = false;
        finishText.GetComponent<TextMeshProUGUI>().alpha = 255;
        bigStar1.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        bigStar2.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        bigStar3.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        repeatBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        repeatBtn.GetComponent<Button>().interactable = true;

        finishBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        finishBtn.GetComponent<Button>().interactable = true;
        
        // Show up the star gauge
        int[] starStat = new int[3] { 3, 3, 3 };
        if (sign1 != null)
            starStat[0] = sign1.GetComponent<SignTracker>().StarCount();
        if (sign2 != null)
            starStat[1] = sign2.GetComponent<SignTracker>().StarCount();
        if (sign3 != null)
            starStat[2] = sign3.GetComponent<SignTracker>().StarCount();

        int minCount = 3;
        for (int i = 0; i < 3; i++)
            minCount = minCount > starStat[i] ? starStat[i] : minCount;

        if (minCount == 0)
            finishText.GetComponent<TextMeshProUGUI>().text = "Time is up";
        else
            finishText.GetComponent<TextMeshProUGUI>().text = "Plan complete";

        if (minCount > 0)
            bigStar1.GetComponent<SpriteBinState>().on = true;
        if (minCount > 1)
            bigStar2.GetComponent<SpriteBinState>().on = true;
        if (minCount > 2)
            bigStar3.GetComponent<SpriteBinState>().on = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(nextScene);
    }

    string getLevelText(float time)
    {
        return "LEVEL " + levelIndex + " :: " + (timeForLevel - Mathf.Floor(time)) + " SEC";
    }
}
