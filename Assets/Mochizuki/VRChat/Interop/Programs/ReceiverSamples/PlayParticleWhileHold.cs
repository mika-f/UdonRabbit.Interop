﻿/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using UdonSharp;

using UnityEngine;

namespace Mochizuki.VRChat.Interop.ReceiverSamples
{
    public class PlayParticleWhileHold : UdonSharpBehaviour
    {
        [SerializeField]
        private EventListener listener;

        [SerializeField]
        private ParticleSystem effect;

        private void Start()
        {
            effect.Stop();
        }

        private void Update()
        {
            if (listener.IsPickupUseDowned())
                effect.Play();

            if (listener.IsPickupUseUpped())
                effect.Stop();
        }
    }
}