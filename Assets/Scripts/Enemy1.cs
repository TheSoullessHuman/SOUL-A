using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy1 : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;               
    public float startWaitTime = 4;                 
    public float timeToRotate = 2;                  
    public float speedWalk;                     
    public float speedRun;
    public float speedattack;

    public float viewRadius = 15;                   //  Radio de la visión del enemigo
    public float viewAngle = 90;                    //  Angulo de la visión del enemigo
    public LayerMask playerMask;                    //  detecta la layer del jugador con raycast
    public LayerMask obstacleMask;                  //  lo mismo pero con obstaculos 
    public float meshResolution = 1.0f;             //  La cantidad de rayos que lanza por grado
    public int edgeIterations = 4;                  //  Numero de iteraciónes 
    public float edgeDistance = 0.5f;               
    public Animator animator;

    public Transform[] waypoints;                   //  para los waypoints
    int m_CurrentWaypointIndex;                     //  donde el enemigo se dirige 
    public Collider hit;
    public bool attack;
    

    Vector3 playerLastPosition = Vector3.zero;      
    Vector3 m_PlayerPosition;                       

    float m_WaitTime;                              
    float m_TimeToRotate;                           
    bool m_playerInRange;                           
    bool m_PlayerNear;                              
    bool m_IsPatrol;                                
    bool m_CaughtPlayer;                            
    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_playerInRange = false;
        m_PlayerNear = false;
        m_WaitTime = startWaitTime;                
        m_TimeToRotate = timeToRotate;
        

        m_CurrentWaypointIndex = 0;                 
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;             
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);    
    }

    private void Update()
    {
        EnviromentView();                       

        if (!m_IsPatrol&&attack==false)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }

        if (navMeshAgent.speed == speedWalk)
        {
            animator.SetBool("walk", true);
            animator.SetBool("run", false);
        }
        else if (navMeshAgent.speed == speedRun)
        {
            animator.SetBool("run", true);
            animator.SetBool("walk", false);
        }
    }

    private void Chasing()
    {
        //  The enemy is chasing the player
        m_PlayerNear = false;                       
        playerLastPosition = Vector3.zero;          

        if (!m_CaughtPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(m_PlayerPosition);          
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)    
        {
            if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                    
                    Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    private void Patroling()
    {
        if (m_PlayerNear)
        {
            
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;           
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);   
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                
                if (m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    private void OnAnimatorMove()
    {

    }

    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void Stop()
    {
        animator.SetBool("run", false);
        animator.SetBool("walk", false);
        animator.SetBool("idle", true);
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
        animator.SetBool("idle", false);
        /*
        if (navMeshAgent.speed== speedWalk)
        {
            animator.SetBool("walk", true);
            animator.SetBool("run", false);
        }
        else if(navMeshAgent.speed == speedRun)
        {
            animator.SetBool("run", true);
            animator.SetBool("walk", false);
        }
        */
        
    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);   //  Make an overlap sphere around the enemy to detect the playermask in the view radius

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);          //  Distance of the enmy and the player
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_playerInRange = true;             //  The player has been seeing by the enemy and then the nemy starts to chasing the player
                    m_IsPatrol = false;                 //  Change the state to chasing the player
                }
                else
                {
                    /*
                     *  If the player is behind a obstacle the player position will not be registered
                     * */
                    m_playerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
               
                m_playerInRange = false;                //  Change the sate of chasing
            }
            if (m_playerInRange)
            {
               
                m_PlayerPosition = player.transform.position;      
            }
        }
    }
    private void attacking()
    {

    }
    private void  OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "Player")
        {
            attack = true;
            animator.SetBool("attack", true);
            navMeshAgent.isStopped = true;
            Move(speedattack);


        }
    }

    private void OnTriggerExit(Collider hit)
    {
        if (hit.tag == "Player")
        {
            attack = false;
            animator.SetBool("attack", false);
            navMeshAgent.isStopped = false;
            Move(speedRun);

        }
    }
    

}