using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

    public int damage = 25;
    public float speed = 8;
    public int health = 100;
    public GameObject player;
    public float knockbackForce = 1;
    public float range = 2;
    private bool following = false;

	// Use this for initialization
	void Start () {
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
	}
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < range)
            following = true;
        if (following)
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if (health <= 0) {
            player.GetComponent<PlayerScript>().kills++;
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if(coll.gameObject.tag == "Player") {
            Debug.Log("Coll");
            coll.gameObject.GetComponent<PlayerScript>().health -= damage;
            coll.gameObject.GetComponent<Animator>().SetTrigger("Hit");

            var x = Random.Range(-1f, 1f);
            var y = Random.Range(-1f, 1f);
            var direction = new Vector2(x, y);
             direction = direction.normalized;

            GetComponent<Rigidbody2D>().AddForce(direction * knockbackForce, ForceMode2D.Force);
        }
    }
}
