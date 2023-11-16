//Created by Jackson Lucas
//Last edited by Jackson Lucas

using UnityEngine;

public class DrinkingState : State
{
    public DrinkingState(CreatureAI ai) : base(ai)
    {
    }
    public override void StartState()
    {
        //Stop moving
        Agent.isStopped = true;
        AI.Animator.SetBool("DrinkingState", true);
    }
    public override void Update()
    {
        //Replenish hydration
        if (AI.GetComponent<FieldOfView>().CanSeeTarget)
        {
            AI.RunFromPlayer(1);
            return;
        }
        if (AI.Hydration >= 100)
        {
            AI.PrepareUpdateState(new IdleState(AI));
        }
        else
            Mathf.Clamp(AI.Hydration += Time.deltaTime * 10, 0, 100);
    }
    public override void EndState()
    {
        AI.GoingForDrink = false;
        Agent.isStopped = false;
        AI.Animator.SetBool("DrinkingState", false);
    }
}

