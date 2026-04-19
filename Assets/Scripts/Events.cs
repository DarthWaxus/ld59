using UnityEngine.Events;

public class Events
{
    public static UnityEvent GameOverEvent = new UnityEvent();
    public static UnityEvent<Unit> UnitDeadEvent = new UnityEvent<Unit>();
}