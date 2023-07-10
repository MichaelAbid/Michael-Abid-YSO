using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class AICharacterRecolte : AICharacter
{
    Vector3 targetPosition;
    public List<Ressource> list;
    protected Ressource FindNearestRessources()
    {
        list = FindObjectsOfType<Ressource>().ToList<Ressource>();
        list.Sort(SortByDistance);
        if (list.Count > 0)
        {
            foreach (Ressource item in list)
            {
                NavMeshPath navMeshPath = new NavMeshPath();
                if (agent.CalculatePath(item.transform.position, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    return item;
                }
            }
        }
        return null;
    }

    protected override void FixedUpdate()
    {
        if (!isActive) return;
        Ressource re = FindNearestRessources();
        if (re != null)
        {
            if (nearRessources.Count==0)
            {
                m_moveSpeed = Mathf.Lerp(m_moveSpeed, m_maxSpeed, 0.125f);
                targetPosition = re.transform.position;
                Move(targetPosition );
            }
            else
            {
                m_moveSpeed = Mathf.Lerp(m_moveSpeed, 0f, 0.3f);
                targetPosition = transform.position;
                StopMoving();
                RessourcesHandle();
               
            }
        }
        else
        {
            m_animator.SetBool("Chopping", false);
            m_moveSpeed = Mathf.Lerp(m_moveSpeed, 0f, 0.3f);
            targetPosition = transform.position;
            StopMoving();
        }
        
        base.FixedUpdate();

    }


}
