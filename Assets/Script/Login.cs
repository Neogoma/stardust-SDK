using com.Neogoma.HoboDream;
using com.Neogoma.HoboDream.Impl;
using com.Neogoma.HoboDream.UI.Loading;
using com.Neogoma.Stardust.API;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Neogoma.Stardust.Demo
{
    /// <summary>
    /// used to login the app
    /// </summary>
    /// <seealso cref="com.Neogoma.HoboDream.Impl.AbstractInteractive" />
    public class Login : AbstractInteractive
    {
        /// <summary>
        /// The email
        /// </summary>
        public InputField email;

        /// <summary>
        /// The password
        /// </summary>
        public InputField password;

        public UnityEvent loginFailed = new UnityEvent();
        public UnityEvent loginSucceed = new UnityEvent();

        private LoadSelectionSceneEvent sceneLoading;


        /// <summary>
        /// Logins the server.
        /// </summary>
        public void LoginServer()
        {
            StardustSDK.Instance.Login(email.text, password.text);
        }

        private void LoadMappingScene()
        {
            loginSucceed.Invoke();
            NotifyListeners(sceneLoading);
        }

        private void LoginFailed()
        {
            loginFailed.Invoke();
        }

        protected override void DoOnDestroy()
        {
            
        }

        protected override void DoOnAwake()
        {
            sceneLoading = new LoadSelectionSceneEvent(this);
            AddInteractiveListener(SceneLoadingController.Instance);
            if (StardustSDK.Instance.IsLoggedIn())
                LoadMappingScene();
            else
            {
                StardustSDK.Instance.onLoginSuceed.AddListener(LoadMappingScene);
                StardustSDK.Instance.onLoginFailed.AddListener(LoginFailed);

            }
                
            
        }

        private class LoadSelectionSceneEvent : BaseInteractionEvent, ISceneLoadingEvent
        {
            public LoadSelectionSceneEvent(IInteractiveElement source) : base(source, InteractiveEventAction.START_LOADSCENE)
            {
            }

            public int GetSceneIndex()
            {
                return 1;
            }
        }
    }
}