using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public float fillamount;
    public Image content;
    public float maxValue;
    public float currentValue;
    public float minValue;
    public Text valueText;
    public float lerpSpeed;
    public GameObject player;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

       

        HandleBar();

    }

    private void HandleBar()
    {
        if (fillamount >= 1)
        {
            fillamount = 1;
        }
        if (fillamount <= 0)
        {
            fillamount = 0;
        }

        fillamount = currentValue / maxValue;
        content.fillAmount = Mathf.Lerp(content.fillAmount, fillamount, Time.deltaTime * lerpSpeed);

        string[] tmp = valueText.text.Split(':');
        valueText.text = tmp[0] + ": " + Mathf.Round(currentValue);

        currentValue = Mathf.Clamp(currentValue, minValue, maxValue);

        currentValue = player.GetComponent<PlayerHealth>().health;
        maxValue = player.GetComponent<PlayerHealth>().maxHealth;
    }

   
}
