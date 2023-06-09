using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUpSFX;
    [SerializeField] int PointsForCoinPickup = 100;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            FindObjectOfType<GameSession>().AddToScore(PointsForCoinPickup);
            AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
