using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100;
        private bool _isDead;

        private void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public bool isDead()
        {
            return _isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {    
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if(healthPoints == 0 && _isDead == false)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetPercentage()
        {
            return  100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        private void Die()
        {
            if (_isDead) return;
            _isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CurrentActionDisable();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }


        public object CaptureState()
        {
            return healthPoints;
        }
        public void RestoreState(object state)
        {
            healthPoints = (float)state;

            if (healthPoints == 0 && _isDead == false)
            {
                Die();
            }


            if (healthPoints > 0 && _isDead == true)
            {
                GetComponent<Animator>().Rebind();
                GetComponent<ActionScheduler>().CurrentActionDisable();
                _isDead = false;
            }

        }
    }

}