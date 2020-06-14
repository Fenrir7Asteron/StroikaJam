using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignTracker : MonoBehaviour
{
    [SerializeField]
    GameObject star1;

    [SerializeField]
    GameObject star2;

    [SerializeField]
    GameObject star3;

    [SerializeField]
    GameObject progressText;

    [SerializeField]
    int firstStarAt = 3;

    [SerializeField]
    int secondStarAt = 6;

    public int total = 10;
    public int currentCount = 0;
    int prevCurrentCount = -1;

    
    void Update()
    {
        if (currentCount > total)
            currentCount = total;

        star1.GetComponent<SpriteBinState>().on = currentCount >= firstStarAt;
        star2.GetComponent<SpriteBinState>().on = currentCount >= secondStarAt;
        star3.GetComponent<SpriteBinState>().on = currentCount == total;

        if (prevCurrentCount != currentCount)
        {
            progressText.GetComponent<TextMeshProUGUI>().text = currentCount + "/" + total;
            prevCurrentCount = currentCount;
        }
    }

    public int StarCount()
    {
        return currentCount >= total        ? 3 : 
               currentCount >= secondStarAt ? 2 : 
               currentCount >= firstStarAt  ? 1 : 
                                              0 ;
    }
}
