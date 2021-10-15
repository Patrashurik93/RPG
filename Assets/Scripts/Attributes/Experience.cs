using UnityEngine;

namespace RPG.Attributes
{
    class Experience : MonoBehaviour
    {
        [SerializeField] float experiencePoint = 0;

        public void GainExperience(float experience)
        {
            experiencePoint += experience;
        }
    }
}
