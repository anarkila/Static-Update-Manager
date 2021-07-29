using UnityEngine;
using UpdateLoop;

public class ExampleScript : MonoBehaviour {

    private void OnEnable() {
        // Add events to UpdateManager when this script/gameobject is enabled
        UpdateManager.AddUpdateEvent(UpdateEvent.EarlyUpdate, EarlyUpdate);
        UpdateManager.AddUpdateEvent(UpdateEvent.NormalUpdate, NormalUpdate);
        UpdateManager.AddUpdateEvent(UpdateEvent.NormalLateUpdate, NormalLateUpdate);
        UpdateManager.AddUpdateEvent(UpdateEvent.PostLateUpdate, PostLateUpdate);
        UpdateManager.AddUpdateEvent(UpdateEvent.EverySecondFrame, EverySecondFrame);
        UpdateManager.AddUpdateEvent(UpdateEvent.FixedUpdate, NormalFixedUpdate);
    }

    private void OnDisable() {
        // Remove events from UpdateManager after this script/gameobject is disabled
        // Remember to remove your events after you no longer need them!
        UpdateManager.RemoveUpdateEvent(UpdateEvent.EarlyUpdate, EarlyUpdate);
        UpdateManager.RemoveUpdateEvent(UpdateEvent.NormalUpdate, NormalUpdate);
        UpdateManager.RemoveUpdateEvent(UpdateEvent.NormalLateUpdate, NormalLateUpdate);
        UpdateManager.RemoveUpdateEvent(UpdateEvent.PostLateUpdate, PostLateUpdate);
        UpdateManager.RemoveUpdateEvent(UpdateEvent.EverySecondFrame, EverySecondFrame);
        UpdateManager.RemoveUpdateEvent(UpdateEvent.FixedUpdate, NormalFixedUpdate);
    }

    private void EarlyUpdate() {            // This runs first, before any other Update()
        Debug.Log("Early Update");
    }

    private void NormalUpdate() {           // Normal Update() behaviour
        Debug.Log("Normal Update");
    }

    private void NormalLateUpdate() {       // Normal LateUpdate() behaviour
        Debug.Log("LateUpdate");
    }

    private void PostLateUpdate() {         // This runs last (after LateUpdate) before new frame starts
        Debug.Log("PostLateUpdate");
    }

    private void NormalFixedUpdate() {      // Normal FixedUpdate() behaviour
        Debug.Log("FixedUpdate");
    }

    private void EverySecondFrame() {       // This runs every second frame
        Debug.Log("Every second frame");
    }
}