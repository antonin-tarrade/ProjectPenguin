using Attacks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Purification
{
    namespace Effects
    {
        public enum EffectType { damager, speedModifier }

        public interface IEffect<ComponentType>
        {
            /// <summary>
            /// Determines if the effect should be applied or not
            /// </summary>
            /// <param name="obj">The object on which to apply the effect</param>
            /// <returns>If the effect should be applied</returns>
            delegate bool CanApplyFunc(GameObject obj);
            CanApplyFunc CanApply { get; set; }
            void ApplyOn(GameObject gameObject);
            void ApplyOn(ComponentType component);
        }

        public abstract class Effect<ComponentType> : IEffect<ComponentType>
        {
            public IEffect<ComponentType>.CanApplyFunc CanApply { get; set; }

            public void ApplyOn(GameObject obj)
            {
                bool canApply = true;
                if (CanApply != null) canApply = CanApply(obj);
                if (!canApply) return;
                if (obj.TryGetComponent<ComponentType>(out ComponentType component))
                {
                    ApplyOn(component);
                }
            }

            public abstract void ApplyOn(ComponentType component);
        }

        public class DamageEffect : Effect<IDamageable>
        {
            private float dmg;

            public DamageEffect(float dmg)
            {
                this.dmg = dmg;
            }
            public override void ApplyOn(IDamageable component)
            {
                component.DoDmg(dmg);
            }
        }

        public class SpeedModifier : Effect<Movement>
        {
            private float modifierValue;
            private float time;

            public SpeedModifier(float modifierValue, float time)
            {
                this.modifierValue = modifierValue;
                this.time = time;
            }

            public override void ApplyOn(Movement component)
            {
                component.ModifySpeed(modifierValue, time);
            }
        }
    }

}