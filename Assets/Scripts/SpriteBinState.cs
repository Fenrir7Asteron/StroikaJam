using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpriteBinState : MonoBehaviour
{
    Image image;

    public bool on = false;

    public Sprite deactivated;
    public Sprite activated;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (on)
            image.sprite = activated;
        else
            image.sprite = deactivated;
    }
}
