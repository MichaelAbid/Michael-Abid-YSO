using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventReceiver : MonoBehaviour
{
    public Character character;



    public void Chop(string s)
    {
        character.Chop(s);
    }

}
