using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;

namespace Neogoma.Stardust.LightEstimation
{
    [RequireComponent(typeof(Light))]
    public class LightEstimation : MonoBehaviour
    {

        private Light lightComponent;

        private ARCameraManager arCameraManager;

        ///<inheritdoc/>
        public void Awake()
        {

            arCameraManager = FindObjectOfType<ARCameraManager>();

            if (arCameraManager == null)
            {
                Debug.LogWarning("No camera manager in scene, light estimation can't be done");

                return;
            }


            lightComponent = GetComponent<Light>();

            arCameraManager.frameReceived += OnFrameReceived;
        }

        private void OnFrameReceived(ARCameraFrameEventArgs args)
        {

            if (args.lightEstimation.averageBrightness.HasValue)
            {
                lightComponent.intensity = args.lightEstimation.averageBrightness.Value;
            }

            if (args.lightEstimation.averageColorTemperature.HasValue)
            {
                lightComponent.colorTemperature = args.lightEstimation.averageColorTemperature.Value;
            }

            if (args.lightEstimation.colorCorrection.HasValue)
            {
                lightComponent.color = args.lightEstimation.colorCorrection.Value;
            }

            if (args.lightEstimation.mainLightDirection.HasValue)
            {
                lightComponent.transform.rotation = Quaternion.LookRotation(args.lightEstimation.mainLightDirection.Value);
            }

            if (args.lightEstimation.mainLightColor.HasValue)
            {

                lightComponent.color = args.lightEstimation.mainLightColor.Value;
            }

            if (args.lightEstimation.mainLightIntensityLumens.HasValue)
            {
                lightComponent.intensity = args.lightEstimation.averageMainLightBrightness.Value;
            }

            if (args.lightEstimation.ambientSphericalHarmonics.HasValue)
            {
                RenderSettings.ambientMode = AmbientMode.Skybox;
                RenderSettings.ambientProbe = args.lightEstimation.ambientSphericalHarmonics.Value;
            }
        }
    }

}