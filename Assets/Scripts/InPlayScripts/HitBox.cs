using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    Entity linkedDamageableObject;

    private void Start()
    {
        FindLinkedDamageableObject();
    }

    public void FindLinkedDamageableObject()
    {
        Transform currentTransform = transform;

        while (currentTransform != null)
        {
            if (currentTransform.TryGetComponent(out Entity _damageableObject))
            {
                linkedDamageableObject = _damageableObject;
                return;
            }
            currentTransform = currentTransform.parent;
        }

        Debug.LogWarning("�θ� DamageableObject�� ���� HitBox" + gameObject.name);
    }

    public void GetDamage(int damage)
    {
        linkedDamageableObject.GetDamage(damage);
    }
}
