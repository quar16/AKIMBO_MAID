using System.Collections;
using UnityEngine;

public class FlashEffect
{
    float flashDuration = 0.03f;
    MonoBehaviour behaviour;
    SpriteRenderer spriteRenderer;
    MaterialPropertyBlock propertyBlock;

    public void Init(MonoBehaviour _behaviour, SpriteRenderer _renderer)
    {
        behaviour = _behaviour;
        spriteRenderer = _renderer;
        propertyBlock = new MaterialPropertyBlock();
    }


    public void Flash()
    {
        if (behaviour != null)
            behaviour.StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.GetPropertyBlock(propertyBlock);

        propertyBlock.SetFloat("_Flash", 1);
        spriteRenderer.SetPropertyBlock(propertyBlock);

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_Flash", 0);
        spriteRenderer.SetPropertyBlock(propertyBlock);
    }
}
