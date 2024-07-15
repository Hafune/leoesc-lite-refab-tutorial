using Core.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Reflex;
using UnityEngine;

namespace Core.Systems
{
    public class MoveSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<
            Inc<
                RigidbodyComponent,
                SpeedValueComponent
            >> _filter;

        private readonly EcsPoolInject<RigidbodyComponent> _rigidbodyPool;
        private readonly EcsPoolInject<SpeedValueComponent> _speedValuePool;

        private PlayerInputs.PlayerActions _playerInputs;

        public MoveSystem(Context context) => _playerInputs = context.Resolve<PlayerInputs.PlayerActions>();

        public void Run(IEcsSystems systems)
        {
            foreach (var i in _filter.Value)
                UpdateEntity(i);
        }

        private void UpdateEntity(int entity)
        {
            var direction = _playerInputs.Move.ReadValue<Vector2>();
            var body = _rigidbodyPool.Value.Get(entity).rigidbody;
            var speed = _speedValuePool.Value.Get(entity).value;
            body.velocity = new Vector3(direction.x, 0, direction.y) * speed;
        }
    }
}