using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;

public class Pokemon
{
    string name;
    string url;
}

public class PokeAPI : MonoBehaviour
{

    [SerializeField]
    RawImage pokeImage;
    [SerializeField]
    TextMeshProUGUI pokeName, pokeNum;
    [SerializeField]
    Button randomButton;
    [SerializeField]
    TMP_Dropdown pokemonSelector;




    private readonly string baseURL = "https://pokeapi.co/api/v2/";

    private void Start()
    {
        randomButton.onClick.AddListener(()=> {
            ButtonPressed();
        });
        StartCoroutine("InitalRequest");
        

    }

    private IEnumerator InitalRequest()
    {
        UnityWebRequest pokedex = UnityWebRequest.Get($"{baseURL}pokemon?limit=1000");

        yield return pokedex.SendWebRequest();

        if(pokedex.result == UnityWebRequest.Result.ConnectionError || pokedex.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(pokedex.error);
            yield break;
        }
        JSONNode pokemonInfo = JSON.Parse(pokedex.downloadHandler.text);
         //pokemon = JsonUtility.FromJson<Pokemon[]>(pokedex.downloadHandler.text);
        //Debug.Log(pokemon.Length);
    }

    private void ButtonPressed(int pokeNum = 0)
    {
        //Unity Web Request call to get the required information from the api.

    }
}
