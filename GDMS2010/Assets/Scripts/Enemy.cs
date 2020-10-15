using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(SphereCollider))]
public class Enemy : MonoBehaviour
{
    public enum State { Idle, Patrol, Alerted, Search, Chase, Attack, Dead}
    public float visualScanDistance;
    public float hearingDistance;
    public Animator animator;
    public int health;
    public State currState;

    private List<Transform> PatrolPoints;

    public GameObject player;
    public int patrolIndex;
    public bool foundPlayer;
    public float patrolSpeed = 1.5f;
    public float foundPlayerSpeed = 5.0f;
    NavMeshAgent agent;
    public float idleTime = 5f;
    public float alertTime = 5f;
    public float attackRange = 1f;
    float timer;

    Vector3 voicePos;
    bool isHearSth = false;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Initialize();
        transform.tag = "Enemy";
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (!player)
        {
            Debug.LogError("Enemy Can't find Player!");
        }
        //(instantiating the prefab) find patrol points
        PatrolPoints = new List<Transform>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PatrolPoint"))
        {
            PatrolPoints.Add(obj.transform);
        }
        timer = idleTime;
        GetComponent<SphereCollider>().radius = hearingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            animator.SetTrigger("Dance");
        foundPlayer = isPlayerNearby();
        switch (currState)
        {
            case State.Idle:
                {
                    animator.SetBool("IsIdle", true);
                    timer -= Time.deltaTime;
                    if(timer<0)
                    {
                        animator.SetBool("IsIdle", false);
                        currState = State.Patrol;
                        timer = idleTime;
                    }
                    if (foundPlayer)
                    {
                        animator.SetBool("IsIdle", false);
                        currState = State.Chase;
                        timer = idleTime;
                    }
                    if (isHearSth)
                    {
                        animator.SetBool("IsIdle", false);
                        animator.SetBool("IsWalking", true);
                        currState = State.Search;
                        timer = 2;
                    }
                }
                break;
            case State.Patrol:
                {
                    animator.SetBool("IsWalking", true);
                    Patrol();
                    if (foundPlayer)
                    {
                        animator.SetBool("IsWalking", false);
                        currState = State.Chase;
                    }
                    if (isHearSth)
                    {
                        currState = State.Search;
                        timer = 2;
                    }
                }
                break;
            case State.Alerted:
                {
                    if(agent.remainingDistance < 1f)
                    {
                        agent.SetDestination(transform.position);
                        animator.SetBool("Alert", true);
                        timer -= Time.deltaTime;
                        agent.speed = patrolSpeed;
                    }    
                    if(timer < 0)
                    {
                        animator.SetBool("Alert", false);
                        currState = State.Idle;
                        timer = idleTime;
                    }
                    if (foundPlayer)
                    {
                        animator.SetBool("Alert", false);
                        currState = State.Chase;
                    }
                }
                break;
            case State.Search:
                {
                    if(isHearSth)
                    {
                        agent.SetDestination(voicePos);
                        animator.SetBool("IsIdle", false);
                        animator.SetBool("IsWalking", true);
                        timer = 2f;
                        isHearSth = false;
                    }     
                    if(!agent.pathPending && agent.remainingDistance < 0.01f)
                    {
                        animator.SetBool("IsWalking", false);
                        animator.SetBool("IsIdle", true);
                        timer -= Time.deltaTime;
                        if(timer < 0)
                        {
                            timer = idleTime;
                            currState = State.Idle;
                            voicePos = Vector3.zero;
                            isHearSth = false;
                        }
                    }
                    if(foundPlayer)
                    {
                        animator.SetBool("IsWalking", false);
                        animator.SetBool("IsIdle", false);
                        voicePos = Vector3.zero;
                        isHearSth = false;
                        currState = State.Chase;
                    }
                }
                break;
            case State.Chase:
                {
                    if (isPlayerNext())
                    {
                        animator.SetBool("IsRunning", false);
                        agent.SetDestination(transform.position);
                        Attack();
                    }
                    else
                    {
                        animator.SetBool("IsRunning", agent.SetDestination(player.transform.position));
                        agent.speed = foundPlayerSpeed;
                        if (!foundPlayer)
                        {
                            animator.SetBool("IsRunning", false);
                            currState = State.Alerted;
                            timer = alertTime;
                        }
                    }                
                }
                break;
            case State.Attack:
                {
                    animator.SetTrigger("Attack");
                    if(isPlayerNext())
                    {
                        Attack();
                    }
                }
                break;
            case State.Dead:
                break;
            default:
                currState = State.Idle;
                break;
        }
    }

    public void walk()
    {
        animator.SetBool("IsWalking", true);
    }

    public void hit()
    {
        health--;
        if (health == 0)
            Death();
        else
            animator.SetTrigger("Hit");
    }

    public void Death()
    {
        animator.SetBool("Death", true);
    }

    public void _reset()
    {
        health = 5;
        animator.SetBool("Death", false);
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    void Patrol()
    {
        if(PatrolPoints.Count > 1)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                Vector3 temp = PatrolPoints[(patrolIndex++) % PatrolPoints.Count].position;
                agent.SetDestination(temp);
                walk();
            }
        }
    }

    bool isPlayerNearby()
    {
        if(!player)
            player = GameObject.FindGameObjectWithTag("Player");
        if (!player)
        {
            Debug.LogError("Enemy Can't find Player!");
        }
        Vector3 toPlayer = player.transform.position - this.transform.position;
        float distance = toPlayer.magnitude;
        if (distance > visualScanDistance)
            return false;
        return true;
    }

    bool isPlayerNext()
    {
        Vector3 toPlayer = player.transform.position - this.transform.position;
        float distance = toPlayer.magnitude;
        if (distance < attackRange)
            return true;
        return false;
    }

    void HearSomething(Vector3 pos)
    {
        if(voicePos != pos)
        {
            isHearSth = true;
            voicePos = pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.tag == "Sound")
            HearSomething(other.transform.position);
    }
}
