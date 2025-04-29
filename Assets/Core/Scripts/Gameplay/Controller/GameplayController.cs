using UnityEngine;

public class GameplayController : Singleton<GameplayController>
{
    [field: SerializeField] public GameplayEvent GameplayEvent { get; private set; }
    [field: SerializeField] public PlayerInputEvent PlayerInputEvent { get; private set; }

    public void OnDestroy()
    {
        GameplayEvent.ClearAllListener();
        PlayerInputEvent.ClearAllListener();
    }
}
