using UnityEngine;

public class Character : MonoBehaviour
{
    public Collider weaponCollider;
    public void PlayAttackSound()
    {
        Sound.I.Play("Slash1");
    }

    public void EnableWeapon()
    {
        weaponCollider.enabled = true;
    }

    public void DisableWeapon()
    {
        weaponCollider.enabled = false;
    }
}