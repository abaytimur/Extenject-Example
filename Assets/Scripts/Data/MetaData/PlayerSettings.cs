using Components.Players;
using UnityEngine;

namespace Data.MetaData
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "ZenjectExample/PlayerSettings")]
    public class PlayerSettings : ScriptableObject
    {
        public PlayerCameraController.Settings playerCameraControllerSettings;
        
    }
}