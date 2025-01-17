using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

public class CharacterEvents
{
    public static UnityAction<GameObject, int> characterDamaged;
    public static UnityAction<GameObject, int> characterHealed;

}
