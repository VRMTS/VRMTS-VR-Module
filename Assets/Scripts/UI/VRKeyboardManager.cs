using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

// ==================================================
// File: VRKeyboardManager.cs
// Purpose: Forces the native Meta Quest OS keyboard to open 
// when a VR pointer clicks a TMP Input Field.
// ==================================================[RequireComponent(typeof(TMP_InputField))]
public class VRKeyboardManager : MonoBehaviour, IPointerClickHandler, IDeselectHandler
{
    private TMP_InputField inputField;
    private TouchScreenKeyboard keyboard;

    void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    // Triggers when the VR Ray hits and clicks the Input Field
    public void OnPointerClick(PointerEventData eventData)
    {
        // Open the native Quest keyboard
        if (keyboard == null || keyboard.status != TouchScreenKeyboard.Status.Visible)
        {
            // Parameters: text, keyboardType, autocorrect, multiline, secure (password), alert, textPlaceholder
            bool isPassword = inputField.contentType == TMP_InputField.ContentType.Password;
            keyboard = TouchScreenKeyboard.Open(inputField.text, TouchScreenKeyboardType.Default, false, false, isPassword);
        }
    }

    void Update()
    {
        // Continuously sync the text from the Quest keyboard to your TMP field
        if (keyboard != null && keyboard.status == TouchScreenKeyboard.Status.Visible)
        {
            inputField.text = keyboard.text;
        }
    }

    // Triggers when you click anywhere else on the Canvas
    public void OnDeselect(BaseEventData eventData)
    {
        if (keyboard != null && keyboard.active)
        {
            keyboard.active = false; // Hide the keyboard
            keyboard = null;
        }
    }
}