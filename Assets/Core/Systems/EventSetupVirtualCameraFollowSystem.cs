using Cinemachine;
using Core.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Reflex;

namespace Core.Systems
{
    public class EventSetupVirtualCameraFollowSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<
            Inc<
                TransformComponent, 
                EventSetupVirtualCameraFollow
            >> _filter;

        private readonly EcsPoolInject<TransformComponent> _transformPool;
        private readonly EcsPoolInject<EventSetupVirtualCameraFollow> _eventVirtualCameraFollowSetupPool;

        private CinemachineVirtualCamera _virtualCamera;

        public EventSetupVirtualCameraFollowSystem(Context context) =>
            _virtualCamera = context.Resolve<CinemachineVirtualCamera>();

        public void Run(IEcsSystems systems)
        {
            foreach (var i in _filter.Value)
            {
                var transform = _transformPool.Value.Get(i).transform;
                _virtualCamera.PreviousStateIsValid = false;
                _virtualCamera.Follow = transform;
                _eventVirtualCameraFollowSetupPool.Value.Del(i);
            }
        }
    }
}