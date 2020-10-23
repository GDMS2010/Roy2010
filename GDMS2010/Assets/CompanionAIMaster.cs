using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Script_Health))]
public class CompanionAIMaster : MonoBehaviour
{
    public string Name;
    public Enemy currTarget;
    public List<Enemy> targetList;
    public GameObject Player;
    public int affection;
    public bool IsAlive = true;
    public bool Interacted = false;

    public Transform GoalPosition; //Ultimate Goal Position;
    public GameObject Ammo;

    public State currState;
    public Mood currMood;

    public Animator anim;
    public NavMeshAgent agent;

    [Header("CompanionStat")]
    public float cautionWalkSpeed = 0.5f;
    public float walkSpeed = 1.0f;
    public float runSpeed = 2.0f;

    [Header("Scanning")]
    public float visualDistance = 10f;
    public float visualAngle = 120f;
    public float hearDistance = 20f;

    [Header("GunStat")]
    public float roundPerSec = 1.0f; //How many can it shoot per second
    public int currAmmo = 20;
    public int Magazine = 20;
    public int damage = 10;
    public float accuracy = 0.75f;
    public float attackDistance = 10f;
    float nextRound = 1f;
    bool IsReloading = false;

    public enum State
    {
        Idle,
        Walk,
        Run,
        Search,
        StandBy,
        Death,
        Reload,
        SpecialAction
    }

    public enum Mood
    {
        Protect,        // Protect player
        Aggresive,      // Aggresive to player
        Alone           // Ignore player
    }

    // Start is called before the first frame update
    void Start()
    {
        targetList = new List<Enemy>();
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        if (!anim) Debug.LogError(name + " has no animator!");
        if (!agent) Debug.LogError(name + " has no NavMeshAgent!");
        anim.SetFloat("ShootingSpeed", roundPerSec);
        GetComponent<Script_Health>().HitEvent.AddListener(hit);
        GetComponent<Script_Health>().DeathEvent.AddListener(Die);
    }

    // Update is called once per frame
    void Update()
    {
        #region debug
        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetBool("Walking", false);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetBool("Walking", true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetBool("Run", true);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            anim.SetBool("Run", false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
            anim.SetBool("IsCaution", true);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            anim.SetBool("IsCaution", false);

        if (Input.GetMouseButton(0))
        {
            //Fire();

            //Vector3 mouse = Input.mousePosition;
            //Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            //RaycastHit hit;
            //if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
            //{
            //    agent.SetDestination(hit.point);
            //}
        }
        if (Input.GetMouseButtonUp(0))
        {
            //anim.SetBool("Fire", false);
        }
        if (Input.GetKeyDown(KeyCode.P))
            Die();
        if (agent.remainingDistance > 0)
            anim.SetBool("Walking", true);
        else
            anim.SetBool("Walking", false);
        #endregion
        if (IsAlive)
        {
            switch (currState)
            {
                case State.Idle:
                    break;
                case State.Walk:
                    break;
                case State.Run:
                    break;
                case State.Search:
                    break;
                case State.StandBy:
                    break;
                case State.Death:
                    break;
                case State.Reload:
                    break;
                case State.SpecialAction:
                    break;
                default:
                    break;
            }

            nextRound -= roundPerSec * Time.deltaTime;
            searchTarget();
            UpdateEnemyList();
            if (currTarget)
            {
                if (!IsReloading)
                {
                    if (withinRange())
                    {
                        agent.SetDestination(transform.position);
                        anim.SetBool("Walking", false);
                        anim.SetBool("IsCaution", false);
                        Fire(currTarget);
                    }
                    else
                    {
                        anim.SetBool("Fire", false);
                        anim.SetBool("IsCaution", true);
                        walk();
                        agent.speed = cautionWalkSpeed;
                        agent.SetDestination(currTarget.gameObject.transform.position);
                    }
                }
            }
            else
            {                
                anim.SetBool("IsCaution", false);
                anim.SetBool("Fire", false);
                agent.speed = walkSpeed;
                walk();
                agent.SetDestination(GoalPosition.position);
                if (currAmmo < 10)
                {
                    Reload();
                }
                if (!Interacted)
                {
                    if (Player)
                    {
                        if (IsCloseToPlayer())
                        {
                            if (affection > 99)
                                Player.GetComponent<Script_Health>().Heal(20);
                            else if (affection > 50)
                                Player.GetComponent<Script_Health>().Heal(10);
                            else if(affection > 25)
                                Instantiate(Ammo, transform.position, transform.rotation);
                            Interacted = true;
                        }
                        else
                        {
                            agent.SetDestination(Player.transform.position);
                        }
                    }
                }
            }
        }
    }

    bool withinRange()
    {
        Vector3 dif = currTarget.gameObject.transform.position - transform.position;
        float dis = dif.magnitude;
        return (dis < attackDistance);
    }

    void Fire(Enemy target)
    {
        if (nextRound <= 0 && !IsReloading)
        {
            if (currAmmo > 0)
            {
                anim.SetBool("Fire", true);
                transform.LookAt(target.transform);
                currAmmo -= 1;
                nextRound = 1f;

                float val = Random.Range(0, 1f);
                if(val < accuracy)
                    target.hit(damage);
                if (!target.isAlive)
                {
                    targetList.Remove(target);
                    currTarget = getCloestTarget();
                }

            }
            if (currAmmo == 0)
                Reload();
        }
    }

    Enemy getNewTargetFromList()
    {
        if (targetList.Count == 0)
            return null;
        else return targetList[Random.Range(0, targetList.Count - 1)];
    }

    Enemy getCloestTarget()
    {
        if (targetList.Count == 0)
            return null;
        Enemy target = null;
        float distance = float.MaxValue;
        foreach (var e in targetList)
        {
            Vector3 dif = e.gameObject.transform.position - transform.position;
            float dis = dif.magnitude;
            if (dis < distance)
            {
                target = e;
                distance = dis;
            }
        }
        return target;
    }

    void UpdateEnemyList()
    {
        if (targetList.Count == 0) return;
        foreach (var t in targetList)
        {
            if (!t.isAlive)
                targetList.Remove(t);
        }
    }

    void Reload()
    {
        //anim.SetBool("Fire", false);
        if (currTarget)
        {
            anim.SetTrigger("WalkReload");
            agent.speed = walkSpeed;
            Vector3 dif = currTarget.gameObject.transform.position - transform.position;
            agent.SetDestination(-dif + transform.position);
        }
        else
            anim.SetTrigger("Reload");
        IsReloading = true;
    }

    void Die()
    {
        if (IsAlive)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Fire", false);
            anim.SetTrigger("Death");
        }
        IsAlive = false;
    }

    void walk()
    {
        anim.SetBool("Walking", true);
    }

    void run()
    {
        anim.SetBool("Run", true);
    }

    public void hit(GameObject attacker)
    {
        Enemy enemy = attacker.GetComponent<Enemy>();
        if (enemy)
        {
            if (!targetList.Contains(enemy))
                targetList.Add(enemy);
        }
    }

    /// AI
    bool IsCloseToPlayer()
    {
        if(!Player)
            return false;
        Vector3 dis = Player.transform.position - transform.position;
        return dis.magnitude < 3f ? true : false;
    }

    Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);
    void searchTarget()
    {
        RaycastHit hit;
        Quaternion startingAngle = Quaternion.AngleAxis(-visualAngle / 2f, Vector3.up);
        var angle = transform.rotation * startingAngle;
        var direction = angle * Vector3.forward;
        var position = transform.position;
        for (int i = 0; i < 24; i++)
        {
            if (Physics.Raycast(position, direction, out hit, visualDistance))
            {
                Enemy go = hit.collider.gameObject.GetComponent<Enemy>();
                if (go)
                {
                    Debug.DrawRay(position, direction * hit.distance, Color.red);
                    if (go.isAlive)
                    {
                        if (!targetList.Contains(go))
                            targetList.Add(go);
                    }
                }
                else
                    Debug.DrawRay(position, direction * hit.distance, Color.green);
                if (hit.collider.gameObject.tag == "Player")
                    Player = hit.collider.gameObject;
            }
            else
                Debug.DrawRay(position, direction * visualDistance, Color.white);
            direction = stepAngle * direction;
        }
        currTarget = getCloestTarget();
    }


    /// Animation Event
    public void DoneReloading()
    {
        currAmmo = Magazine;
        IsReloading = false;
    }
}
