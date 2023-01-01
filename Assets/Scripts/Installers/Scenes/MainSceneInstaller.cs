using System;
using System.Linq;
using Events.External;
using Events.Internal;
using Zenject;

namespace Installers.Scenes
{
    public class MainSceneInstaller : MonoInstaller<MainSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<MainSceneInputEvents>().AsSingle();
            Container.Bind<PlayerEvents>().AsSingle();

            Container.BindInterfacesAndSelfTo<Test>().AsSingle();
        }
    }
}