﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMState<T>
{
    Dictionary<T, FSMState<T>> _dic = new Dictionary<T, FSMState<T>>();

    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnSleep() { }

    public void AddTransition(T input, FSMState<T> state)
    {

        if (!_dic.ContainsKey(input))
            _dic.Add(input, state);

    }

    public FSMState<T> GetTransition(T input)
    {
        if (_dic.ContainsKey(input))
            return _dic[input];

        else return null;
    }
}
