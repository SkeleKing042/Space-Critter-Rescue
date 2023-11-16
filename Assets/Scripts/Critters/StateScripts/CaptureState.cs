//Created by Jackson Lucas
//Last edited by Jackson Lucas

using UnityEngine;

public class CaptureState : State
{
    private float stateTime;
    public CaptureState(CreatureAI ai) : base(ai)
    {

    }
    public override void StartState()
    {
        AI.Animator.SetBool("CaptureState", true);
        AI.RigidMode(true);
    }
    public override void Update()
    {
        Quaternion targetRot = Quaternion.LookRotation(AI.Player.transform.position - AI.transform.position);
        AI.transform.rotation = Quaternion.Slerp(AI.transform.rotation, targetRot, AI.FacePlayerRate);
        Failsafe();
    }
    public override void EndState()
    {
        AI.Animator.SetBool("CaptureState", false);
        AI.RigidMode(false);
    }

    private void Failsafe()
    {
        stateTime += Time.deltaTime;
        if(stateTime >= 5)
        {
            AI.StunThenRun(2);
        }

    }

}
