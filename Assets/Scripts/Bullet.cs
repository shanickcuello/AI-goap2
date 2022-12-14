using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed;
    public GameObject player;
    public GameObject myPlayerHealth;
    public float time;

    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        myPlayerHealth = GameObject.FindGameObjectWithTag("PlayerHealth");
    }
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z).normalized;

        if (Vector3.Angle(transform.forward, direction) < 10)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        time += Time.deltaTime;

        if (Vector3.Distance(transform.position, player.transform.position) <= 0.5f)
        {
            myPlayerHealth.GetComponent<PlayerHealth>().health -= 10;
            Destroy(gameObject);
        }

        if (time >= 2)
        {
            Destroy(gameObject);
        }
    }
}
