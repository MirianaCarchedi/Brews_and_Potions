using UnityEngine;
using UnityEngine.UI;
public class MoveObjectBetweenSlots : MonoBehaviour

{
    [Header("Canvas di origine e destinazione")]
    public Canvas sourceCanvas;
    public Canvas targetCanvas;

    [Header("Slot di origine e destinazione")]
    public Transform sourceSlot;
    public Transform targetSlot;

    public void MoveUIElement()
    {
        if (sourceSlot.childCount == 0)
        {
            Debug.LogWarning("Nessun oggetto nello slot di origine.");
            return;
        }

        if (targetSlot.childCount > 0)
        {
            Debug.LogWarning("Lo slot di destinazione è già occupato.");
            return;
        }

        Transform objToMove = sourceSlot.GetChild(0);

        // ✅ Sposta l'oggetto nel nuovo slot, mantenendo layout corretto
        objToMove.SetParent(targetSlot, false);

        // ✅ Aggiorna il DragDrop (se presente)
        DragDrop dragScript = objToMove.GetComponent<DragDrop>();
        if (dragScript != null)
        {
            dragScript.OriginalParent = targetSlot;
            dragScript.canvas = targetCanvas; // già reso pubblico prima
        }
    }
}
