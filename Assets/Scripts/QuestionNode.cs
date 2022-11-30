using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNode:BaseNode
{
    public delegate bool myDelegate();
    BaseNode _isTrue;
    BaseNode _isFalse;
    myDelegate _myQuestion;
    public QuestionNode(myDelegate question, BaseNode isTrue, BaseNode isFalse)
    {
        _myQuestion = question;
        _isTrue = isTrue;
        _isFalse = isFalse;

    }

    public void Execute()
    {
        if (_myQuestion())
        {
            _isTrue.Execute();
        }
        else
        {
            _isFalse.Execute();
        }
    }
    
}
