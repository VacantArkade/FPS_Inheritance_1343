using UnityEngine;

public class BoomerangRotation : MonoBehaviour
{
    Vector3 holdRotation;
    [SerializeField] float speed;
    public bool equipped = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        holdRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if(!equipped)
        {
            transform.Rotate(Vector3.up, speed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Euler( holdRotation);
        }
    }
}
