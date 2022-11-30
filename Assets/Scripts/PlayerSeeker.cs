using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSeeker : MonoBehaviour
{
    public GameObject[] myEnemyLineList;
    public bool seenPlayer;
    public float counter = 0;


    // Start is called before the first frame update
    void Start()
    {
        myEnemyLineList = GameObject.FindGameObjectsWithTag("Enemy");

    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in myEnemyLineList)
        {
            if (item.GetComponent<EnemyLineOfSight>().playerOnSight == true)
            {
                seenPlayer = true;

            }

            if (seenPlayer == true)
            {
                counter += Time.deltaTime;
            }

            if (counter >= 2f && seenPlayer == true)
            {

                seenPlayer = false;
                counter = 0;

            }



        }

    }
}
