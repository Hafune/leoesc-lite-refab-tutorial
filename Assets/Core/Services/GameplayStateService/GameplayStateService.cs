using System;
using Core.Components;
using Leopotam.EcsLite;
using Lib;
using UnityEngine;
using Voody.UniLeo.Lite;

namespace Core.Services
{
    [Serializable]
    public class GameplayStateService : MonoConstruct, IAbstractGlobalState
    {
        [SerializeField] private ConvertToEntity _playerCharacterPrefab;
        private ConvertToEntity _playerCharacter;
        private GlobalStateService _globalStateService;
        private PlayerInputs.PlayerActions _playerInputs;
        private int _totalInputUsers;
        private EcsPool<EventSetupVirtualCameraFollow> _cameraSetupPool;

        private void Awake()
        {
            _globalStateService = Context.Resolve<GlobalStateService>();
            _playerInputs = Context.Resolve<PlayerInputs.PlayerActions>();
            _cameraSetupPool = Context.Resolve<EcsWorld>().GetPool<EventSetupVirtualCameraFollow>();
        }

        public void EnableState()
        {
            _globalStateService.ChangeActiveState(this);
            Debug.Log("GameplayStateService enabled");
            BuildCharacter();
        }

        public void DisableState() => RemoveCharacterAndControls();

        public void PauseInputs()
        {
            if (--_totalInputUsers != 0)
                return;

            _playerInputs.Disable();
        }

        public void ResumeInputs()
        {
            if (++_totalInputUsers != 1)
                return;

            _playerInputs.Enable();
        }

        private void RemoveCharacterAndControls()
        {
            PauseInputs();
            Destroy(_playerCharacter.gameObject);
            _playerCharacter = null;
        }

        private void BuildCharacter()
        {
            if (_playerCharacter is not null)
                return;

            _playerCharacter = Context.Instantiate(_playerCharacterPrefab, Vector3.zero, Quaternion.identity);
            _cameraSetupPool.Add(_playerCharacter.RawEntity);
            DontDestroyOnLoad(_playerCharacter);
            
            ResumeInputs();
        }
    }
}