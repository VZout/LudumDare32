using UnityEngine;
using System.Collections;

public class PopupScript : MonoBehaviour {

    public float activeTime = 3.5f;

	void Start () {
	
	}
	
	void Update () {
	    if(gameObject.activeSelf) {
            Invoke("DeActivate", activeTime);
        }
	}

    void DeActivate() {
        this.gameObject.SetActive(false);
    }
}
