using UnityEngine;
using System.Collections;
using System;

public class RobotStateMachine : MonoBehaviour
{
    public float _idleTime;
    public bool autoIA = false;
    public bool doStuff = false;
    public bool testBool;

    public EnemyBehaviours myBehaviours;
    public EnemyMovement myMovement;
    public EnemyLineOfSight mySight;

    public GameObject player;

    Vector3 dir;

    public ActionNode _goIdle;
    public ActionNode _chasePlayer;
    public ActionNode _patrol;
    public ActionNode _beAlert;
    public ActionNode _shootPlayer;

    QuestionNode _playerOnSight;
    QuestionNode _endedPatrol;
    QuestionNode _lostPlayer;
    QuestionNode _alerLostPlayer;
    QuestionNode _imClose;
    QuestionNode _seenPlayer;
    QuestionNode _endedAlert;

    public int timeToNextAction;

    QuestionNode _rootNode;

    private void Awake()
    {
        _goIdle = new ActionNode(GoIdle);
        _chasePlayer = new ActionNode(ChasePlayer);
        _patrol = new ActionNode(Patrol);
        _beAlert = new ActionNode(BeAlert);
        _shootPlayer = new ActionNode(Shoot);
        //  _nextAction = new ActionNode(DoNextAction);

        // _alerLostPlayer = new QuestionNode(HaveFoodWood, _nextAction, _haveFood);
      
        _endedPatrol = new QuestionNode(EndedPatrol, _goIdle, _patrol);
        _endedAlert = new QuestionNode(EndedAlert, _patrol, _beAlert);
        _lostPlayer = new QuestionNode(LostPlayer, _endedAlert, _chasePlayer);
        _imClose = new QuestionNode(ImClose, _shootPlayer, _lostPlayer);
        _seenPlayer = new QuestionNode(SeenPlayer, _imClose, _endedPatrol);
        _playerOnSight = new QuestionNode(PlayerOnSight, _imClose, _seenPlayer);

        _rootNode = _playerOnSight;

        myBehaviours = GetComponent<EnemyBehaviours>();
        myMovement = GetComponent<EnemyMovement>();
        mySight = GetComponent<EnemyLineOfSight>();
        player = GameObject.FindGameObjectWithTag("Player");

    }


    public void Start()
    {
    }



    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) || autoIA == true)
        {
            if (transform.position == myMovement.startPos)
            {
                
            }
            _rootNode.Execute();

        }

    }

    bool PlayerOnSight()
    {
        
        return mySight.playerOnSight;

    }

    bool SeenPlayer()
    {
       
        return myBehaviours.seenPlayer;
    }

    bool EndedPatrol()
    {
        
        return myBehaviours.endedPatrol;
    }

    bool ImClose()
    {
      
        return Vector3.Distance(transform.position, player.transform.position) < mySight.seePlayerDist / 2 && mySight.playerOnSight;
    }

    void Shoot()
    {
      
        myBehaviours.shootingPlayerBehaviour = true;
        myBehaviours.chasePlayerBehaviour = true;
        myBehaviours.idleBehaviour = false;
        myBehaviours.patrolBehaviour = false;
        myBehaviours.alertBehaviour = false;
        
    }

    void GoIdle()
    {
       
        myBehaviours.idleBehaviour = true;
        myBehaviours.shootingPlayerBehaviour = false;
        myBehaviours.chasePlayerBehaviour = false;
        myBehaviours.patrolBehaviour = false;
        myBehaviours.alertBehaviour = false;
    }

    void ChasePlayer()
    {
      
        myBehaviours.chasePlayerBehaviour = true;
        myBehaviours.shootingPlayerBehaviour = false;
        myBehaviours.idleBehaviour = false;
        myBehaviours.patrolBehaviour = false;
        myBehaviours.alertBehaviour = false;
    }

    bool LostPlayer()
    {
   
        return myBehaviours.chaseCounter > 3;
    }

    bool EndedAlert()
    {
        return myBehaviours.alerCounter > 5;
    }

    void Patrol()
    {
     
        myBehaviours.patrolBehaviour = true;
        myBehaviours.shootingPlayerBehaviour = false;
        myBehaviours.chasePlayerBehaviour = false;
        myBehaviours.idleBehaviour = false;
        myBehaviours.alertBehaviour = false;
    }

    void BeAlert()
    {
     
        myBehaviours.alertBehaviour = true;
        myBehaviours.shootingPlayerBehaviour = false;
        myBehaviours.chasePlayerBehaviour = false;
        myBehaviours.idleBehaviour = false;
        myBehaviours.patrolBehaviour = false;
    }


}