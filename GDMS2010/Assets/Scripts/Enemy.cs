using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(SphereCollider))]
public class Enemy : MonoBehaviour
{
    public enum State { Idle, Patrol, Alerted, Search, Chase, Attack, Dead }
    public float visualScanDistance;
    public float hearingDistance;
    public Animator animator;
    public int health;
    public State currState;

    private List<Transform> PatrolPoints;

    public List<GameObject> players;
    public GameObject target;
    public int patrolIndex;
    public bool foundPlayer;
    public float patrolSpeed = 1.5f;
    public float foundPlayerSpeed = 5.0f;
    NavMeshAgent agent;
    public float idleTime = 5f;
    public float alertTime = 5f;
    public float attackRange = 1f;
    public int Damage = 5;
    float timer;

    Vector3 voicePos;
    bool isHearSth = false;
    public bool isAlive = true;
    public float nextAttack;
    public float attackSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        players = new List<GameObject>();
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if(go)
            players.Add(go);
        foreach (var item in GameObject.FindGameObjectsWithTag("Companion"))
        {
            players.Add(item);
        }
        transform.tag = "Enemy";
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        animator = GetComponentInChildren<Animator>();
        if (players.Count == 0)
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
        //GetComponent<SphereCollider>().radius = hearingDistance;
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
                    if (timer < 0)
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
                    if (agent.remainingDistance < 1f)
                    {
                        agent.SetDestination(transform.position);
                        animator.SetBool("Alert", true);
                        timer -= Time.deltaTime;
                        agent.speed = patrolSpeed;
                    }
                    if (timer < 0)
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
                    if (isHearSth)
                    {
                        agent.SetDestination(voicePos);
                        animator.SetBool("IsIdle", false);
                        animator.SetBool("IsWalking", true);
                        timer = 2f;
                        isHearSth = false;
                    }
                    if (!agent.pathPending && agent.remainingDistance < 0.01f)
                    {
                        animator.SetBool("IsWalking", false);
                        animator.SetBool("IsIdle", true);
                        timer -= Time.deltaTime;
                        if (timer < 0)
                        {
                            timer = idleTime;
                            currState = State.Idle;
                            voicePos = Vector3.zero;
                            isHearSth = false;
                        }
                    }
                    if (foundPlayer)
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
                        if (!target)
                        {
                            currState = State.Alerted;
                            break;
                        }
                        animator.SetBool("IsRunning", agent.SetDestination(target.transform.position));
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
                    //animator.SetTrigger("Attack");
                    if (isPlayerNext())
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
        nextAttack -= attackSpeed * Time.deltaTime;
    }

    public void walk()
    {
        animator.SetBool("IsWalking", true);
    }

    public void hit(int value)
    {
        health -= value;
        if (health <= 0)
        {
            Death();
        }
        else
        {
            //animator.SetTrigger("Hit");
        }
    }

    public void Death()
    {
        agent.enabled = false;
        currState = State.Dead;
        animator.SetBool("Death", true);
        isAlive = false;
        Destroy(this.gameObject, 3f);
    }

    public void _reset()
    {
        health = 5;
        animator.SetBool("Death", false);
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        //if (nextAttack <= 0)
        //{
        //    Script_Health dmg = target.GetComponent<Script_Health>();
        //    if (dmg.IsAlive)
        //    {
        //        target.GetComponent<Script_Health>().Hit(this.gameObject, Damage);
        //        nextAttack = 1;
        //        animator.SetTrigger("Attack");
        //    }
        //    else
        //    {
        //        players.Remove(target);
        //        target = null;
        //        currState = State.Alerted;
        //    }
        //}
    }

    void Patrol()
    {
        if (PatrolPoints.Count > 1)
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
        foreach (var p in players)
        {
            Vector3 toPlayer = p.transform.position - this.transform.position;
            float distance = toPlayer.magnitude;
            if (distance < visualScanDistance)
            {
                target = p;
                return true;
            }
        }
        return false;
    }

    bool isPlayerNext()
    {
        Vector3 toPlayer = target.transform.position - this.transform.position;
        float distance = toPlayer.magnitude;
        if (distance < attackRange)
            return true;
        return false;
    }

    void HearSomething(Vector3 pos)
    {
        if (voicePos != pos)
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
