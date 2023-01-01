using Events.Internal;
using Zenject;

namespace Installers.Prefabs
{
    public class EnemyInstaller: MonoInstaller<EnemyInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<EnemyInternalEvents>().AsSingle();
        }
    }
}