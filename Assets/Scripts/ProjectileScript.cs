using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

    public int damage = 25;
    public GameObject player;
    public bool error = false;

    void Start() {
        if (!error) {
            player = GameObject.Find("Character");
            Physics2D.IgnoreLayerCollision(8, 8, true);
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        } else {
            Invoke("KillStuff", 0.5f);
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Enemy") {
            coll.gameObject.GetComponent<EnemyScript>().health -= damage;
            KillStuff();
        } else if(coll.gameObject.tag == "Wall") {
            KillStuff();
        }
    }

    void KillStuff() {
        Destroy(transform.FindChild("Particle System").gameObject, 1.5f);
        GetComponentInChildren<ParticleSystem>().Stop();
        transform.FindChild("Particle System").parent = null; ;
        Destroy(this.gameObject);
    }
}
