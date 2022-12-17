using System;
using UnityEngine;

public class EnemyWorldState : MonoBehaviour
{
    public bool seenPlayer = false;
    public float timer;
    public float distanceToPlayer;
    public float DistanceToSeePlayer = 5;
    private DonePlayerMovement Player;
    private PlayerHealth playerHealth;
    public float PlayerLife;

    private void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        Player = FindObjectOfType<DonePlayerMovement>();
    }

    public float DistanceToPlayer
    {
        get => distanceToPlayer;
        private set => distanceToPlayer = value;
    }


    private void Update()
    {
        UpdatePlayerLife();
        if (GetDistanceToPlayer() < DistanceToSeePlayer)
        {
            timer += Time.deltaTime;

            if (timer >= 10f)
            {
                seenPlayer = false;
                timer = 0;
            }
        }
    }

    private void UpdatePlayerLife()
    {
        PlayerLife = playerHealth.health;
        Debug.Log($"PlayerLife: {PlayerLife}");
    }

    private float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, Player.transform.position);
    }

    public void ResetTimer()
    {
        timer = 0;
        seenPlayer = true;
    }
}
