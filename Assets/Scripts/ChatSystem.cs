
using Fusion;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChatSystem : NetworkBehaviour
{
    [Header("Objects")]
    public GameObject chatEntryCanvas;
    public TMP_InputField chatEntryInput;
    public GameObject chatDisplayCanvas;
    public TMP_Text chatBody;
    
    [Header("Action Reference")]
    public InputActionReference startChat;
    public InputActionReference sendChat;

    [Header("Networked")]
    private GameObject _placeholder;
    
    [Networked(OnChanged = nameof(LastPublicChatChanted))]
    public NetworkString<_256> LastPublicChat { get; set; }
    [Networked(OnChanged = nameof(LastPrivateChatChanted))]
    public NetworkString<_256> LastPrivateChat { get; set; }

    private static TMP_Text _myChatBody;
    
    private string _thisPlayerName;
    
    
    private void Start()
    {
        if (HasStateAuthority)
        {
            startChat.action.performed += StartChat;
            sendChat.action.performed += SendChat;
            chatDisplayCanvas.SetActive(true);
            _myChatBody = chatBody;
        }

        _thisPlayerName = transform.root.GetComponent<PlayerStats>().PlayerName.ToString();
    }

    private void StartChat(InputAction.CallbackContext obj)
    {
        chatEntryCanvas.SetActive(true);
        chatEntryInput.Select();
    }

    private void SendChat(InputAction.CallbackContext obj)
    {
        LastPublicChat = chatEntryInput.text;
        chatEntryCanvas.SetActive(false);
    }

    protected static void LastPublicChatChanted(Changed<ChatSystem> changed)
    {
        _myChatBody.text += "\n" 
                           + changed.Behaviour._thisPlayerName 
                           + " : " 
                           + changed.Behaviour.LastPublicChat;
    }
    
    protected static void LastPrivateChatChanted(Changed<ChatSystem> changed)
    {
        
    }
}
