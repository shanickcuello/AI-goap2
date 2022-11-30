using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : BaseNode
    
{

    public delegate void myDelegate();
    myDelegate _myAction;
    public ActionNode(myDelegate action)
    {
        _myAction = action;
    }

   public void Execute()
    {
        _myAction();
    }
}
