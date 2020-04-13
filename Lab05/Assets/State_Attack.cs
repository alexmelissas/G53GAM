using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_Attack : IState
{
    EnemyController owner;
    NavMeshAgent agent;

    public State_Attack(EnemyController owner) { this.owner = owner; }

    public void Enter()
    {
        Debug.Log("entering attack state");
        agent = owner.GetComponent<NavMeshAgent>();
        if (owner.seenTarget)
        {
            agent.destination = owner.lastSeenPosition;
            agent.acceleration = 16;
            agent.isStopped = false;
        }
    }

    public void Execute()
    {
        Debug.Log("updating attack state");
        agent.destination = owner.lastSeenPosition; agent.isStopped = false;
        if (!agent.pathPending && agent.remainingDistance < 5.0f)
        {
            agent.isStopped = true;
            
        }
        if (owner.seenTarget != true)
        {
            Debug.Log("lost sight, SEARCHING");
            owner.stateMachine.ChangeState(new State_Search(owner));
        }

        owner.FireOnPlayer();
    }

    public void Exit()
    {
        Debug.Log("exiting attack state");
        agent.isStopped = true;
    }
}
