using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent navAgent;

    [SerializeField] float alertRange;
    [SerializeField] float stopRange;
    [SerializeField] Transform playerTarget;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] bool inRange;
    [SerializeField] Animator animator;
    [SerializeField] int health;
    private bool canAttack;

    [SerializeField] bool questSkelly;
    // Start is called before the first frame update
    void Start()
    {
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.CheckSphere(transform.position, alertRange, playerLayer))
            inRange = true;
        else
            inRange = false;

        if (inRange)
        {
            navAgent.SetDestination(playerTarget.position);
        }
        if(navAgent.velocity.magnitude > 0.5f)
        {
            animator.SetBool("run", true);
        }
        if (navAgent.velocity.magnitude <= 0.5f)
        {
            animator.SetBool("run", false);
        }

        if(Physics.CheckSphere(transform.position, 2, playerLayer)) 
            canAttack = true; 
        else
            canAttack = false;

        AttackPlayer();
    }
    float attackCooldown = 5f;
    float currentCoolDownTime = 0;
    void AttackPlayer()
    {
        if (canAttack && currentCoolDownTime <= 0f)
        {
            animator.SetBool("attack", true);
            StartCoroutine(Attack());
            currentCoolDownTime = attackCooldown;
        }

        if (canAttack)
            currentCoolDownTime -= Time.deltaTime;

        if (!canAttack)
        {
            animator.SetBool("attack", false);
            currentCoolDownTime = 1f;
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f);
        if(canAttack)
            playerTarget.gameObject.GetComponent<PlayerMovement>().Damage(10);
    }

    public void Damage(int damage)
    {
        health -= damage;
        if(health <= 0f)
        {
            if (questSkelly)
            {
                playerTarget.GetComponent<PlayerMovement>().questSkelly++;
            }
            Destroy(gameObject);
        }
    }
}
