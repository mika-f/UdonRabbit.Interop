﻿/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using UdonSharp;

using UnityEngine;

using VRC.SDKBase;

namespace Mochizuki.VRChat.Interop
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class ToggleButton : UdonSharpBehaviour
    {
        private bool _hasListener;
        private bool _hasLogger;
        private bool _isInteracted;
        private bool _localState;

        [UdonSynced]
        private bool _state;

        private void Start()
        {
            _state = initialState;
            _hasListener = listener != null;
            _hasLogger = logger != null;
        }

        private void Update()
        {
            if (_isInteracted && Networking.IsOwner(Networking.LocalPlayer, gameObject))
                ToggleState();
        }

        public override void Interact()
        {
            if (!_hasListener)
                return;

            if (!Networking.IsOwner(Networking.LocalPlayer, gameObject))
                Networking.SetOwner(Networking.LocalPlayer, gameObject);

            if (_hasLogger)
                logger.LogInfo($"{nameof(Interact)} - Request Manual Synchronization");

            _isInteracted = true;
        }

        private void ToggleState()
        {
            _isInteracted = false;
            _localState = !_localState;

            // for owner
            if (_hasListener)
            {
                listener.SetArgument(_localState);
                listener.OnInteracted();
            }

            RequestSerialization();
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (player == Networking.LocalPlayer)
                return;

            if (Networking.IsOwner(Networking.LocalPlayer, gameObject))
                RequestSerialization();
        }

        public override void OnPreSerialization()
        {
            _state = _localState;

            if (_hasLogger)
            {
                logger.LogInfo($"{nameof(OnPreSerialization)} - Request Serialization Variables for Synchronization");
                logger.LogInfo($"{nameof(OnPreSerialization)} - Current State is {_state}");
            }
        }

        public override void OnDeserialization()
        {
            if (_hasLogger)
            {
                logger.LogInfo($"{nameof(OnDeserialization)} - Received Serialization Variables for Synchronization");
                logger.LogInfo($"{nameof(OnDeserialization)} - Current State is {_state}");
            }

            _localState = _state;

            if (_hasListener)
            {
                listener.SetArgument(_localState);
                listener.OnInteracted();
            }
        }

#pragma warning disable CS0649

        [SerializeField]
        private bool initialState;

        [SerializeField]
        private EventListener listener;

        [SerializeField]
        private InteropLogger logger;

#pragma warning restore CS0649
    }
}