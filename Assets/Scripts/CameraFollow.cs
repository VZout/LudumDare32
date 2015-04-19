using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    void FixedUpdate () {
        Vector3 point = Camera.main.WorldToViewportPoint(player.transform.position);
        Vector3 delta = player.transform.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
    }
}
