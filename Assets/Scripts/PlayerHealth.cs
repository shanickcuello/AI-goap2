using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public float health = 100;
    public float maxHealth = 100;
    public float time;
    public float endScreenAlpha;
    public GameObject endScreen;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            time += Time.deltaTime;
            endScreenAlpha += Time.deltaTime;
            endScreen.GetComponent<Image>().color = new Color(255, 255, 255, endScreenAlpha);
            endScreen.SetActive(true);

            if (time >= 5)
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }
}
