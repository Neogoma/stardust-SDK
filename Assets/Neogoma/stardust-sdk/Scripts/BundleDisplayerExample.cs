using com.Neogoma.Stardust.API.Persistence;
using com.Neogoma.Stardust.Datamodel;
using Siccity.GLTFUtility;
using UnityEngine;

namespace com.Neogoma.Stardust.Persistence
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

        protected override GameObject LoadGLBFile(string filepath)
        {
            ImportSettings settings = new ImportSettings();
            settings.useLegacyClips = true;
            AnimationClip[] animations;
            GameObject loadedObject= Importer.LoadFromFile(filepath,settings,out animations,Format.AUTO);

            if (animations.Length > 0)
            {
                Animation anim = loadedObject.AddComponent<Animation>();
                animations[0].legacy = true;
                anim.AddClip(animations[0], animations[0].name);
                anim.clip = anim.GetClip(animations[0].name);
                anim.wrapMode = WrapMode.Loop;
                anim.Play();
            }


            return loadedObject;
        }



        ///<inheritdoc/>
        protected override void ObjectLoadedFailure()
        {

        }
        ///<inheritdoc/>
        protected override void ObjectLoadedSucessfully(GameObject obj,PersistentObject persistent)
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
            Debug.Log(gameObject.name + " " + progressEvent);
        }
    }
}
