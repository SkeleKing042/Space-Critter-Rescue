//Created by Jackson Lucas
//Last edited by Jackson Lucas

using UnityEngine;

public class AlertState : State
{
    public AlertState(CreatureAI ai) : base(ai)
    {

    }
    public override void StartState()
    {
        AI.Animator.SetBool("AlertState", true);
        AI.RigidMode(false);
        AI.GetAgent.isStopped = true;
    }
    public override void Update()
    {
        Quaternion targetRot = Quaternion.LookRotation(AI.Player.transform.position - AI.transform.position);
        AI.transform.rotation = Quaternion.Slerp(AI.transform.rotation, targetRot, AI.FacePlayerRate);
    }
    public override void EndState()
    {
        AI.Animator.SetBool("AlertState", false);
    }

}
