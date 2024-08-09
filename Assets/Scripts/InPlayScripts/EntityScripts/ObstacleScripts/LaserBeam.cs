using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public Animator laserBeam;
    public SpriteRenderer laserRady;

    float chargeTime;
    float fireTime;
    bool canAttack;
    int range;
    public int damage = 4;

    public void Init(float _chargeTime, float _fireTime, int _range, bool isFront)
    {
        chargeTime = _chargeTime;
        fireTime = _fireTime;
        range = _range;

        if (!isFront)
            transform.localEulerAngles = new Vector3(0, 0, 90);
    }

}
