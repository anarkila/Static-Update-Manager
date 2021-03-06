# Static Update Manager

Simple to use System.Action based Static Update Manager for Unity.

This works by utilizing [PlayerLoop API](https://docs.unity3d.com/ScriptReference/LowLevel.PlayerLoop.html) introduced in Unity 2018.1 version. This API is not well documented but it allows modifying, adding or removing calls from Unity Engine internal loop.

Available queues:

* EarlyUpdate
* Update
* LateUpdate
* PostLateUpdate
* FixedUpdate
* EverySecondFrame

# Why?

  * Allows more controlled update behaviour.
  * Improves performance (a bit) as each MonoBehaviour 'magic' Fixed/Late/Update method has some overhead due native C++ to managed C# call. See this [Unity blog.](https://blog.unity.com/technology/1k-update-calls) 
  * Ideal for MonoBehaviour based Jobs that run often or every frame. With this you can  start Jobs early in the frame (EarlyUpdate) and end them at right before new frame starts (PostLateUpdate), giving your Jobs as much time to execute while your other game logic executes.

# How To Use

1. Download and import [StaticUpdateManager package](https://github.com/anarkila/Static-Update-Manager/releases/download/v1.02/StaticUpdateManager.unitypackage) into your project.

2. Add / Remove events to / from UpdateManager like below.

Remember to remove your events when you no longer need them!

```C#
using UnityEngine;

public class ExampleScript : MonoBehaviour {

    private void OnEnable() {
        UpdateManager.AddUpdateEvent(UpdateEvent.EarlyUpdate, EarlyUpdate);
        UpdateManager.AddUpdateEvent(UpdateEvent.PostLateUpdate, PostLateUpdate);
    }
    
    private void OnDisable() {
        UpdateManager.RemoveUpdateEvent(UpdateEvent.EarlyUpdate, EarlyUpdate);
        UpdateManager.RemoveUpdateEvent(UpdateEvent.PostLateUpdate, PostLateUpdate);
    }

    private void EarlyUpdate() {     // this is called before any Update() method
        Debug.Log("EarlyUpdate");
    }

    private void PostLateUpdate() {  // this is called after all LateUpdate() methods
        Debug.Log("PostLateUpdate");
    }
}
```
