using UnityEngine;
using UnityEngine.Video;

using ServerDefense.Common.Controller;

namespace ServerDefense.Gameplay.Splash
{
    public class SplashController : SceneController
    {
        [SerializeField] private SCENE targetScene = SCENE.NONE;
        [SerializeField] private VideoPlayer video = null;

        protected override void Awake()
        {
            video.loopPointReached += (video) => ChangeSceneOnVideoEnd();
        }

        protected override void OnDisable()
        {
        }

        protected override void OnEnable()
        {
        }

        private void ChangeSceneOnVideoEnd()
        {
            ChangeScene(targetScene, true);
        }
    }
}