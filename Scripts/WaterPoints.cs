using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPoints : MonoBehaviour
{
    private Rigidbody2D body;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Waterpoint")) return;
        if (body == null) body = GetComponent<Rigidbody2D>();
        body.AddForce(Vector2.down * collision.GetComponent<Rigidbody2D>().velocity.y, ForceMode2D.Impulse);
    }
}
