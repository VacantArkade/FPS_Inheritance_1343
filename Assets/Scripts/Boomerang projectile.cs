using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoomerangProjectile : MonoBehaviour
{
    float damageAmount;
    float speed;
    float knockback;
    float lifetime;
    float halfLife;
    UnityAction<HitBoomData> OnHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(float damage, float velocity, float life, float force, UnityAction<HitBoomData> onHit)
    {
        damageAmount = damage;
        speed = velocity;
        lifetime = life;
        knockback = force;
        OnHit += onHit;
        halfLife = lifetime / 2;

        if (lifetime < halfLife)
        {
            GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
        }
        else
        {
            GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
        }
        Destroy(gameObject, lifetime);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<Damageable>();
        if (target != null)
        {
            var direction = GetComponent<Rigidbody>().linearVelocity;
            direction.Normalize();

            Debug.Log("hit enemy trigger");
            target.Hit(direction * knockback, damageAmount);

            HitBoomData hd = new HitBoomData();
            hd.target = target;
            hd.direction = direction;
            hd.location = transform.position;

            OnHit?.Invoke(hd);
        }

        Destroy(gameObject);
    }
}

public class HitBoomData
{
    public Vector3 location;
    public Vector3 direction;
    public Damageable target;
}
