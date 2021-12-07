using com.Neogoma.Stardust.API.Mapping;
using System;
using System.Collections;
using UnityEngine;

namespace Neogoma.Stardust.Demo
{
    public class HintAlertManager:MonoBehaviour
    {
        /// <summary>
        /// Triggered when moving too fast
        /// </summary>
        public GameObject movingTooFast;

        /// <summary>
        /// Triggered when the mergency cut happened
        /// </summary>
        public GameObject emergencyCut;

        public void Awake()
        {
            MapDataUploader.Instance.onEmergencyCut.AddListener(EmergencyTriggered);
            MapDataUploader.Instance.onMovingTooFast.AddListener(MovingTooFast);
        }

        private void MovingTooFast()
        {
            movingTooFast.gameObject.SetActive(true);
            StartCoroutine(HideHint(movingTooFast));
        }

        private void EmergencyTriggered()
        {
            emergencyCut.gameObject.SetActive(true);
            StartCoroutine(HideHint(emergencyCut));
        }

        private IEnumerator HideHint(GameObject hint)
        {
            yield return new WaitForSeconds(2);

            hint.gameObject.SetActive(false);
        }
    }
}
