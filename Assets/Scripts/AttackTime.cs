using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTime : MonoBehaviour
{
    // Start is called before the first frame update
    public bool canAttack;

    void CanAttackEnemy()
    {
        canAttack = true;
    }

    void DontAttackEnemy()
    {
        canAttack = false;
    }
}
