using UnityEngine;
using Leopotam.EcsLite;
using Lib;

namespace Voody.UniLeo.Lite
{
    public abstract class MonoProvider<T> : BaseMonoProvider where T : struct
    {
        [SerializeField] protected T value;
        private EcsPool<T> _pool;

        public override void Attach(int entity, EcsWorld world)
        {
            _pool ??= world.GetPool<T>();
            _pool.Add(entity) = value;
        }
    }
}