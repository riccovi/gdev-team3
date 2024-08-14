using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    private SpriteRenderer playerRenderer;

    private void Start()
    {
        Transform spriteTransform = transform.Find("PlayerGraphics/Sprite");
        playerRenderer = spriteTransform.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Nails"){
            playerRenderer.color = Color.red;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
