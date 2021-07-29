using System.Collections.Generic;
using UnityEngine.PlayerLoop;
using UnityEngine.LowLevel;
using UnityEngine;
using System;

// System.Action based Static UpdateManager
// Instead of multiple MonoBehaviour scripts calling Unity magic Fixed/Late/Update functions
// This script loops through all registered Update events
// whichs eliminates overhead (Native C++ to managed C# calls), see this Unity blog
// https://blog.unity.com/technology/1k-update-calls

// This also allows custom update flow which can be nice for several reasons
// For example MonoBehaviour based Jobs, you can start Job(s) early in EarlyUpdate
// and end them in PostLateUpdate while your other game logic executes

// This works by using Unity's PlayerLoop API which is not that well documented
// https://docs.unity3d.com/ScriptReference/LowLevel.PlayerLoop.html
// See UnityCsReference PlayerLoop.bindings.cs for more information about update order of all Unity native systems
// https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/PlayerLoop/PlayerLoop.bindings.cs

// NOTE. You have to register and unregister updates manually. Use this with care!
// For profiling, turn Deep Profile on!

namespace UpdateLoop {

    public static class UpdateManager {

        private static List<Action> normalUpdate = new List<Action>(50);                // normal Update() behaviour
        private static List<Action> earlyUpdate = new List<Action>(50);                 // early update (ideal for starting Jobs)
        private static List<Action> normalLateUpdate = new List<Action>(50);            // normal LateUpdate() behaviour
        private static List<Action> postLateUpdate = new List<Action>(50);              // postlateupdate (ideal for ending Jobs)
        private static List<Action> fixedUpdate = new List<Action>(50);                 // normal FixedUpdate() behaviour
        private static List<Action> everySecondFrame = new List<Action>(50);            // every second frame
        private static bool setupDone = false;
        private static int frameCount = 0;

        /// <summary> 
        /// Setup UpdateManager before scene loads
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]      // Comment this line if you don't want to use this manager!
        private static void Setup() {
            if (setupDone) return;

            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            for (int i = 0; i < playerLoop.subSystemList.Length; i++) {
                if (playerLoop.subSystemList[i].type == typeof(PreUpdate)) {
                    playerLoop.subSystemList[i].updateDelegate += EarlyUpdate;
                }
                if (playerLoop.subSystemList[i].type == typeof(Update)) {
                    playerLoop.subSystemList[i].updateDelegate += Update;
                }
                if (playerLoop.subSystemList[i].type == typeof(PreLateUpdate)) {        // MonoBehaviour LateUpdate() calls belong to PreLateUpdate phase
                    playerLoop.subSystemList[i].updateDelegate += LateUpdate;
                }
                if (playerLoop.subSystemList[i].type == typeof(PostLateUpdate)) {
                    playerLoop.subSystemList[i].updateDelegate += PostLateUpdate;
                }
                if (playerLoop.subSystemList[i].type == typeof(FixedUpdate)) {
                    playerLoop.subSystemList[i].updateDelegate += FixedUpdate;
                }
            }
            PlayerLoop.SetPlayerLoop(playerLoop);
            setupDone = true;


#if UNITY_EDITOR // You can comment these lines
            Debug.Log("UpdateManager Initialized");
#endif
        }

        /// <summary> 
        /// Add event (Action) to UpdateManager
        /// </summary>
        public static void AddUpdateEvent(UpdateEvent updateEvent, Action updateAction) {
            if (updateAction == null) return;

            switch (updateEvent) {
                case UpdateEvent.NormalUpdate:
                    normalUpdate.Add(updateAction);
                    break;
                case UpdateEvent.EarlyUpdate:
                    earlyUpdate.Add(updateAction);
                    break;
                case UpdateEvent.NormalLateUpdate:
                    normalLateUpdate.Add(updateAction);
                    break;
                case UpdateEvent.PostLateUpdate:
                    postLateUpdate.Add(updateAction);
                    break;
                case UpdateEvent.FixedUpdate:
                    fixedUpdate.Add(updateAction);
                    break;
                case UpdateEvent.EverySecondFrame:
                    everySecondFrame.Add(updateAction);
                    break;
            }
        }

        /// <summary> 
        /// Remove event (Action) from UpdateManager
        /// </summary>
        public static void RemoveUpdateEvent(UpdateEvent updateEvent, Action updateAction) {
            if (updateAction == null) return;

            switch (updateEvent) {
                case UpdateEvent.NormalUpdate:
                    normalUpdate.Remove(updateAction);
                    break;
                case UpdateEvent.EarlyUpdate:
                    earlyUpdate.Remove(updateAction);
                    break;
                case UpdateEvent.NormalLateUpdate:
                    normalLateUpdate.Remove(updateAction);
                    break;
                case UpdateEvent.PostLateUpdate:
                    postLateUpdate.Remove(updateAction);
                    break;
                case UpdateEvent.FixedUpdate:
                    fixedUpdate.Remove(updateAction);
                    break;
                case UpdateEvent.EverySecondFrame:
                    everySecondFrame.Remove(updateAction);
                    break;
            }
        }

        /// <summary> 
        /// Loop through all the registered EarlyUpdate events
        /// </summary>
        private static void EarlyUpdate() {
            for (int i = 0; i < earlyUpdate.Count; i++) {
                earlyUpdate[i].Invoke();
            }
        }

        /// <summary> 
        /// Loop through all the registered Update events
        /// </summary>
        private static void Update() {
            for (int i = 0; i < normalUpdate.Count; i++) {
                normalUpdate[i].Invoke();
            }

            ++frameCount;  // You can move these 5 lines to Early/Post/lateUpdate if you wish
            if (frameCount % 2 == 0) {
                EverySecondFrameUpdate();
                frameCount = 0;
            }
        }

        /// <summary> 
        /// Loop through all the registered LateUpdate events
        /// </summary>
        private static void LateUpdate() {
            for (int i = 0; i < normalLateUpdate.Count; i++) {
                normalLateUpdate[i].Invoke();
            }
        }

        /// <summary> 
        /// Loop through all the registered PostLateUpdate events
        /// </summary>
        private static void PostLateUpdate() {
            for (int i = 0; i < postLateUpdate.Count; i++) {
                postLateUpdate[i].Invoke();
            }
        }

        /// <summary> 
        /// Loop through all the registered FixedUpdate events
        /// </summary>
        private static void FixedUpdate() {
            for (int i = 0; i < fixedUpdate.Count; i++) {
                fixedUpdate[i].Invoke();
            }
        }

        /// <summary> 
        /// Loop through all the registered FixedUpdate events
        /// </summary>
        private static void EverySecondFrameUpdate() {
            for (int i = 0; i < everySecondFrame.Count; i++) {
                everySecondFrame[i].Invoke();
            }
        }
    }

    public enum UpdateEvent {
        EarlyUpdate,
        NormalUpdate,
        NormalLateUpdate,
        PostLateUpdate,
        FixedUpdate,
        EverySecondFrame
    }
}