//Created by Jackson Lucas
//Last Edited by Jackson Lucas

public class StunnedState : State
{
    public StunnedState(CreatureAI ai) : base(ai)
    {

    }
    public override void StartState()
    {
        if (AIBrainReady())
        {
            AI.Animator.SetBool("StunnedState", true);
            AI.RigidMode(true);
            AI.Rb.useGravity = true;
            AI.PrepareUpdateState(new IdleState(AI), 5);
        }
    }
    public override void Update()
    {

    }
    public override void EndState()
    {
        if (AIBrainReady())
        {
            AI.Animator.SetBool("StunnedState", false);
            AI.RigidMode(false);
            AI.Rb.useGravity = false;
        }
    }

}

