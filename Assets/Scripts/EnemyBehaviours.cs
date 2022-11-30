using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviours : MonoBehaviour
{

    public bool chasePlayerBehaviour;
    public bool alertBehaviour;
    public bool returnBehaviour;
    public bool patrolBehaviour;
    public bool endedPatrol;
    public bool shootingPlayerBehaviour;
    public bool seekPlayerBehaviour;
    public bool idleBehaviour;
    public bool seenPlayer;
    public bool endedSeeking;
    public float alerCounter;
    public float chaseCounter;
    public float shootCounter;
    public float idleCounter;
    public Animator myAnim;
    public GameObject player;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;
    public GameObject thisRobot;
    public GameObject playerSeeker;
    public PlayerSeeker mySeeker;
    public float animWeight;
    public GameObject[] myEnemyLineList; 

    FSM<string> _fsm;



    // Start is called before the first frame update

    private void Awake()
    {
        playerSeeker = GameObject.FindGameObjectWithTag("PlayerSeeker");
        mySeeker = playerSeeker.GetComponent<PlayerSeeker>();
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        myEnemyLineList = GameObject.FindGameObjectsWithTag("Enemy");
        myMovement = GetComponent<EnemyMovement>();
        myAnim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        animWeight = 0;

        StateIdle<string> idle = new StateIdle<string>(gameObject);
        StatePatrol<string> patrol = new StatePatrol<string>(gameObject);
        StateShoot<string> shoot = new StateShoot<string>(gameObject);
        StateChase<string> chase = new StateChase<string>(gameObject);
        StateAlert<string> alert = new StateAlert<string>(gameObject);
        StateSeek<string> seek = new StateSeek<string>(gameObject);

        idle.AddTransition("IdleToPatrol", patrol);
        idle.AddTransition("IdleToChase", chase);
        idle.AddTransition("IdleToSeek", seek);
        patrol.AddTransition("PatrolToIdle", idle);
        patrol.AddTransition("PatrolToChase", chase);
        patrol.AddTransition("PatrolToSeek", seek);
        seek.AddTransition("SeekToIdle", idle);
        seek.AddTransition("SeekToChase", chase);
        seek.AddTransition("SeekToPatrol", patrol);
        shoot.AddTransition("ShootToChase", chase);
        chase.AddTransition("ChaseToShoot", shoot);
        chase.AddTransition("ChaseToAlert", alert);
        alert.AddTransition("AlertToChase", chase);
        alert.AddTransition("AlertToPatrol", patrol);
        alert.AddTransition("AlertToSeek", seek);

        
        _fsm = new FSM<string>(patrol);
    }

    // Update is called once per frame
    void Update()
    {
        Transitions();
        _fsm.OnUpdate();   
    }

    void Transitions()
    {
     if (idleBehaviour == true && endedPatrol == false && seekPlayerBehaviour == false)
        {
            
                _fsm.Transition("IdleToPatrol");
                idleBehaviour = false;
                patrolBehaviour = true;
         
                   
        }

        if (idleBehaviour == true && myLineOfSight.playerOnSight == true)
        {
            _fsm.Transition("IdleToChase");
            idleBehaviour = false;
            chasePlayerBehaviour = true;
        }

        if (patrolBehaviour == true && endedPatrol == true)
        {
            _fsm.Transition("PatrolToIdle");
            patrolBehaviour = false;
            idleBehaviour = true;
            
        }

        //transicion de perseguir player
        if (myLineOfSight.playerOnSight == true && seenPlayer == false)
        {
            _fsm.Transition("PatrolToChase");
            patrolBehaviour = false;
            chasePlayerBehaviour = true;
        

        //    chasePlayerBehaviour = true;
         //   patrolBehaviour = false;
        }


        //falta shoottochase

       if (Vector3.Distance(transform.position, player.transform.position) > myLineOfSight.seePlayerDist / 2 && shootingPlayerBehaviour == true)
        {
            _fsm.Transition("ShootToChase");
            shootingPlayerBehaviour = false;
            chasePlayerBehaviour = true;

        }

    


        if (myLineOfSight.playerOnSight == true && seenPlayer == true)
        {
            if(Vector3.Distance(transform.position, player.transform.position) < myLineOfSight.seePlayerDist / 2)
            {
                _fsm.Transition("ChaseToShoot");
                shootingPlayerBehaviour = true;
                chasePlayerBehaviour = false;

            }
        }

        if (myLineOfSight.playerOnSight == false && seenPlayer == true && chaseCounter >= 6)
        {
            _fsm.Transition("ChaseToAlert");
            chasePlayerBehaviour = false;
            alertBehaviour = true;

        }

        if (alertBehaviour == true && myLineOfSight.playerOnSight == true)
        {
            _fsm.Transition("AlertToChase");
            alertBehaviour = false;
            chasePlayerBehaviour = true;
          
        }

        if (endedSeeking == true)
        {
            _fsm.Transition("SeekToPatrol");
            seekPlayerBehaviour = false;
            myMovement.currentWaypoint = 0;
            patrolBehaviour = true;
            endedSeeking = false;


        }

        if (seekPlayerBehaviour == true && myLineOfSight.playerOnSight == true)
        {
            _fsm.Transition("SeekToChase");
            seekPlayerBehaviour = false;
            chasePlayerBehaviour = true;
           
        }     

        if (patrolBehaviour == true && mySeeker.seenPlayer == true)
        {
            _fsm.Transition("PatrolToSeek");
            patrolBehaviour = false;
            seekPlayerBehaviour = true;
          
        }

        if (idleBehaviour == true && mySeeker.seenPlayer == true)
        {
            _fsm.Transition("IdleToSeek");
            endedPatrol = false;
            idleBehaviour = false;
            seekPlayerBehaviour = true;
           
        }

        if (alerCounter >= 5)
        {
            _fsm.Transition("AlertToPatrol");
            alertBehaviour = false;
            patrolBehaviour = true;
          
        }

        if (mySeeker.seenPlayer == true && alertBehaviour == true)
        {
            _fsm.Transition("AlertToSeek");
            alertBehaviour = false;
            seekPlayerBehaviour = true;

        }
    }
}
