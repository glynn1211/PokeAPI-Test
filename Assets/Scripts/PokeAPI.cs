using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PokeAPI : MonoBehaviour
{

    [SerializeField]
    RawImage pokeImage;
    [SerializeField]
    TextMeshProUGUI pokeName, pokeNum;
    [SerializeField]
    Button randomButton;


    private readonly string baseURL = "https://poekap.co/api/v2/";

    private void Start()
    {
        randomButton.onClick.AddListener(ButtonPressed);
    }

    private void ButtonPressed()
    {
        //JsonUtility.FromJson<>();

    }
}
