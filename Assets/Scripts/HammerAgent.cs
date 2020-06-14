using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerAgent : SimpleAgent
{
    protected override void StartWorkAnimation()
    {
        _currentWorkAnimation = "hammer";
        animator.SetBool(_currentWorkAnimation, true);
        workAudio.Play();
    }
}