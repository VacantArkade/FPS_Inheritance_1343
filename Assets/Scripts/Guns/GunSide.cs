using UnityEngine;

public class GunSide : Gun
{
    /*public override bool AttemptFire()
    {
        if (!base.AttemptFire())
            return false;

        var b = Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);
        b.GetComponent<Projectile>().Initialize(3, 100, 5, 2, null); // version without special effect
        //b.GetComponent<Projectile>().Initialize(1, 100, 2, 5, DoThing); // version with special effect

        anim.SetTrigger("shoot");
        elapsed = 0;
        ammo -= 1;

        return true;
    }*/

    private void Update()
    {

        var b = Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);
        b.GetComponent<Projectile>().Initialize(3, 100, 5, 2, null); // version without special effect
                                                                     //b.GetComponent<Projectile>().Initialize(1, 100, 2, 5, DoThing); // version with special effect

        anim.SetTrigger("shoot");
        //elapsed = 0;
        ammo -= 1;

    }
}
