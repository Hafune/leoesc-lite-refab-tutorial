using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

namespace Lib
{
    public static class MyLeoEcsExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetOrInitialize<T>(this EcsPool<T> poolInject, int entity) where T : struct =>
            ref poolInject.Has(entity)
                ? ref poolInject.Get(entity)
                : ref poolInject.Add(entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DelIfExist<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (!pool.Has(entity))
                return;

            pool.Del(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddIfNotExist<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (pool.Has(entity))
                return;

            pool.Add(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasEntity(this EcsFilter filter, int entity) => filter.GetSparseIndex()[entity] != 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetFirst(this EcsFilter filter) => filter.GetRawEntities()[0];
    }
}