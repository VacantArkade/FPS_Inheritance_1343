using UnityEngine;

public class GunSide : Gun
{
    float betweenTime = 0.3f;
    float counter = 0.0f;

    private void Update()
    {
        counter += Time.deltaTime;
        if (counter >= betweenTime)
        {
            var b = Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);
            b.GetComponent<Projectile>().Initialize(3, 100, 5, 2, null); // version without special effect
            anim.SetTrigger("shoot");
            counter = 0.0f;
            //ammo -= 1;
        }
    }
}
