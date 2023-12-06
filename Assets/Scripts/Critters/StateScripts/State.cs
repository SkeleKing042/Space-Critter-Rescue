
using UnityEngine;
using UnityEngine.AI;

public abstract class State
{
    private CreatureAI _ai;
    private NavMeshAgent _agent;
    private string _animName;
    public State(CreatureAI ai)
    {
        _ai = ai;
        _agent = _ai.GetAgent;
        _animName = this.GetType().ToString();
    }
    public CreatureAI AI { get { return _ai; } }
    public NavMeshAgent Agent { get { return _agent; } }
    public string Name { get { return _animName; } }
    public void BASE_StartState()
    {
        if(_ai.Animator != null) _ai.Animator.SetTrigger(_animName);
        StartState();
    }
    public abstract void StartState();
    public abstract void Update();
    public void BASE_EndState()
    {
        if (_ai.Animator != null) _ai.Animator.ResetTrigger(_animName);
        EndState();
    }
    public abstract void EndState();
    protected bool AIBrainReady()
    {
        if (_ai != null && _agent != null && _ai.isActiveAndEnabled)
            return true;
        else return false;
    }
}

