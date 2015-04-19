using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class MenuHandler : MonoBehaviour {

    public bool paused;
    public bool mainMenu = false;

    public GameObject[] canvas;
    public int currentCanvas = 0;
    public int currentEditing = 0;
    public InputField inputField;
    public GameObject popup;
    public GameObject playerObject;
    private PlayerScript playerScript;

    private string keyQCode = "";
    private string keyWCode = "";
    private string keyECode = "";
    private string keyRCode = "";

    public Attack qAttack = new Attack();
    public Attack wAttack = new Attack();
    public Attack eAttack = new Attack();
    public Attack rAttack = new Attack();

    private float qDelay = 0.1f;
    private float wDelay = 0.1f;
    private float eDelay = 0.1f;
    private float rDelay = 0.1f;
    private bool qLooping = false;
    private bool wLooping = false;
    private bool eLooping = false;
    private bool rLooping = false;

    void Start() {
        if (!mainMenu)
            playerScript = playerObject.GetComponent<PlayerScript>(); 
    }

    void Update() {
        if (paused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void OpenCanvas(int id) {
        paused = true;
        canvas[currentCanvas].gameObject.SetActive(false);
        canvas[id].gameObject.SetActive(true);
        currentCanvas = id;
    }
    
    public void CloseCanvas() {
        canvas[currentCanvas].gameObject.SetActive(false);
        paused = false;
    }

    public void StartEdit(int key) {
        switch(key) {
            case 0:
                currentEditing = key;
                inputField.text = keyQCode;
                Debug.Log("Editing Q");
                break;
            case 1:
                currentEditing = key;
                inputField.text = keyWCode;
                Debug.Log("Editing W");
                break;
            case 2:
                currentEditing = key;
                inputField.text = keyECode;
                Debug.Log("Editing E");
                break;
            case 3:
                currentEditing = key;
                inputField.text = keyRCode;
                Debug.Log("Editing R");
                break;
        }
    }

    public void FinishEdit() {
        switch (currentEditing) {
            case 0:
                playerScript.CancelAllLoops();
                qAttack = new Attack();
                ParseCode(inputField.text, 0);
                keyQCode = inputField.text;
                break;
            case 1:
                playerScript.CancelAllLoops();
                wAttack = new Attack();
                keyWCode = inputField.text;
                ParseCode(inputField.text, 1);
                break;
            case 2:
                playerScript.CancelAllLoops();
                eAttack = new Attack();
                keyECode = inputField.text;
                ParseCode(inputField.text, 2);
                break;
            case 3:
                playerScript.CancelAllLoops();
                rAttack = new Attack();
                keyRCode = inputField.text;
                ParseCode(inputField.text, 3);
                break;
        }
    }

    private void ParseCode(string code, int key) {

        code = Regex.Replace(code, @"\s+", "");
        List<string> cl = new List<string>(code.Split(';'));
        cl.RemoveAt(cl.Count - 1);

        if (!code.Contains(";")) {
            AddActionToKey(new Action(0, "none", 0, false), key);
        }

        for(int i = 0; i < cl.Count; i++) {
            if (cl[i].ToString().StartsWith("fireBall")) {
                Debug.Log("Detected fire.");
                AddActionToKey(new Action(1, GetTarget(cl[i].ToString(), key), GetDelay(key), GetLooping(key)), key);
            } else if (cl[i].ToString().StartsWith("water")) {
                Debug.Log("Detected water.");
                AddActionToKey(new Action(2, GetTarget(cl[i].ToString(), key), GetDelay(key), GetLooping(key)), key);
            } else if (cl[i].ToString().StartsWith("setDelay")) {
                Debug.Log("Detected setDelay.");
                SetDelay(cl[i].ToString(), key);
            } else if (cl[i].ToString().StartsWith("setLooping()")) {
                Debug.Log("Detected setLooping.");
                SetLooping(key);
            } else {
                Debug.Log("Syntax Error (Unknown Function) (Adding error action)");
                AddActionToKey(new Action(0, "none", 0, false), key);
            }
        }
    }

    private string GetTarget(string code, int key) {
        string[] f = code.Split('(');
        if (f[1].Equals("Direction.up)")) {
            Debug.Log("Direction is up.");
            return "up";
        } else if (f[1].Equals("Direction.down)")) {
            Debug.Log("Direction is down.");
            return "down";
        } else if (f[1].Equals("Direction.left)")) {
            Debug.Log("Direction is left.");
            return "left";
        } else if (f[1].Equals("Direction.right)")) {
            Debug.Log("Direction is right.");
            return "right";
        } else {
            Debug.Log("Syntax Error. (After '(')");
            AddActionToKey(new Action(0, "none", 0, false), key);
            return "error";
        }
    }

    private void SetDelay(string code, int key) {
        string[] tmp = code.Split('(');
        tmp = tmp[1].Split(')');
        Debug.Log(tmp[0]);
        int d = int.Parse(tmp[0]);
        switch (key) {
            case 0:
                qDelay = d;
                break;
            case 1:
                wDelay = d;
                break;
            case 2:
                eDelay = d;
                break;
            case 3:
                rDelay = d;
                break;
        }
    }

    private void SetLooping(int key) {
        switch (key) {
            case 0:
                qLooping = true;
                break;
            case 1:
                wLooping = true;
                break;
            case 2:
                eLooping = true;
                break;
            case 3:
                rLooping = true;
                break;
        }
    }

    private void AddActionToKey(Action action, int key) {
        switch (key) {
            case 0:
                qAttack.AddAction(action);
                break;
            case 1:
                wAttack.AddAction(action);
                break;
            case 2:
                eAttack.AddAction(action);
                break;
            case 3:
                rAttack.AddAction(action);
                break;
        }
    }

    private float GetDelay(int key) {
        float delay = 0;
        switch(key) {
            case 0:
                delay = qDelay;
                break;
            case 1:
                delay = wDelay;
                break;
            case 2:
                delay = eDelay;
                break;
            case 3: delay = rDelay;
                break;
        }
        return delay;
    }

    private bool GetLooping(int key) {
        bool looping = false;
        switch (key) {
            case 0:
                looping = qLooping;
                break;
            case 1:
                looping = wLooping;
                break;
            case 2:
                looping = eLooping;
                break;
            case 3:
                looping = rLooping;
                break;
        }
        return looping;
    }

    public void Popup(string text) {
        popup.SetActive(true);
        popup.GetComponent<PopupScript>().activeTime = 3.5f;
        popup.GetComponentInChildren<Text>().text = text;
    }

    public void Popup(string text, float time) {
        popup.SetActive(true);
        popup.GetComponent<PopupScript>().activeTime = time;
        popup.GetComponentInChildren<Text>().text = text;
    }

    public void loadScene(int scene) {
        Application.LoadLevel(scene);
    }

    public void Quit() {
        Application.Quit();
    }
}
