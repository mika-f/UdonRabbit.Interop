﻿using Mochizuki.VRChat.Interop.Validator.Attributes;

using UdonSharp;

using UnityEngine;

namespace Mochizuki.VRChat.Interop.NoSynced
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    [DefaultExecutionOrder(-1)]
    public class ToggleButton : UdonSharpBehaviour
    {
        [SerializeField]
        private bool initialState;

        [SerializeField]
        [NoSyncedEvent]
        private EventListener listener;

        [SerializeField]
        private InteropLogger logger;

        private bool _hasListener;
        private bool _hasLogger;
        private bool _localState;

        private void Start()
        {
            _localState = initialState;
            _hasListener = listener != null;
            _hasLogger = logger != null;

            if (_hasListener)
                listener.SetArgument(_localState);
        }

        public override void Interact()
        {
            _localState = !_localState;

            if (_hasLogger)
                logger.LogInfo($"{nameof(Interact)} - OnInteracted (Not Synced, No Request Synchronized)");

            if (_hasListener)
            {
                listener.SetArgument(_localState);
                listener.OnInteracted();
            }
        }
    }
}