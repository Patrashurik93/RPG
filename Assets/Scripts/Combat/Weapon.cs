using RPG.Attributes;
using System;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/New Weapon")]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController AnimatorOverride;
        [SerializeField] GameObject EquipedPrefab = null;
        [SerializeField] float WeaponRange = 2.0f;
        [SerializeField] float TimeBetweenAttacks = 1.0f;
        [SerializeField] float WeaponDamage = 5f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public void Spawn(Transform RightHand, Transform LeftHand, Animator animator)
        {
            DestroyOldWeapon(RightHand, LeftHand);

            if (EquipedPrefab != null)
            {
                Transform handTransform = GetTransfom(RightHand, LeftHand);
                GameObject weapon = Instantiate(EquipedPrefab, handTransform);
                weapon.name = weaponName;
            }
            if (AnimatorOverride != null)
            {
                animator.runtimeAnimatorController = AnimatorOverride;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }

            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransfom(Transform RightHand, Transform LeftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = RightHand;
            else handTransform = LeftHand;
            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransfom(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, WeaponDamage);
        }

        public float GetDamage()
        {
            return WeaponDamage;
        }

        public float GetWeaponRange()
        {
            return WeaponRange;
        }

        public float GetTimeBetweenAttacks()
        {
            return TimeBetweenAttacks;
        }
    }
}