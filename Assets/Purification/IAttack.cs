using System.Collections;
using UnityEngine;

namespace Assets.Purification
{
    /// <summary>
    /// Interface that caracterizes any component that can be treated as an attack
    /// </summary>
    public interface IAttack
    {
        /// <summary>
        /// Fire the attack in the given direction
        /// </summary>
        /// <param name="direction">The direction in which to fire</param>
        void Fire(Vector3 direction);
    }
}