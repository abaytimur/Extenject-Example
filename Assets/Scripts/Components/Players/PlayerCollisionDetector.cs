using System.Collections.Generic;
using Components.Enemies;
using UnityEngine;

namespace Components.Players
{
    public class PlayerCollisionDetector : MonoBehaviour
    {
        private readonly List<IAttackable> _attackedTargets = new();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IAttackable attackable))
            {
                _attackedTargets.Add(attackable);
                attackable.OnDeath += OnAttackedDeath;

                attackable.OnWeaponTriggerEnter();
            }
        }

        private void OnAttackedDeath(IAttackable diedAttackable)
        {
            diedAttackable.OnDeath -= OnAttackedDeath;
            _attackedTargets.Remove(diedAttackable);
            Debug.LogWarning("Target Died");
        }
    }
}