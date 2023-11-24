using UnityEditor.Experimental.GraphView;
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
        FireTrigger();
    }
    public CreatureAI AI { get { return _ai; } }
    public NavMeshAgent Agent { get { return _agent; } }
    public string Name { get { return _animName; } }
    public abstract void StartState();
    public abstract void Update();
    public abstract void EndState();
    protected bool AIBrainReady()
    {
        if (_ai != null && _agent != null && _ai.isActiveAndEnabled && _agent.isActiveAndEnabled)
            return true;
        else return false;
    }
    public void FireTrigger()
    {
        _ai.Animator.SetTrigger(_animName);
    }
}

