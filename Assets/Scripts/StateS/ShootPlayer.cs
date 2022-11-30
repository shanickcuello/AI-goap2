using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlayer : MonoBehaviour
{

    public EnemyLineOfSight myLineOfSight;
    public GameObject playerHealth;
    public PlayerHealth myPlayerHealth;
    public float shootCount = 4;
    public EnemyBehaviours myBehaviours;
    public GameObject player;
    public Transform spawnPos;
    public GameObject bullet;

    private void Awake()
    {
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        playerHealth = GameObject.FindGameObjectWithTag("PlayerHealth");
        myPlayerHealth = playerHealth.GetComponent<PlayerHealth>();
        myBehaviours = GetComponent<EnemyBehaviours>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
                if (shootCount >= 5)
                {                   
                    var newBullet = GameObject.Instantiate(bullet);
                    newBullet.transform.position = spawnPos.transform.position;
                    shootCount = 0;
                

                }
    }
}
