using UnityEngine;

public class NPCInteraccion : MonoBehaviour
{
    [SerializeField] private GameObject npcButtonInter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            npcButtonInter.SetActive(true);
        }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            npcButtonInter.SetActive(false);
        }

    }
}
