using System.Reflection;
using Events.External;
using Extensions.Unity;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Components.Players
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private Transform myTransform;
        [SerializeField] private NavMeshAgent navMeshAgent;

        [Inject] private MainSceneInputEvents MainSceneInputEvents { get; set; }
        [Inject] private PlayerEvents PlayerEvents { get; set; }

        private RoutineHelper _onPositionUpdate;

        private void OnEnable()
        {
            RegisterEvents();
            _onPositionUpdate = new RoutineHelper(this, null, OnPositionUpdate, () => true);
        }

        private void OnDisable()
        {
            UnRegisterEvents();
        }

        private void OnPositionUpdate()
        {
            PlayerEvents.OnPlayerMove?.Invoke(myTransform.position);
            
            if (navMeshAgent.isStopped)
            {
                _onPositionUpdate.StopCoroutine();
            }
        }

        private void RegisterEvents()
        {
            MainSceneInputEvents.OnInputUpdate += OnInputUpdate;
        }

        private void OnInputUpdate(MainSceneInputEvents.InputUpdate inputUpdate)
        {
            if (!_onPositionUpdate.IsInvoking)
            {
                _onPositionUpdate.StartCoroutine();
            }

            // Debug.LogWarning(MethodBase.GetCurrentMethod().Name);
            navMeshAgent.destination = inputUpdate.TerrainPosition;
        }

        private void UnRegisterEvents()
        {
            MainSceneInputEvents.OnInputUpdate -= OnInputUpdate;
        }
    }
}