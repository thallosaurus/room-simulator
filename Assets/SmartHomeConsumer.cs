using UnityEngine;

public abstract class SmartHomeConsumer : MonoBehaviour {
    public abstract void UpdateFromState(State state);
}

