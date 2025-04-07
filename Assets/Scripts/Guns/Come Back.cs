using UnityEngine;

public class ComeBack : MonoBehaviour
{
    GameObject player;
    Transform position;

    [SerializeField] float speed;
    [SerializeField] float lifetime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player = FindFirstObjectByType<FPSController>().gameObject;
        position = player.transform;

        //GetComponent<Rigidbody>().linearVelocity = transform.position * speed;
        //Destroy(gameObject, lifetime);
        transform.position = Vector3.Lerp(transform.position, position.position, Time.deltaTime * speed);
    }
}
