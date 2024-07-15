using System;
using UnityEngine;
using Voody.UniLeo.Lite;

namespace Core.Components
{
    [Serializable]
    public struct TransformComponent
    {
        public Transform transform;
        public ConvertToEntity convertToEntity;
    }
}