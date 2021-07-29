# Static Update Manager

Simple to use garbage-free System.Action based Static Update Manager for Unity.

This works by utilizing [PlayerLoop API](https://docs.unity3d.com/ScriptReference/LowLevel.PlayerLoop.html) introduced in Unity 2018.1 version.

Tested on Unity 2019.4 and 2020.3 LTS versions.

# Why?

  * Allows more controlled update behaviour
  * Improves performance (a bit) as each MonoBehaviour 'magic' Fixed/Late/Update methods adds some overhead due native C++ to managed C# call. See this [Unity blog](https://blog.unity.com/technology/1k-update-calls) 
  * Ideal for MonoBehaviour based Jobs that run often or every frame. With this you can  start Jobs early in the frame (EarlyUpdate) and end them at right before new frame starts (PostLateUpdate).

# How To Use

Have [UpdateManager.cs](https://github.com/anarkila/Static-Update-Manager/blob/main/Assets/Scripts/UpdateManager.cs) in your project and add/remove events to it when needed.

```C#
using UpdateLoop;

public class ExampleScript : MonoBehaviour {

    private void OnEnable() {
        UpdateManager.AddUpdateEvent(UpdateEvent.EarlyUpdate, EarlyUpdate);
        UpdateManager.AddUpdateEvent(UpdateEvent.PostLateUpdate, PostLateUpdate);
    }
    
    private void OnDisable() {
        UpdateManager.RemoveUpdateEvent(UpdateEvent.EarlyUpdate, EarlyUpdate);
        UpdateManager.RemoveUpdateEvent(UpdateEvent.PostLateUpdate, PostLateUpdate);
    }

    private void EarlyUpdate() {    // this is called before any Update() function
        Debug.Log("EarlyUpdate");
    }

    private void PostLateUpdate() { // this is called after all LateUpdate() functions
        Debug.Log("PostLateUpdate");
    }
}
```
