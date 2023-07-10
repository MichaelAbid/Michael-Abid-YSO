using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AICharacter : Character
{
    protected float lifePoint;
    public bool isActive;

    protected NavMeshAgent agent;

    protected Vector3 m_targetPosition = Vector3.zero;
    protected Vector3 m_lastValidPosition = Vector3.zero;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected void Move(Vector3 moveDirection)
    {
        float maxSpeedModified = m_maxSpeed;
        foreach (Modifiers item in modifiers)
        {
            if (item.modifierType == EModifierType.MovementSpeed)
            {
                maxSpeedModified *= item.modifierAmount;
            }
        }

        agent.speed = maxSpeedModified;

        agent.isStopped = false;
        agent.SetDestination(moveDirection);
        
    }

    protected void StopMoving()
    {
        m_targetPosition = transform.position;
        agent.isStopped = true;
    }

}
