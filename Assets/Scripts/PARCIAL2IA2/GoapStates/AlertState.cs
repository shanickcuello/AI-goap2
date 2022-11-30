using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class AlertState : MonoBaseState
{
    public override event Action OnNeedsReplan;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public override void UpdateLoop()
    {
        //
    }

    public override IState ProcessInput()
    {
        //llama distancia al player
        return this;
    }
}
