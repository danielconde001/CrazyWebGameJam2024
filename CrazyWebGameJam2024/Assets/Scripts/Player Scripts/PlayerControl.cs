using UnityEngine;

public abstract class PlayerControl : MonoBehaviour
{
    protected bool canControl = true;
    public void LoseControl()
    {
        canControl = false;
    }

    public void RegainControl()
    {
        canControl = true;
    }
}
