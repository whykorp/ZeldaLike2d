using UnityEngine;
using UnityEngine.UI;

public class PickUpItem : MonoBehaviour
{
    private bool isInRange;
    public InteractUI interactUI;
    public Item item;
    // public AudioClip soundToPlay;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            TakeItem();
        }
    }

    void TakeItem()
    {
        Inventory.instance.content.Add(item);
        StartCoroutine(Inventory.instance.UpdateInventoryUI());
        // AudioManager.instance.PlayClipAt(soundToPlay, transform.position);
        interactUI.HideUiInteract();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("IN RANGE");
            interactUI.ShowUiInteract();
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("OUT RANGE");
            interactUI.HideUiInteract();
            isInRange = false;
        }
    }
}