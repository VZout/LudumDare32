using System.Collections;

public class Action {

    public int type;
    public float delay;
    public string dir;
    public bool looping;

    public Action(int type, string dir, float delay, bool looping) {
        this.type = type;
        this.delay = delay;
        this.dir = dir;
        this.looping = looping;
    }
}
