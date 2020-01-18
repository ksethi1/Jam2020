using UnityEngine;


public class WeaponController : MonoBehaviour
{ 
    [SerializeField] Transform gunPivot;
    [SerializeField] Transform gunHolder;
    [SerializeField] IElementalWeapon weapon;

    [SerializeField] WeaponsEventController weaponEvent;
    [SerializeField] GunPointer pointer;

    Elemental elemental;

    IWeaponInput input;
    SpriteRenderer sprite;

    Vector3 aimDirection;
    float aimAngle;
    

    private void Start()
    {
        input = GetComponent<IWeaponInput>();
        elemental = GetComponent<Elemental>();
    }

    private void Update()
    {
        input.SetWeaponInputs();
        Aim();
        if(input.Shooting)
            PullTrigger();
    }

    void Equip(IElementalWeapon weapon)
    {
        this.weapon = weapon;
        weapon.Equip();
        
        if(elemental)
            weapon.SwitchElement(elemental.Element);

        pointer.Deactivate();
        gunHolder.localScale = new Vector2(1, 1);
        weapon.gunTransform.parent = gunHolder;
        weapon.gunTransform.localPosition = Vector2.zero;
        weapon.gunTransform.localRotation= Quaternion.identity;
        sprite = weapon.gunTransform.GetComponent<SpriteRenderer>();
    }


    void Aim()
    {
        aimDirection.x= input.MouseXDirection;
        aimDirection.y= input.MouseYDirection;
        aimAngle = Vector2.SignedAngle(Vector2.right,aimDirection);
        if (sprite != null)
        {
            if (aimDirection.x < 0)
                sprite.flipY = true;
            else
                sprite.flipY = false;
        }
        else
        if (aimDirection.x <0)
            gunHolder.localScale = new Vector2(1,-1);
        else
            gunHolder.localScale = new Vector2(1,1);
        gunPivot.rotation =Quaternion.Euler(0,0,aimAngle);
    }

    void PullTrigger()
    {
        if (weapon == null)
            return;

        weapon.Shoot();
        if (weapon.IsEmpty)
        {
            weapon.Dequip();
            weapon = null;
        }
        
        weaponEvent.OnWeaponShot();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.weapon != null)
            return;
        IElementalWeapon weapon=collision.GetComponent<IElementalWeapon>();
        if (weapon!=null)
        {
            Equip(weapon);
        }
    }

    public void SwitchElement(Elements element)
    {
        weapon?.SwitchElement(element);
    }
    

}
