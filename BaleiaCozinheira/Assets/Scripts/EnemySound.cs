using UnityEngine;

public class Enemy : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Toca o áudio "hit" associado ao prefab deste inimigo
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null && audioSource.clip != null)
            {
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            }

        }
    }
}