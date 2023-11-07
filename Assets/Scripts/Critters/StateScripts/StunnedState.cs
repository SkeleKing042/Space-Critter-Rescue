//Created by Jackson Lucas
//Last Edited by Jackson Lucas

public class StunnedState : State
{
    public StunnedState(CreatureAI ai) : base(ai)
    {

    }
    public override void StartState()
    {
        AI.Animator.SetBool("StunnedState", true);
        AI.RigidMode(true);
        AI.Rigidbody.useGravity = true;
    }
    public override void Update()
    {

    }
    public override void EndState()
    {
        AI.Animator.SetBool("StunnedState", false);
        AI.RigidMode(false);
    }

}

