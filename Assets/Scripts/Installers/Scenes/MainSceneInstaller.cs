using Events.External;
using Zenject;

namespace Installers.Scenes
{
    public class MainSceneInstaller : MonoInstaller<MainSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<MainSceneInputEvents>().AsSingle();
            Container.Bind<PlayerEvents>().AsSingle();
        }
    }
}