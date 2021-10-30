using UnityEngine;
using UpdateLoop;

public class RotateCube : MonoBehaviour {

    [SerializeField] private float degreesPerSecond = 29.0f;
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 1f;

    private Transform cachedTransform;
    private Vector3 startPosition;
    private Vector3 tempPos;

    private void Awake() {
        cachedTransform = this.transform;
        startPosition = cachedTransform.position;
    }

    private void OnEnable() {
        UpdateManager.AddUpdateEvent(UpdateEvent.NormalUpdate, CustomUpdate);
    }

    private void OnDisable() {
        UpdateManager.RemoveUpdateEvent(UpdateEvent.NormalUpdate, CustomUpdate);
    }

    private void CustomUpdate() {
        // Spin object around Y-Axis
        var rotation = new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f);
        cachedTransform.Rotate(rotation, Space.World);

        // Float up/down
        tempPos = startPosition;
        tempPos.y += Mathf.Sin(Time.time * Mathf.PI * frequency) * amplitude;
        cachedTransform.position = tempPos;
    }
}
