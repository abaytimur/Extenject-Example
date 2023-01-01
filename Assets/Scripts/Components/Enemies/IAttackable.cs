using Events.Internal;
using UnityEngine.Events;

namespace Components.Enemies
{
    public interface IAttackable
    {
        EnemyInternalEvents OnWeaponTriggerEnter();
        UnityAction OnDeath { get; set; }
    }
}