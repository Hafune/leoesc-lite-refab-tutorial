using System;
using Core.Components;
using Leopotam.EcsLite;
using Lib;

namespace Voody.UniLeo.Lite
{
    public class ConvertToEntity : MonoConstruct
    {
        private Action<int, EcsWorld>[] _cache;
        private EcsPool<EventRemoveEntity> _eventRemovePool;
        private EcsWorld _world;
        
        public int RawEntity { get; private set; } = -1;

        private void Awake()
        {
            _world ??= Context.Resolve<EcsWorld>();
            _eventRemovePool = _world.GetPool<EventRemoveEntity>();

            MakeCache();
        }

        private void OnEnable() => ConnectToWorld();

        private void OnDisable() => DisconnectFromWorld();

        public void RemoveConnectionInfo()
        {
            if (RawEntity == -1)
                return;

            RawEntity = -1;
            gameObject.SetActive(false);
        }

        private void MakeCache()
        {
            var list = gameObject.GetComponents<BaseMonoProvider>();
            _cache = new Action<int, EcsWorld>[list.Length];

            for (int i = 0, iMax = list.Length; i < iMax; i++)
            {
                var entityComponent = list[i];
                _cache[i] = entityComponent.Attach;
                Destroy(entityComponent);
            }
        }

        private void ConnectToWorld()
        {
            RawEntity = _world.NewEntity();
            
            for (int i = 0, iMax = _cache.Length; i < iMax; i++)
                _cache[i].Invoke(RawEntity, _world);
        }

        private void DisconnectFromWorld()
        {
            if (RawEntity == -1)
                return;

            _eventRemovePool.AddIfNotExist(RawEntity);
            RemoveConnectionInfo();
        }
    }
}