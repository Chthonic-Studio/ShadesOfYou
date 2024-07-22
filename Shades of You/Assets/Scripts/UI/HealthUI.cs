using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Image healthImage;

    // Update is called once per frame
    void Update()
    {
        // Get the player's health from the Player instance
        float playerHealth = Player.Instance.health / 100f; 

        // Convert the color to HSV
        Color.RGBToHSV(healthImage.color, out float H, out float S, out float V);

        // Decrease the V value based on the player's health
        V = playerHealth;

        // Convert the HSV back to RGB
        Color currentColor = Color.HSVToRGB(H, S, V);

        // Apply the current color to the image
        healthImage.color = currentColor;

        // Set the sprite of the Image to be the same as the current sprite of the SpriteRenderer
        healthImage.sprite = spriteRenderer.sprite;
    }
}
