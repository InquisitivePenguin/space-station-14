﻿using System;
using Content.Server.Explosions;
using Content.Server.GameObjects.Components.Chemistry;
using Content.Shared.Interfaces;
using Robust.Shared.Interfaces.GameObjects;
using Robust.Shared.Serialization;

namespace Content.Server.Chemistry.ReactionEffects
{
    class ExplosionReactionEffect : IReactionEffect
    {
        private float _devastationRange;
        private float _heavyImpactRange;
        private float _lightImpactRange;
        private float _flashRange;

        /// <summary>
        /// If true, then scale ranges by intensity. If not, the ranges are the same regardless of reactant amount.
        /// </summary>
        private bool _scaled;
        /// <summary>
        /// Maximum scaling on ranges. For example, if it equals 5, then it won't scaled anywhere past
        /// 5 times the minimum reactant amount.
        /// </summary>
        private float _maxScale;

        public void ExposeData(ObjectSerializer serializer)
        {
            serializer.DataField(ref _devastationRange, "devastationRange", 1);
            serializer.DataField(ref _heavyImpactRange, "heavyImpactRange", 2);
            serializer.DataField(ref _lightImpactRange, "lightImpactRange", 3);
            serializer.DataField(ref _flashRange, "flashRange", 0);

            serializer.DataField(ref _scaled, "scaled", false);
            serializer.DataField(ref _maxScale, "maxScale", 1);
        }

        public void React(IEntity solutionEntity, decimal intensity)
        {
            float floatIntensity = (float)intensity; 
            if (solutionEntity == null)
                return;
            if(!solutionEntity.TryGetComponent(out SolutionComponent solution))
                return;

            //Handle scaling
            if (_scaled)
            {
                floatIntensity = Math.Min(floatIntensity, _maxScale);
            }
            else
            {
                floatIntensity = 1;
            }

            //Calculate intensities
            int finalDevastationRange = (int)Math.Round(_devastationRange * floatIntensity);
            int finalHeavyImpactRange = (int)Math.Round(_heavyImpactRange * floatIntensity);
            int finalLightImpactRange = (int)Math.Round(_lightImpactRange * floatIntensity);
            int finalFlashRange = (int)Math.Round(_flashRange * floatIntensity);
            ExplosionHelper.SpawnExplosion(solutionEntity.Transform.GridPosition, finalDevastationRange,
                finalHeavyImpactRange, finalLightImpactRange, finalFlashRange);
        }
    }
}


