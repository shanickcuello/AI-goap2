using System.Collections.Generic;
using FSM;
using UnityEngine;

public class GOAPAction {

    public Dictionary<EEffects, bool> preconditions { get; private set; }
    public Dictionary<EEffects, bool> effects       { get; private set; }
    public EActions                   name          { get; private set; }
    public float                    cost          { get; private set; }
    public IState                   linkedState   { get; private set; }


    public GOAPAction(EActions actions) {
        this.name     = actions;
        cost          = 1f;
        preconditions = new Dictionary<EEffects, bool>();
        effects       = new Dictionary<EEffects, bool>();
    }

    public GOAPAction Cost(float cost) {
        if (cost < 1f) {
            //Costs < 1f make the heuristic non-admissible. h() could overestimate and create sub-optimal results.
            //https://en.wikipedia.org/wiki/A*_search_algorithm#Properties
            Debug.Log(string.Format("Warning: Using cost < 1f for '{0}' could yield sub-optimal results", name));
        }

        this.cost = cost;
        return this;
    }

    public GOAPAction Pre(EEffects preCondition, bool value) {
        preconditions[preCondition] = value;
        return this;
    }

    public GOAPAction Effect(EEffects action, bool value) {
        effects[action] = value;
        return this;
    }

    public GOAPAction LinkedState(IState state) {
        linkedState = state;
        return this;
    }
}

public enum EActions
{
    PATROL, 
    CHASE,
    ATTACK,
    IDLE, 
    ALERT,
    RELOADWEAPON
}

public enum EEffects
{
    PLAYERINSIGHT,
    PLAYERINRANGE,
    PLAYERALIVE,
    INALERT,
    HASBULLET,
    ALARMTRIGGERED,
    FINISHEDMISION
}