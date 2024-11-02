using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    public static PlayerManager Instance()
    {
        if (instance == null)
        {
            GameObject playerManager = new GameObject("PlayerManager");
            instance = playerManager.AddComponent<PlayerManager>();
        }
        return instance;
    }
    
    [SerializeField] private GameObject player;
    public GameObject GetPlayer()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        return player;
    }
    
    [SerializeField] private Transform weaponAnchor;
    public Transform WeaponAnchor
    {
        get
        {
            if (weaponAnchor == null)
            {
                weaponAnchor = player.transform.GetChild(0);
            }
            
            return weaponAnchor;
        }
    }
    
    [SerializeField] private Weapon currentlyEquippedWeapon;
    public Weapon CurrentlyEquippedWeapon
    {
        get
        {
            if (currentlyEquippedWeapon == null)
            {
                currentlyEquippedWeapon = weaponAnchor.GetChild(0).GetComponent<Weapon>();
            }
            
            return currentlyEquippedWeapon;
        }
    }
    
    public void ReplaceEquippedGunWith(Weapon pReplacementWeapon)
    {
        Vector3 dropPosition = pReplacementWeapon.transform.position;
        
        pReplacementWeapon.Equip();
        
        if (currentlyEquippedWeapon != null)
        {
            currentlyEquippedWeapon.Unequip(dropPosition);
        }
        
        currentlyEquippedWeapon = pReplacementWeapon;
    }
    
    public bool PlayerIsDead()
    {
        return player.GetComponent<PlayerKillable>().IsKilled();
    }
    
    private void Awake()
    {
        instance = this;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
