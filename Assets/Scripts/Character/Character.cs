using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    [SerializeField] protected float m_maxSpeed = 1f;
    [SerializeField] protected Animator m_animator;
    protected Rigidbody m_rigidbody;
    protected Vector3 m_lastPosition = Vector3.zero;
    protected float m_moveSpeed = 1f;
    protected Vector3 m_animationDirection = Vector3.zero;

    public float attackPerSeconds = 1;
    public int strength = 1;
    private float attackTimer=10f;

    public List<Modifiers> modifiers = new List<Modifiers>();

    public List<Ressource> nearRessources = new List<Ressource>();
    protected virtual void Start()
    {
        m_lastPosition = transform.position;
        m_rigidbody = GetComponent<Rigidbody>();
    }
        
    protected virtual bool CanWalk(Vector3 position)
    {
        return IsGrounded(position);
    }

    protected virtual bool IsGrounded(Vector3 targetPos)
    {
        targetPos += Vector3.up;
        RaycastHit hit;
        if (Physics.Raycast(targetPos, -Vector3.up, out hit, 10f, LayerMask.NameToLayer("Ground")))
        {
            float dst = hit.distance;
            return dst < 10f;
        }
        return false;
    }

    protected virtual void FixedUpdate()
    {
        ModifierHandle();
        AnimationHandler();
    }

    protected virtual void MoveTo(Vector3 position)
    {
        transform.position = position;
        /*Vector3 direction = (position - transform.position);

        Vector3 vel = direction / Time.fixedDeltaTime;
        vel.y = m_rigidbody.velocity.y;
        m_rigidbody.velocity = vel;*/
    }

    private void AnimationHandler()
    {
        m_animator.SetFloat("Speed", m_moveSpeed);
    }

    protected int SortByDistance(Ressource a, Ressource b)
    {
        if (a == null || b == null)
        {
            return a == b ? 0 : (a == null ? -1 : 1);
        }
        float dstA = (a.transform.position - transform.position).sqrMagnitude;
        float dstB = (b.transform.position - transform.position).sqrMagnitude;
        return dstA.CompareTo(dstB);
    }


    protected void ModifierHandle()
    {
        List<int> indexToRemove = new List<int>();
        int i = 0;
        foreach (Modifiers item in modifiers)
        {
            if (!item.infinite)
            {
                item.modifierTime -= Time.fixedDeltaTime;
                if (item.modifierTime <= 0)
                {
                    indexToRemove.Add(i);
                }
            }
            i++;
        }

        foreach (int item in indexToRemove)
        {
            modifiers.RemoveAt(item);
        }
    }


    protected void RessourcesHandle()
    {

        float AttackPerSecondsModified = attackPerSeconds;
        foreach (Modifiers item in modifiers)
        {
            if(item.modifierType == EModifierType.AttackSpeedModifier)
            {
                AttackPerSecondsModified *= item.modifierAmount;
            }
        }
        m_animator.SetFloat("ChoppingSpeed", AttackPerSecondsModified);

        List<int> li = new List<int>();
        for (int i = 0; i < nearRessources.Count;i++)
        {
            if (nearRessources[i] == null)
            {
                li.Add(i);
            }
        }
        foreach (int j in li)
        {
            nearRessources.RemoveAt(j);
        }
        if (nearRessources.Count > 0)
        {
            m_animator.SetBool("Chopping", true);
            nearRessources.Sort(SortByDistance);
            Ressource nearestRessource = nearRessources[0];
            attackTimer = 0;
            Vector3 forward = (nearestRessource.transform.position - transform.position).normalized;
            forward.y = 0;
            transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        }
        else
        {
            m_animator.SetBool("Chopping", false);
        }
    }
        

    public void Chop(string s)
    {
        nearRessources.Sort(SortByDistance);
        Ressource nearestRessource = nearRessources[0];
        nearestRessource.RetrieveRessources(this);
    }

}
