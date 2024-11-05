using UnityEngine;
using UnityEngine.Events;

public class EventsManager : MonoBehaviour
{
    private static EventsManager instance;
    public static EventsManager Instance()
    {
        if (instance == null)
        {
            GameObject eventsManager = Instantiate(Resources.Load("Prefabs/Singletons/EventsManager", typeof(GameObject)) as GameObject);
            instance = eventsManager.GetComponent<EventsManager>();
        }
        return instance;
    }

    public UnityEvent OnPlayerSpotted;

    public void PlayerSpotted()
    {
        OnPlayerSpotted.Invoke();
    }

    private void Awake()
    {
        instance = this;
    }
}
