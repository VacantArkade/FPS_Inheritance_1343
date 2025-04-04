using UnityEngine;

public class Boomerang : Gun
{
    public override bool AttemptFire()
    {
        if (!base.AttemptFire())
            return false;

        var b = Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);
        b.GetComponent<BoomerangProjectile>().Initialize(10, 1, 20, 5, null); // version without special effect
        //b.GetComponent<Projectile>().Initialize(1, 100, 2, 5, DoThing); // version with special effect

        anim.SetTrigger("shoot");
        elapsed = 0;
        ammo -= 1;

        return true;
    }
    void DoThing(HitBoomData data)
    {
        Vector3 impactLocation = data.location;

        var colliders = Physics.OverlapSphere(impactLocation, 5);
        foreach (var c in colliders)
        {
            if (c.GetComponent<Rigidbody>())
            {
                c.GetComponent<Rigidbody>().AddForce(Vector3.up * 20, ForceMode.Impulse);
            }
        }
    }
}
