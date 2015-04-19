using System.Collections;
using System.Collections.Generic;

public class Attack {

    public List<Action> actions = new List<Action>();

    public void AddAction(Action action) {
        this.actions.Add(action);
    }
}
