using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperFruit : MonoBehaviour
{
   public SpriteRenderer spriteRenderer;
   public ParticleSystem HealthPickupVFX;
   public float extraHits = 0;

    public AudioClip collectedClip;

    // The color to change to when entering the trigger
    public Color targetColor = Color.yellow;
    private void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        // Check if the entering collider has a tag you are interested in
        if (controller != null)
        {
            controller.PlaySound(collectedClip);
            spriteRenderer.color = targetColor;
            if(controller.health  < controller.maxHealth)
            {

                controller.ChangeHealth(controller.maxHealth);
                Destroy(gameObject);
                spriteRenderer.color = targetColor;
                extraHits++;

                Instantiate(HealthPickupVFX, transform.position, Quaternion.identity);

                controller.PlaySound(collectedClip);
            }
        }
    }
    public void OnPlayerHit()
    {
        if (extraHits>0)
        {
            extraHits--;
            spriteRenderer.color = Color.white;
        }
    }
    
}
