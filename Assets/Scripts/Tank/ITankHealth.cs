using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITankHealth
{
    // Taking damage
    void ITakeDamage(float damage);

    // Gaining health
    void IGainHealth(float healthAmount);


}
