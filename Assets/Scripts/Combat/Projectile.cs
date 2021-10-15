using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] float speed = 1f;
        [SerializeField] bool IsHoming;
        [SerializeField] GameObject HitEffect = null;
        [SerializeField] float MaxLifeTme = 10;
        [SerializeField] GameObject[] DestroyAtHit = null;
        [SerializeField] float lifeaAfterImpact = 2;

        Health target = null;
        float damage = 0;
        GameObject instigator = null;
        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }
        void Update()
        {
            if (target == null) return;

            if (IsHoming == true && !target.isDead())
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, MaxLifeTme);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            {
                if (target.isDead())
                {
                    return;
                }
                target.TakeDamage(instigator, damage);

                speed = 0;

                if (HitEffect != null)
                {
                    Instantiate(HitEffect, GetAimLocation(), Quaternion.identity);
                }

                foreach (GameObject toDestroy in DestroyAtHit)
                {
                    Destroy(toDestroy);
                }

                Destroy(gameObject, lifeaAfterImpact);
            }
        }
    }
}
