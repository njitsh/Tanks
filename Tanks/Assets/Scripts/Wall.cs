using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private int health = 100;
    public bool isWood;

    public void Hit(int damage)
    {
        if (health > damage && isWood) health -= damage;
        else if (isWood)
        {
            health = 0;
            Destroy(gameObject);
        }
    }
}
