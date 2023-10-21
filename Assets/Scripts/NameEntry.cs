using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameEntry : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button submitButton;


    private void Awake()
    {
        nameInputField.onValueChanged.AddListener(_ => ActivateButton());
        submitButton.onClick.AddListener(SubmitName);
    }

    private void OnDestroy()
    {
        nameInputField.onValueChanged.RemoveAllListeners();
        submitButton.onClick.RemoveAllListeners();
    }

    
    
    
    private void SubmitName()
    {
        FusionConnection.instance.ConnectToRunner(nameInputField.text);
        canvas.SetActive(false);
    }

    private void ActivateButton()
    {
        submitButton.interactable = true;
    }
}
