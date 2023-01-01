using Events.External;
using Extensions.Unity;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Components
{
    public class MainSceneInputReceiver : MonoBehaviour
    {
        private RoutineHelper _inputRoutine;
        private bool _isMouseButtonDown;
        private Vector3 _lastInputPosition;
        private Vector3 _mousePositionDelta;
        private Camera _mainCamera;

        [UsedImplicitly] [Inject] private MainSceneInputEvents MainSceneInputEvents { get; set; }

        private void Awake()
        {
            //TODO: Temporary
            _mainCamera = Camera.main;

            _inputRoutine = new RoutineHelper
            (
                this,
                null,
                InputUpdate,
                () => true
            );
        }

        private void OnEnable()
        {
            _inputRoutine.StartCoroutine();
        }

        private void OnDisable()
        {
            _inputRoutine.StartCoroutine();
        }

        private void InputUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isMouseButtonDown = true;
                MainSceneInputEvents.OnInputBegin?.Invoke();
            }
            else if (_isMouseButtonDown)
            {
                if (TryGetTerrainInputPos(Input.mousePosition, out Vector3 terrainInputPos))
                {
                    MainSceneInputEvents.InputUpdate inputUpdate = new(terrainInputPos);

                    MainSceneInputEvents.OnInputUpdate?.Invoke(inputUpdate);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isMouseButtonDown = false;
                MainSceneInputEvents.OnInputEnd?.Invoke();
            }
        }

        private bool TryGetTerrainInputPos(Vector3 currMousePos, out Vector3 terrainInputPos)
        {
            Ray inputRay = _mainCamera.ScreenPointToRay(currMousePos);

            Debug.DrawRay(inputRay.origin, inputRay.direction);

            bool didInputRayHit = Physics.Raycast(inputRay, out RaycastHit inputRayHit);

            if (didInputRayHit)
            {
                if (inputRayHit.collider.gameObject.TryGetComponent(out Terrain _))
                {
                    terrainInputPos = inputRayHit.point;
                    return true;
                }
            }

            terrainInputPos = new Vector3();
            return false;
        }
    }
}