using UnityEngine;

public class WeaponVisual : MonoBehaviour
{
    public WeaponSystem weaponSystem;
    public SpriteRenderer weaponRenderer;

    public Sprite pistolSprite;
    public Sprite glockSprite;
    public Sprite mp5Sprite;
    public Sprite ak47Sprite;
    public Sprite shotgunSprite;
    public Sprite m249Sprite;

    int lastWeaponIndex = -1;

    void Start()
    {
        if (weaponSystem == null)
            weaponSystem = GetComponentInParent<WeaponSystem>();
        UpdateWeaponSprite();
    }

    void Update()
    {
        if (weaponSystem.activeWeaponIndex != lastWeaponIndex)
        {
            lastWeaponIndex = weaponSystem.activeWeaponIndex;
            UpdateWeaponSprite();
        }
    }

    void UpdateWeaponSprite()
    {
        if (weaponRenderer == null) return;

        switch (weaponSystem.activeWeaponIndex)
        {
            case 0: weaponRenderer.sprite = pistolSprite; break;
            case 1: weaponRenderer.sprite = glockSprite; break;
            case 2: weaponRenderer.sprite = mp5Sprite; break;
            case 3: weaponRenderer.sprite = ak47Sprite; break;
            case 4: weaponRenderer.sprite = shotgunSprite; break;
            case 5: weaponRenderer.sprite = m249Sprite; break;
        }
    }
}