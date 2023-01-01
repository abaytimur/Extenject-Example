using System;
using Data.MetaData;
using Events.External;
using Extensions.Unity;
using UnityEngine;
using Zenject;

namespace Components.Players
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private Transform myTransform;
        [Inject] private PlayerEvents PlayerEvents { get; set; }
        [Inject] private PlayerSettings PlayerSettings { get; set; }

        private Settings _mySettings;

        private void Awake()
        {
            _mySettings = PlayerSettings.playerCameraControllerSettings;
        }

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            PlayerEvents.OnPlayerMove += OnPlayerMove;
        }

        private void UnregisterEvents()
        {
            PlayerEvents.OnPlayerMove -= OnPlayerMove;
        }

        private void OnPlayerMove(Vector3 playerPosition)
        {
            myTransform.position = playerPosition + _mySettings.cameraOffset;
        }

        [Serializable]
        public class Settings
        {
            public Vector3 cameraOffset;
        }
    }
}