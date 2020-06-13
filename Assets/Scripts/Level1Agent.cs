using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Agent : SimpleAgent
{
    protected override void StartWorkAnimation()
    {
        _currentWorkAnimation = "saw";
        animator.SetBool(_currentWorkAnimation, true);
        workAudio.Play();
    }
}
