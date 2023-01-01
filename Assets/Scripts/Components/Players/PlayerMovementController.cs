using System.Reflection;
using Events.External;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Components.Players
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Inject] private MainSceneInputEvents MainSceneInputEvents { get; set; }

        [SerializeField] private NavMeshAgent navMeshAgent;

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnRegisterEvents();
        }

        private void RegisterEvents()
        {
            MainSceneInputEvents.OnInputUpdate += OnInputUpdate;
        }

        private void OnInputUpdate(MainSceneInputEvents.InputUpdate inputUpdate)
        {
            Debug.LogWarning(MethodBase.GetCurrentMethod().Name);
            navMeshAgent.destination = inputUpdate.TerrainPosition;
        }

        private void UnRegisterEvents()
        {
            MainSceneInputEvents.OnInputUpdate -= OnInputUpdate;
        }
    }
}