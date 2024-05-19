using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    DamageableObject linkedDamageableObject;

    private void Start()
    {
        FindLinkedDamageableObject();
    }

    public void FindLinkedDamageableObject()
    {
        Transform currentTransform = transform;

        while (currentTransform != null)
        {
            if (currentTransform.TryGetComponent(out DamageableObject _damageableObject))
            {
                linkedDamageableObject = _damageableObject;
                return;
            }
            currentTransform = currentTransform.parent;
        }

        Debug.LogWarning("부모에 DamageableObject가 없는 HitBox" + gameObject.name);
    }

    public void GetDamage(int damage)
    {
        linkedDamageableObject.GetDamage(damage);
    }
}
