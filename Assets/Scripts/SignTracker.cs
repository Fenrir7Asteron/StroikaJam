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
    int firstStarAt = 10;

    [SerializeField]
    int secondStarAt = 12;

    [SerializeField]
    int thirdStartAt = 15;
    
    public int currentCount = 0;
    int prevCurrentCount = -1;

    
    void Update()
    {
        star1.GetComponent<SpriteBinState>().on = currentCount >= firstStarAt;
        star2.GetComponent<SpriteBinState>().on = currentCount >= secondStarAt;
        star3.GetComponent<SpriteBinState>().on = currentCount >= thirdStartAt;

        if (prevCurrentCount != currentCount)
        {
            int cap = currentCount > secondStarAt ? thirdStartAt : currentCount > firstStarAt ? secondStarAt : firstStarAt;
            progressText.GetComponent<TextMeshProUGUI>().text = currentCount + "/" + cap;
            prevCurrentCount = currentCount;
        }
    }

    public int StarCount()
    {
        return currentCount >= thirdStartAt ? 3 : 
               currentCount >= secondStarAt ? 2 : 
               currentCount >= firstStarAt  ? 1 : 
                                              0 ;
    }
}
