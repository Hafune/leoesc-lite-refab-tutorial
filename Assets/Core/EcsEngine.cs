using Core.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Lib;

namespace Core
{
    public class EcsEngine : MonoConstruct
    {
        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystems;

        private void Awake()
        {
            var world = Context.Resolve<EcsWorld>();

            _updateSystems = new EcsSystems(world);
            _fixedUpdateSystems = new EcsSystems(world);
        
            _updateSystems
                .Add(new EventRemoveEntitySystem())
                .Add(new EventSetupVirtualCameraFollowSystem(Context))
                .Add(new MoveSystem(Context))
#if UNITY_EDITOR
                // Регистрируем отладочные системы по контролю за состоянием каждого отдельного мира:
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(bakeComponentsInName: false));
#endif
            ;

            _updateSystems.Inject();
            _fixedUpdateSystems.Inject();

            _updateSystems.Init();
            _fixedUpdateSystems.Init();
        }

        private void Update() => _updateSystems.Run();

        private void FixedUpdate() => _fixedUpdateSystems.Run();

        private void OnDestroy()
        {
            _updateSystems.Destroy();
            _fixedUpdateSystems.Destroy();
        }
    }
}