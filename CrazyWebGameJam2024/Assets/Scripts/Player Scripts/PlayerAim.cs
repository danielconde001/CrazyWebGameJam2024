using UnityEngine;

public class PlayerAim : PlayerControl
{
    [SerializeField] private GameObject weaponAnchor;
    [SerializeField] private GameObject center;
    [SerializeField] private bool useCrosshair = true;
    private Vector3 mousePos;
    private Vector2 aimDirection;

    private void Update()
    {
        if(!canControl)
            return;

        // Convert Mouse Position to World Positon, also set mousePos.z to 0
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Move crosshair using Mouse Position
        if (useCrosshair) GameManager.Instance().MoveCrosshair(mousePos);

        // Get the Normalized Vector between the Player's postion and Mouse Postion
        aimDirection = (mousePos - center.transform.position).normalized;

        // Get the angle of the aforementioned Vector
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        
        // Set Weapon Anchor's z rotation equal to the angle
        weaponAnchor.transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    public Vector2 GetAimDirection()
    {
        return aimDirection;
    }

    public Transform GetCenterTransform()
    {
        return center.transform;
    }
}
