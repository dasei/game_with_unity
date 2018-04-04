using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AttackBehavior : MonoBehaviour {

    public Waffe weapon;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SpawnWeapon(Vector2 direction)
    {
        if (weapon == null)
            return;


        float entityWidth = spriteRenderer.sprite.bounds.size.x;
        float entityHeight = spriteRenderer.sprite.bounds.size.y;

        float weaponX = direction.x * entityWidth / 2;
        float weaponY = direction.y * entityHeight / 2;

        //get rotation from directional vector
        float rotation = -(direction.x + direction.y) * Mathf.Rad2Deg * Mathf.Acos(direction.y);

        Instantiate(weapon, (new Vector3(weaponX, weaponY, 0)) + (transform.position), Quaternion.Euler(0, 0, rotation), transform);
    }
}
