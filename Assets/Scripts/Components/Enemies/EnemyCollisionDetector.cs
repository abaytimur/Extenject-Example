using Events.Internal;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Components.Enemies
{
    public class EnemyCollisionDetector : MonoBehaviour, IAttackable
    {
        private int _enemyHits = 2;
        [UsedImplicitly] [Inject] private EnemyInternalEvents EnemyInternalEvents { get; set; }

        EnemyInternalEvents IAttackable.OnWeaponTriggerEnter()
        {
            _enemyHits--;
            if (_enemyHits ==0 )
            {
                OnDeath?.Invoke(); 
            }
            return EnemyInternalEvents;
        }

        public UnityAction OnDeath { get; set; }
    }
}