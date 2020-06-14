public class BrushAgent : SimpleAgent
{
    protected override void StartWorkAnimation()
    {
        _currentWorkAnimation = "brush";
        animator.SetBool(_currentWorkAnimation, true);
        workAudio.Play();
    }
}