using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    IDLE,
    ROAM,
    CHASE,
    ATTACK
}

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private float health = 15;

    [SerializeField]
    private float damage = 10;

    private GameController game;
    private PlayerBehaviour player;


    [SerializeField, Header("PawnSensing")]
    private float detectionRadius = 7.0f;
    private EnemyState state = EnemyState.IDLE;
    [SerializeField]
    private float attackRadius = 2.0f;

    private Animator animator = null;


    // AI
    Transform target;
    NavMeshAgent agent;

    public readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    public readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");

    private Vector3 initialPosition;

    private void Start()
    {
        // Get components
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        game = FindObjectOfType<GameController>();
        player = FindObjectOfType<PlayerBehaviour>();
        animator = GetComponent<Animator>();
        target = player.transform;
        agent.enabled = true;

        // Save initial position for later
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Check if can see target, if so change state to chase
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= detectionRadius)
        {
            state = EnemyState.CHASE;

            // If target is within attack range, change to attack state
            if (distance <= attackRadius)
            {
                state = EnemyState.ATTACK;
                animator.SetBool(IsAttackingHash, true);
                //soundManager.PlayEnemyAttackSFX();
            }
            else
            {
                animator.SetBool(IsMovingHash, true);
                animator.SetBool(IsAttackingHash, false);
            }

        }
        else
        {
            state = EnemyState.IDLE;
            animator.SetBool(IsMovingHash, false);
        }

        switch (state)
        {
            case EnemyState.IDLE:
                break;

            case EnemyState.ROAM:
                break;

            case EnemyState.CHASE:
                // Chase player
                agent.SetDestination(new Vector3(target.position.x, transform.position.y, target.position.z));

                //RotateToTarget();
                break;

            case EnemyState.ATTACK:
                RotateToTarget();
                break;
        }
    }

    public void ShotAt(float damage)
    {
        health -= damage;
        Debug.Log(health);

        if (health <= 0)
        {
            transform.position = initialPosition;
            gameObject.SetActive(false);
        }
    }

    public float GetDamage()
    {
        if (game.phase2)
        {
            agent.speed *= 1.3f;
            return damage * 2f;
        }

        return damage;
    }

    private void RotateToTarget()
    {
        // Rotate to face the target
        Vector3 lookAtPosition = target.position;
        transform.LookAt(lookAtPosition);
    }
}
