using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon Weapon = null;
        [SerializeField] float respawntime = 5;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(Weapon);
                StartCoroutine(HideForSeconds(respawntime));
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            HidePickup();
            yield return new WaitForSeconds(seconds);
            ShowPickup();
        }

        void HidePickup()
        {
            Collider collider = GetComponent<Collider>();
            collider.enabled = false;
            Renderer prefab = GetComponentInChildren<Renderer>();
            prefab.enabled = false;
        }
        void ShowPickup()
        {
            Collider collider = GetComponent<Collider>();
            collider.enabled = true;
            Renderer prefab = GetComponentInChildren<Renderer>();
            prefab.enabled = true;
        }
    }
}