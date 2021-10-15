using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Saving;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        private bool wasPlayed = false;
        private void OnTriggerEnter(Collider other)
        {
            if (!wasPlayed && other.gameObject.CompareTag("Player"))
            {
                wasPlayed = true;
                GetComponent<PlayableDirector>().Play();
            }
        }

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["Cinematic"] = wasPlayed;
            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;
            wasPlayed = (bool)data["Cinematic"];
            Debug.Log("Restored " + wasPlayed);
        }
    }

}