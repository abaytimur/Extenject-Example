using System;
using Components.Enemies;
using UnityEngine;

namespace Components.Players
{
    public class PlayerCollisionDetector : MonoBehaviour
    {
        private IAttackable _lastAttackedTarget;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IAttackable attackable))
            {
                if (_lastAttackedTarget != attackable)
                {
                    if (_lastAttackedTarget is not null)
                    {
                        _lastAttackedTarget.OnDeath -= OnAttackedDeath;
                    }

                    attackable.OnDeath += OnAttackedDeath;
                }


                attackable.OnWeaponTriggerEnter();

                _lastAttackedTarget = attackable;
            }
        }

        private void OnAttackedDeath()
        {
            Debug.LogWarning("Target Died");
        }
    }
}