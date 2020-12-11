using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Neogoma.Stardust.LightEstimation
{
    [RequireComponent(typeof(Light))]
    public class LightEstimation : MonoBehaviour
    {
        
        private Light lightComponent;

        ///<inheritdoc/>
        public void Start()
        {

            ARCameraManager cameraManager = FindObjectOfType<ARCameraManager>();

            if (cameraManager == null)
            {
                Debug.LogWarning("No camera manager in scene, light estimation can't be done");

                return;
            }

            cameraManager.frameReceived += OnFrameReceived;

            lightComponent = GetComponent<Light>();            
        }

        private void OnFrameReceived(ARCameraFrameEventArgs obj)
        {
            ARLightEstimationData lightEstimation = obj.lightEstimation;

            if (lightEstimation.averageBrightness.HasValue)
            {
                lightComponent.intensity = lightEstimation.averageBrightness.Value;
            }

            if (lightEstimation.averageColorTemperature.HasValue)
            {
                lightComponent.colorTemperature = lightEstimation.averageColorTemperature.Value;
            }

            if (lightEstimation.colorCorrection.HasValue)
            {
                lightComponent.color = lightEstimation.colorCorrection.Value;
            }

            if (lightEstimation.mainLightDirection.HasValue)
            {
                lightComponent.transform.rotation = Quaternion.LookRotation(lightEstimation.mainLightDirection.Value);
            }

            if (lightEstimation.mainLightColor.HasValue)
            {
                lightComponent.color = lightEstimation.mainLightColor.Value;
            }

            if (lightEstimation.mainLightIntensityLumens.HasValue)
            {
                lightComponent.intensity = lightEstimation.averageMainLightBrightness.Value;
            }

            if (lightEstimation.ambientSphericalHarmonics.HasValue)
            {
                RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
                RenderSettings.ambientProbe = lightEstimation.ambientSphericalHarmonics.Value;
            }
        }
    }

}