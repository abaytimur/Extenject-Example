using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Events.External
{
    [UsedImplicitly]
    public class MainSceneInputEvents
    {
        public UnityAction OnInputBegin;
        public UnityAction<InputUpdate> OnInputUpdate;
        public UnityAction OnInputEnd;

        public readonly struct InputUpdate
        {
            public readonly Vector3 TerrainPosition;

            public InputUpdate(Vector3 terrainPosition)
            {
                TerrainPosition = terrainPosition;
            }
        }
    }
}