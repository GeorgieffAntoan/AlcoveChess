using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class GarbageCollectionManager : MonoBehaviour
{
    [SerializeField] private float maxTimeBetweenGarbageCollections = 20f;
    private float _timeSinceLastGarbageCollection;
    private void Start()
    {
    GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
        // You might want to run this during loading times, screen fades and such.
        // Events.OnScreenFade += CollectGarbage;
    }
    private void Update()
    {
        _timeSinceLastGarbageCollection += Time.unscaledDeltaTime;
        if (_timeSinceLastGarbageCollection > maxTimeBetweenGarbageCollections)
        {
            CollectGarbage();
        }
    }
    private void CollectGarbage()
    {
        _timeSinceLastGarbageCollection = 0f;
        Debug.Log("Collecting garbage"); // talking about garbage... 
    // Not supported on the editor
    GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
    System.GC.Collect();
    GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;

    }
}
