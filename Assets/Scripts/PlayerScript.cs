using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    public int kills = 0;
    public int killsNeeded = 20;
    public int health = 100;
    public int soulProcessor = 100;
    public int soulCapacity = 100;

    public float speed;
    private float startSpeed;
    public GameObject gameManager;
    public AudioSource buttonAudioSource;
    private MenuHandler gm;

    private bool hInput = false;
    private bool vInput = false;

    [Header("Attack Stuff")]
    public GameObject fireball;
    public float fireballlSpeed = 14;
    public int fireballCost = 10;
    public GameObject water;
    public float waterSpeed = 10;
    public int waterCost = 5;
    public GameObject errorObject;
    public int errorDamage = 23;

    [Header("UI Stuff")]
    public Slider cpSlider;
    public Slider hSlider;

    /*
    SyntaxError = 0;
    FireBall = 1
    Water = 2
    */

    void Start() {
        InvokeRepeating("RechargeSP", 0, 0.5f);
        gm = gameManager.GetComponent<MenuHandler>();
        startSpeed = speed;
    }

    void Update() {
        cpSlider.value = soulProcessor;
        hSlider.value = health;

        if (kills >= killsNeeded)
            Application.LoadLevel(3);

        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            GetComponent<Animator>().SetBool("Walking", true);
        else
            GetComponent<Animator>().SetBool("Walking", false);

        if (health <= 0)
            Application.LoadLevel(2);
    }

    void FixedUpdate() {
        if (!gm.paused) {

            if (soulProcessor <= 0)
                speed = startSpeed / 4;
            else
                speed = startSpeed;
            speed *= hInput && vInput ? 0.7071f : 1;

            if (Input.GetKey(KeyCode.UpArrow)) {
                GetComponent<Rigidbody2D>().velocity += new Vector2(0, speed);
                vInput = true;
            } else if (Input.GetKey(KeyCode.DownArrow)) {
                GetComponent<Rigidbody2D>().velocity += new Vector2(0, -speed);
                vInput = true;
            } else { vInput = false; }
            if (Input.GetKey(KeyCode.LeftArrow)) {
                GetComponent<Rigidbody2D>().velocity += new Vector2(-speed, 0);
                hInput = true;
            } else if (Input.GetKey(KeyCode.RightArrow)) {
                GetComponent<Rigidbody2D>().velocity += new Vector2(speed, 0);
                hInput = true;
            } else { hInput = false; }

            if (Input.GetKeyDown(KeyCode.Q))
                PerformQAttack();

            if (Input.GetKeyDown(KeyCode.W))
                PerformWAttack();

            if (Input.GetKeyDown(KeyCode.E))
                PerformEAttack();

            if (Input.GetKeyDown(KeyCode.R))
                PerformRAttack();
        }

        if (gm.currentCanvas != 2) {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.M)) {
                if (!gm.paused) {
                    buttonAudioSource.Play();
                    gm.OpenCanvas(0);
                } else {
                    buttonAudioSource.Play();
                    gm.CloseCanvas();
                }
            }
        }
    }

    public void ActionError() {
        gm.Popup("Syntax Error! \n Check your spelling, capitals and semicolons", 4.8f);
        Instantiate(errorObject, transform.position, transform.rotation);
        health -= errorDamage;
        soulProcessor = -5;
    }

    public void PerformQAttack() {
        if (gm.qAttack.actions.Count > 0) {
            for (int i = 0; i < gm.qAttack.actions.Count; i++) {
                //Fireball
                if (gm.qAttack.actions[i].type == 1) {
                    if (gm.qAttack.actions[i].looping) {
                        if (gm.qAttack.actions[i].dir.Equals("up")) {
                            InvokeRepeating("ActionFireBallUp", 0, gm.qAttack.actions[i].delay);
                        } else if (gm.qAttack.actions[i].dir.Equals("down")) {
                            InvokeRepeating("ActionFireBallDown", 0, gm.qAttack.actions[i].delay);
                        } else if (gm.qAttack.actions[i].dir.Equals("left")) {
                            InvokeRepeating("ActionFireBallLeft", 0, gm.qAttack.actions[i].delay);
                        } else if (gm.qAttack.actions[i].dir.Equals("right")) {
                            InvokeRepeating("ActionFireBallRight", 0, gm.qAttack.actions[i].delay);
                        }
                    } else {
                        if (gm.qAttack.actions[i].dir.Equals("up")) {
                            Debug.Log(gm.qAttack.actions[i].delay);
                            Invoke("ActionFireBallUp", gm.qAttack.actions[i].delay);
                        } else if (gm.qAttack.actions[i].dir.Equals("down")) {
                            Invoke("ActionFireBallDown", gm.qAttack.actions[i].delay);
                        } else if (gm.qAttack.actions[i].dir.Equals("left")) {
                            Invoke("ActionFireBallLeft", gm.qAttack.actions[i].delay);
                        } else if (gm.qAttack.actions[i].dir.Equals("right")) {
                            Invoke("ActionFireBallRight", gm.qAttack.actions[i].delay);
                        }
                    }


                    //Water
                } else if (gm.qAttack.actions[i].type == 2) {
                    if (gm.qAttack.actions[i].looping) {
                        Debug.Log("Starting Water");
                        if (gm.qAttack.actions[i].dir.Equals("up")) {
                            InvokeRepeating("ActionWaterUp", 0, gm.qAttack.actions[i].delay);
                        } else if (gm.qAttack.actions[i].dir.Equals("down")) {
                            InvokeRepeating("ActionWaterDown", 0, gm.qAttack.actions[i].delay);
                        } else if (gm.qAttack.actions[i].dir.Equals("left")) {
                            InvokeRepeating("ActionWaterLeft", 0, gm.qAttack.actions[i].delay);
                        } else if (gm.qAttack.actions[i].dir.Equals("right")) {
                            InvokeRepeating("ActionWaterRight", 0, gm.qAttack.actions[i].delay);
                        }
                    } else {
                        if (gm.qAttack.actions[i].dir.Equals("up")) {
                            Debug.Log(gm.qAttack.actions[i].delay);
                            Invoke("ActionWaterUp", gm.qAttack.actions[i].delay);
                        } else if (gm.qAttack.actions[i].dir.Equals("down")) {
                            Invoke("ActionWaterDown", gm.qAttack.actions[i].delay);
                        } else if (gm.qAttack.actions[i].dir.Equals("left")) {
                            Invoke("ActionWaterLeft", gm.qAttack.actions[i].delay);
                        } else if (gm.qAttack.actions[i].dir.Equals("right")) {
                            Invoke("ActionWaterRight", gm.qAttack.actions[i].delay);
                        }
                    }
                } else if (gm.qAttack.actions[i].type == 0) {
                    ActionError();
                }
            }
        } else
            gm.OpenCanvas(1);
    }

    public void PerformWAttack() {
        if (gm.wAttack.actions.Count > 0) {
            for (int i = 0; i < gm.wAttack.actions.Count; i++) {
                //Fireball
                if (gm.wAttack.actions[i].type == 1) {
                    if (gm.wAttack.actions[i].looping) {
                        if (gm.wAttack.actions[i].dir.Equals("up")) {
                            InvokeRepeating("ActionFireBallUp", 0, gm.wAttack.actions[i].delay);
                        } else if (gm.wAttack.actions[i].dir.Equals("down")) {
                            InvokeRepeating("ActionFireBallDown", 0, gm.wAttack.actions[i].delay);
                        } else if (gm.wAttack.actions[i].dir.Equals("left")) {
                            InvokeRepeating("ActionFireBallLeft", 0, gm.wAttack.actions[i].delay);
                        } else if (gm.wAttack.actions[i].dir.Equals("right")) {
                            InvokeRepeating("ActionFireBallRight", 0, gm.wAttack.actions[i].delay);
                        }
                    } else {
                        if (gm.wAttack.actions[i].dir.Equals("up")) {
                            Debug.Log(gm.wAttack.actions[i].delay);
                            Invoke("ActionFireBallUp", gm.wAttack.actions[i].delay);
                        } else if (gm.wAttack.actions[i].dir.Equals("down")) {
                            Invoke("ActionFireBallDown", gm.wAttack.actions[i].delay);
                        } else if (gm.wAttack.actions[i].dir.Equals("left")) {
                            Invoke("ActionFireBallLeft", gm.wAttack.actions[i].delay);
                        } else if (gm.wAttack.actions[i].dir.Equals("right")) {
                            Invoke("ActionFireBallRight", gm.wAttack.actions[i].delay);
                        }
                    }


                    //Water
                } else if (gm.wAttack.actions[i].type == 2) {
                    if (gm.wAttack.actions[i].looping) {
                        Debug.Log("Starting Water");
                        if (gm.wAttack.actions[i].dir.Equals("up")) {
                            InvokeRepeating("ActionWaterUp", 0, gm.wAttack.actions[i].delay);
                        } else if (gm.wAttack.actions[i].dir.Equals("down")) {
                            InvokeRepeating("ActionWaterDown", 0, gm.wAttack.actions[i].delay);
                        } else if (gm.wAttack.actions[i].dir.Equals("left")) {
                            InvokeRepeating("ActionWaterLeft", 0, gm.wAttack.actions[i].delay);
                        } else if (gm.wAttack.actions[i].dir.Equals("right")) {
                            InvokeRepeating("ActionWaterRight", 0, gm.wAttack.actions[i].delay);
                        }
                    } else {
                        if (gm.wAttack.actions[i].dir.Equals("up")) {
                            Debug.Log(gm.wAttack.actions[i].delay);
                            Invoke("ActionWaterUp", gm.wAttack.actions[i].delay);
                        } else if (gm.wAttack.actions[i].dir.Equals("down")) {
                            Invoke("ActionWaterDown", gm.wAttack.actions[i].delay);
                        } else if (gm.wAttack.actions[i].dir.Equals("left")) {
                            Invoke("ActionWaterLeft", gm.wAttack.actions[i].delay);
                        } else if (gm.wAttack.actions[i].dir.Equals("right")) {
                            Invoke("ActionWaterRight", gm.wAttack.actions[i].delay);
                        }
                    }
                } else if (gm.qAttack.actions[i].type == 0) {
                    ActionError();
                }
            }
        } else
            gm.OpenCanvas(1);
    }

    public void PerformEAttack() {
        if (gm.eAttack.actions.Count > 0) {
            for (int i = 0; i < gm.eAttack.actions.Count; i++) {
                //Fireball
                if (gm.eAttack.actions[i].type == 1) {
                    if (gm.eAttack.actions[i].looping) {
                        if (gm.eAttack.actions[i].dir.Equals("up")) {
                            InvokeRepeating("ActionFireBallUp", 0, gm.eAttack.actions[i].delay);
                        } else if (gm.eAttack.actions[i].dir.Equals("down")) {
                            InvokeRepeating("ActionFireBallDown", 0, gm.eAttack.actions[i].delay);
                        } else if (gm.eAttack.actions[i].dir.Equals("left")) {
                            InvokeRepeating("ActionFireBallLeft", 0, gm.eAttack.actions[i].delay);
                        } else if (gm.eAttack.actions[i].dir.Equals("right")) {
                            InvokeRepeating("ActionFireBallRight", 0, gm.eAttack.actions[i].delay);
                        }
                    } else {
                        if (gm.eAttack.actions[i].dir.Equals("up")) {
                            Debug.Log(gm.eAttack.actions[i].delay);
                            Invoke("ActionFireBallUp", gm.eAttack.actions[i].delay);
                        } else if (gm.eAttack.actions[i].dir.Equals("down")) {
                            Invoke("ActionFireBallDown", gm.eAttack.actions[i].delay);
                        } else if (gm.eAttack.actions[i].dir.Equals("left")) {
                            Invoke("ActionFireBallLeft", gm.eAttack.actions[i].delay);
                        } else if (gm.eAttack.actions[i].dir.Equals("right")) {
                            Invoke("ActionFireBallRight", gm.eAttack.actions[i].delay);
                        }
                    }


                    //Water
                } else if (gm.eAttack.actions[i].type == 2) {
                    if (gm.eAttack.actions[i].looping) {
                        Debug.Log("Starting Water");
                        if (gm.eAttack.actions[i].dir.Equals("up")) {
                            InvokeRepeating("ActionWaterUp", 0, gm.eAttack.actions[i].delay);
                        } else if (gm.eAttack.actions[i].dir.Equals("down")) {
                            InvokeRepeating("ActionWaterDown", 0, gm.eAttack.actions[i].delay);
                        } else if (gm.eAttack.actions[i].dir.Equals("left")) {
                            InvokeRepeating("ActionWaterLeft", 0, gm.eAttack.actions[i].delay);
                        } else if (gm.eAttack.actions[i].dir.Equals("right")) {
                            InvokeRepeating("ActionWaterRight", 0, gm.eAttack.actions[i].delay);
                        }
                    } else {
                        if (gm.eAttack.actions[i].dir.Equals("up")) {
                            Debug.Log(gm.eAttack.actions[i].delay);
                            Invoke("ActionWaterUp", gm.eAttack.actions[i].delay);
                        } else if (gm.eAttack.actions[i].dir.Equals("down")) {
                            Invoke("ActionWaterDown", gm.eAttack.actions[i].delay);
                        } else if (gm.eAttack.actions[i].dir.Equals("left")) {
                            Invoke("ActionWaterLeft", gm.eAttack.actions[i].delay);
                        } else if (gm.eAttack.actions[i].dir.Equals("right")) {
                            Invoke("ActionWaterRight", gm.eAttack.actions[i].delay);
                        }
                    }
                } else if (gm.qAttack.actions[i].type == 0) {
                    ActionError();
                }
            }
        } else
            gm.OpenCanvas(1);
    }

    public void PerformRAttack() {
        if (gm.rAttack.actions.Count > 0) {
            for (int i = 0; i < gm.rAttack.actions.Count; i++) {
                //Fireball
                if (gm.rAttack.actions[i].type == 1) {
                    if (gm.rAttack.actions[i].looping) {
                        if (gm.rAttack.actions[i].dir.Equals("up")) {
                            InvokeRepeating("ActionFireBallUp", 0, gm.rAttack.actions[i].delay);
                        } else if (gm.rAttack.actions[i].dir.Equals("down")) {
                            InvokeRepeating("ActionFireBallDown", 0, gm.rAttack.actions[i].delay);
                        } else if (gm.rAttack.actions[i].dir.Equals("left")) {
                            InvokeRepeating("ActionFireBallLeft", 0, gm.rAttack.actions[i].delay);
                        } else if (gm.rAttack.actions[i].dir.Equals("right")) {
                            InvokeRepeating("ActionFireBallRight", 0, gm.rAttack.actions[i].delay);
                        }
                    } else {
                        if (gm.rAttack.actions[i].dir.Equals("up")) {
                            Debug.Log(gm.rAttack.actions[i].delay);
                            Invoke("ActionFireBallUp", gm.rAttack.actions[i].delay);
                        } else if (gm.rAttack.actions[i].dir.Equals("down")) {
                            Invoke("ActionFireBallDown", gm.rAttack.actions[i].delay);
                        } else if (gm.rAttack.actions[i].dir.Equals("left")) {
                            Invoke("ActionFireBallLeft", gm.rAttack.actions[i].delay);
                        } else if (gm.rAttack.actions[i].dir.Equals("right")) {
                            Invoke("ActionFireBallRight", gm.rAttack.actions[i].delay);
                        }
                    }


                    //Water
                } else if (gm.rAttack.actions[i].type == 2) {
                    if (gm.rAttack.actions[i].looping) {
                        Debug.Log("Starting Water");
                        if (gm.rAttack.actions[i].dir.Equals("up")) {
                            InvokeRepeating("ActionWaterUp", 0, gm.rAttack.actions[i].delay);
                        } else if (gm.rAttack.actions[i].dir.Equals("down")) {
                            InvokeRepeating("ActionWaterDown", 0, gm.rAttack.actions[i].delay);
                        } else if (gm.rAttack.actions[i].dir.Equals("left")) {
                            InvokeRepeating("ActionWaterLeft", 0, gm.rAttack.actions[i].delay);
                        } else if (gm.rAttack.actions[i].dir.Equals("right")) {
                            InvokeRepeating("ActionWaterRight", 0, gm.rAttack.actions[i].delay);
                        }
                    } else {
                        if (gm.rAttack.actions[i].dir.Equals("up")) {
                            Debug.Log(gm.rAttack.actions[i].delay);
                            Invoke("ActionWaterUp", gm.rAttack.actions[i].delay);
                        } else if (gm.rAttack.actions[i].dir.Equals("down")) {
                            Invoke("ActionWaterDown", gm.rAttack.actions[i].delay);
                        } else if (gm.rAttack.actions[i].dir.Equals("left")) {
                            Invoke("ActionWaterLeft", gm.rAttack.actions[i].delay);
                        } else if (gm.rAttack.actions[i].dir.Equals("right")) {
                            Invoke("ActionWaterRight", gm.rAttack.actions[i].delay);
                        }
                    }
                } else if (gm.qAttack.actions[i].type == 0) {
                    ActionError();
                }
            }
        } else
            gm.OpenCanvas(1);
    }

    public void ActionFireBallUp() {
        if (soulProcessor >= 0) {
            Debug.Log("FIREBALL! up");
            GameObject p = Instantiate(fireball, transform.position, transform.rotation) as GameObject;
            p.GetComponent<Rigidbody2D>().velocity = new Vector2(0, fireballlSpeed);
            soulProcessor -= fireballCost;
        }
    }

    public void ActionFireBallDown() {
        if (soulProcessor >= 0) {
            Debug.Log("FIREBALL! down");
            GameObject p = Instantiate(fireball, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), transform.rotation) as GameObject;
            p.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -fireballlSpeed);
            soulProcessor -= fireballCost;
        }
    }

    public void ActionFireBallLeft() {
        if (soulProcessor >= 0) {
            Debug.Log("FIREBALL! left");
            GameObject p = Instantiate(fireball, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), transform.rotation) as GameObject;
            p.GetComponent<Rigidbody2D>().velocity = new Vector2(-fireballlSpeed, 0);
            soulProcessor -= fireballCost;
        }
    }

    public void ActionFireBallRight() {
        if (soulProcessor >= 0) {
            Debug.Log("FIREBALL! right");
            GameObject p = Instantiate(fireball, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), transform.rotation) as GameObject;
            p.GetComponent<Rigidbody2D>().velocity = new Vector2(fireballlSpeed, 0);
            soulProcessor -= fireballCost;
        }
    }

    public void ActionWaterUp() {
        if (soulProcessor >= 0) {
            Debug.Log("WATER! up");
            GameObject p = Instantiate(water, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), transform.rotation) as GameObject;
            p.GetComponent<Rigidbody2D>().velocity = new Vector2(0, waterSpeed);
            soulProcessor -= waterCost;
        }
    }

    public void ActionWaterDown() {
        if (soulProcessor >= 0) {
            Debug.Log("WATER! down");
            GameObject p = Instantiate(water, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), transform.rotation) as GameObject;
            p.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -waterSpeed);
            soulProcessor -= waterCost;
        }
    }

    public void ActionWaterRight() {
        if (soulProcessor >= 0) {
            Debug.Log("WATER! right");
            GameObject p = Instantiate(water, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), transform.rotation) as GameObject;
            p.GetComponent<Rigidbody2D>().velocity = new Vector2(waterSpeed, 0);
            soulProcessor -= waterCost;
        }
    }

    public void ActionWaterLeft() {
        if (soulProcessor >= 0) {
            Debug.Log("WATER! left");
            GameObject p = Instantiate(water, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), transform.rotation) as GameObject;
            p.GetComponent<Rigidbody2D>().velocity = new Vector2(-waterSpeed, 0);
            soulProcessor -= waterCost;
        }
    }

    public void ActionWater() {
        Debug.Log("WATER!");
    }

    public void RechargeSP() {
        if (soulProcessor < soulCapacity)
            soulProcessor += 1;
    }

    public void CancelAllLoops() {
        CancelInvoke("ActionFireBallUp");
        CancelInvoke("ActionFireBallDown");
        CancelInvoke("ActionFireBallLeft");
        CancelInvoke("ActionFireBallRight");
        CancelInvoke("ActionWaterUp");
        CancelInvoke("ActionWaterDown");
        CancelInvoke("ActionWaterLeft");
        CancelInvoke("ActionWaterRight");

    }
}
