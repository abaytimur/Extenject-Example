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
        [Inject] private EnemyInternalEvents EnemyInternalEvents { get; set; }

        EnemyInternalEvents IAttackable.OnWeaponTriggerEnter()
        {
            _enemyHits--;
            if (_enemyHits == 0)
            {
                OnDeath?.Invoke(this);
            }

            return EnemyInternalEvents;
        }

        public UnityAction<IAttackable> OnDeath { get; set; }
    }
}