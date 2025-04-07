using UnityEngine;

public class Boomerang : Gun
{
    [SerializeField] GameObject returnBoom;
    public override bool AttemptFire()
    {
        if (!base.AttemptFire())
            return false;

        var b = Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);
        b.GetComponent<BoomerangProjectile>().Initialize(10, 5, 20, 5, DoThing); // version without special effect
        //b.GetComponent<Projectile>().Initialize(1, 100, 2, 5, DoThing); // version with special effect

        anim.SetTrigger("shoot");
        elapsed = 0;
        ammo -= 1;
        FindAnyObjectByType<FPSController>().RemoveGun(this);
        return true;
    }
    void DoThing(HitBoomData data)
    {
        Debug.Log("HI");
        Vector3 impactLocation = data.location;
        var p = Instantiate(returnBoom, data.location, Quaternion.identity);

        /*var colliders = Physics.OverlapSphere(impactLocation, 5);
        foreach (var c in colliders)
        {
            if (c.GetComponent<Rigidbody>())
            {
                c.GetComponent<Rigidbody>().AddForce(Vector3.up * 20, ForceMode.Impulse);
            }
        }*/
    }
}
