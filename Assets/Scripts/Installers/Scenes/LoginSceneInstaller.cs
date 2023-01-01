using UnityEngine.SceneManagement;
using Zenject;

namespace Installers.Scenes
{
    public class LoginSceneInstaller : MonoInstaller<LoginSceneInstaller>
    {
        private const int MainSceneBuildIndex = 1;

        public override void InstallBindings()
        {
        }

        private void Awake()
        {
            SceneManager.LoadScene(MainSceneBuildIndex);
        }
    }
}
