using System;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class AttackState : MonoBaseState
{
    [SerializeField] private Transform bulletInstantiationPosition;
    public EnemyLineOfSight myLineOfSight;
    public GameObject playerHealth;
    public GameObject myRobot;
    public PlayerHealth myPlayerHealth;
    public EnemyMovement myMovement;
    public ShootPlayer myShootPlayer;
    public float shootCount;

    public GameObject bullet;
    public GameObject player;
    public float distance;
    public EnemyWorldState myWorldState;

    public override event Action OnNeedsReplan;

    public bool onState;

    private void Awake()
    {
        myShootPlayer = GetComponent<ShootPlayer>();
        player = GameObject.FindGameObjectWithTag("Player");
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        playerHealth = GameObject.FindGameObjectWithTag("PlayerHealth");
        myPlayerHealth = playerHealth.GetComponent<PlayerHealth>();
        myMovement = GetComponent<EnemyMovement>();
        myWorldState = GetComponent<EnemyWorldState>();
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        shootCount = 0;
        onState = true;
    }

    private void Update()
    {
        if (myWorldState.PlayerLife <= 0)
        {
            OnNeedsReplan?.Invoke();
        }
        if (onState)
        {
            if (myWorldState.seenPlayer)
            {
                if (myMovement.target != player.transform)
                    myMovement.target = player.transform;
            }

            distance = (player.transform.position - transform.position).sqrMagnitude;
        }
    }

    public override void UpdateLoop()
    {
    }

    public override IState ProcessInput()
    {
        
        if (myWorldState.seenPlayer)
        {
            if (distance >= 20f) //si me alejo del player, replaneo
            {
                onState = false;
                OnNeedsReplan?.Invoke();
            }

            if (distance < 10f)
            {
                Instantiate(bullet, transform.position, transform.rotation);
                onState = false;
                return Transitions["ReloadState"];
            }
        }

        if (!myWorldState.seenPlayer)
        {
            onState = false;
            return Transitions["ReloadState"];
        }
        return this;
    }
}