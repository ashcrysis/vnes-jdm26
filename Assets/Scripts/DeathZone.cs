using System;
using Player;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class DeathZone : MonoBehaviour
{
    [SerializeField]
    private Sprite open;

    [SerializeField]
    private Sprite closed;

    private bool bueiroIsOpen;
    private Coroutine coroutine;
    private SpriteRenderer spriteRenderer;
    

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (bueiroIsOpen)
            {
                player.Die();
                bueiroIsOpen = false;
                StopCoroutine(coroutine);
                UpdateSprite();

            } else
            {
                bueiroIsOpen = true;
                coroutine = StartCoroutine(Wait());
                UpdateSprite();
            }

        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(30f);
        bueiroIsOpen = false;
    }

    private void UpdateSprite()
    {
        spriteRenderer.sprite = bueiroIsOpen? open:closed;
    }
}
