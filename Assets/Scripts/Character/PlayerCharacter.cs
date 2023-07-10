using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCharacter : Character
{

    private Vector3 m_targetPosition = Vector3.zero;
    private Vector2 m_lastJoystickDirection;
    private Vector3 m_lastValidPosition = Vector3.zero;

    public Shop currentShop;
    [SerializeField] [ReadOnly] private float shopTimer;
    [SerializeField] private float shopTimeToBuy;

    protected override void Start()
    {
        base.Start();
        
        m_targetPosition = transform.position;
        m_lastValidPosition = transform.position;
    }

    

    private void Move(Vector2 moveDirection)
    {
        Vector3 moveDir = new Vector3(moveDirection.x, 0f, moveDirection.y);

        Vector3 moveOffset = moveDir * Time.fixedDeltaTime * m_moveSpeed;
        moveOffset.y = 0f;

        m_targetPosition = transform.position + moveOffset;

        if (CanWalk(m_targetPosition))
        {
            m_lastValidPosition = transform.position;
            MoveTo(m_targetPosition);
        }
        else
        {
            m_rigidbody.velocity = Vector3.zero;
            transform.position = m_lastValidPosition;
            m_targetPosition = transform.position + moveOffset.z * Vector3.forward;
            if (CanWalk(m_targetPosition))
                MoveTo(m_targetPosition);
            else
            {
                m_targetPosition = transform.position + moveOffset.x * Vector3.right;
                if (CanWalk(m_targetPosition))
                    MoveTo(m_targetPosition);
                else
                    StopMoving();
            }
        }
        transform.forward = Vector3.Lerp(transform.forward, moveDir, 0.3f);
    }


    private void StopMoving()
    {
        m_targetPosition = transform.position;
        m_lastJoystickDirection = Vector2.zero;
        m_moveSpeed = 0f;

        m_rigidbody.velocity = new Vector3(0f, m_rigidbody.velocity.y, 0f);
    }
    
    
    protected override void FixedUpdate()
    {
        float maxSpeedModified = m_maxSpeed;
        foreach (Modifiers item in modifiers)
        {
            if (item.modifierType == EModifierType.MovementSpeed)
            {
                maxSpeedModified *= item.modifierAmount;
            }
        }
        if (Joystick.Instance.moveDirection != Vector2.zero)
        {
            m_moveSpeed = Mathf.Lerp(m_moveSpeed, maxSpeedModified, 0.125f);
            Move(Joystick.Instance.moveDirection);
            m_lastJoystickDirection = Joystick.Instance.moveDirection;
        }
        else
        {
            m_lastJoystickDirection = Vector2.Lerp(m_lastJoystickDirection, Vector2.zero, 0.3f);
            m_moveSpeed = Mathf.Lerp(m_moveSpeed, 0f, 0.3f);
            if (m_lastJoystickDirection == Vector2.zero)
            {
                StopMoving();
            }
            else
            {
                Vector3 forward = transform.forward;
                Move(m_lastJoystickDirection);
                transform.forward = forward;
            }
        }
        RessourcesHandle();
        ShopHandle();
        base.FixedUpdate();
        
        if (m_rigidbody.velocity.y > 0)
        {
            m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, m_rigidbody.velocity.y / 2f, m_rigidbody.velocity.z);
        }
    }

    private void ShopHandle()
    {
        if (shopTimer <= shopTimeToBuy)
        {
            shopTimer += Time.fixedDeltaTime;
        }
        else
        {
            if (currentShop != null && GameManager.Instance.GetRessourcesOfType(currentShop.ressourceToUse)>0)
            {
                currentShop.AddAmount(1);
                shopTimer = 0;
            }
        }
    }
}
