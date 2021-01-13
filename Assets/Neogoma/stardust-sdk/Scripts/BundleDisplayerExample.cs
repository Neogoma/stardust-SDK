using com.Neogoma.Stardust.API.Persistence;
using UnityEngine;

namespace com.Neogoma.Stardust.Bundle
{
    /// <summary>
    /// Base extension of <see cref="AbstractBundleDisplayer"/>
    /// </summary>
    public class BundleDisplayerExample : AbstractBundleDisplayer
    {
        /// <summary>
        /// Object used to display the loading progresses
        /// </summary>
        [Tooltip("GameObject to display when the object is loading")]
        public GameObject progressBg;

        

        ///<inheritdoc/>
        protected override void ObjectLoadedFailure()
        {

        }
        ///<inheritdoc/>
        protected override void ObjectLoadedSucessfully(GameObject obj)
        {
            progressBg.SetActive(false);
        }
        ///<inheritdoc/>
        protected override void ObjectNotAvailable()
        {
            Debug.LogWarning("This bundle is not available on this platform");
        }

        ///<inheritdoc/>
        protected override void OnDownloadUpdate(float progressEvent)
        {



        }
    }
}
