using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Pokemon
{
    public string name;
    public string url;

    public Pokemon(string name, string url)
    {
        this.name = name;
        this.url = url;
    }
}

public class PokeAPI : MonoBehaviour
{

    [SerializeField]
    RawImage pokeImage;
    [SerializeField]
    TextMeshProUGUI pokeName, pokeNum;
    [SerializeField]
    TextMeshProUGUI[] typeNames;
    [SerializeField]
    Button randomButton;
    [SerializeField]
    TMP_Dropdown pokemonSelector;

    Dictionary<int, Pokemon> pokedex = new Dictionary<int, Pokemon>();




    private readonly string baseURL = "https://pokeapi.co/api/v2/";

    private void Start()
    {
        randomButton.onClick.AddListener(() =>
        {
            ButtonPressed();
        });
        StartCoroutine("InitalRequest");
        ClearData();

    }
    /// <summary>
    /// This method is used to fill the dropdown with all the pokemon from the original 151
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitalRequest()
    {
        // Looks to get 151 pokemon from API
        UnityWebRequest pokedexGet = UnityWebRequest.Get($"{baseURL}pokemon?limit=151");

        yield return pokedexGet.SendWebRequest();
        //Checks to make sure that the web request connects if not logs out the error
        if (pokedexGet.result == UnityWebRequest.Result.ConnectionError || pokedexGet.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(pokedexGet.error);
            yield break;
        }

        JSONNode pokemonInfo = JSON.Parse(pokedexGet.downloadHandler.text);
        List<string> pokemon = new List<string>();

        for (int i = 0; i < pokemonInfo["results"].Count; i++)
        {
            pokemon.Add(CapitalLetter(pokemonInfo["results"][i]["name"].Value));
            pokedex.Add(i + 1, new Pokemon(pokemonInfo["results"][i]["name"].Value, pokemonInfo["results"][i]["url"].Value));
            
        }
        pokemonSelector.AddOptions(pokemon);
        pokemonSelector.onValueChanged.AddListener(ButtonPressed);

    }
    /// <summary>
    /// Is triggered by either the drop down or the random button, will check the pokeNum if it is 0 it will instead get a random number and send it off to GetPokemonData
    /// </summary>
    /// <param name="pokeNum"></param>
    private void ButtonPressed(int pokeNum = 0)
    {
        pokeNum = pokeNum == 0 ? Random.Range(1, 151) : pokeNum;
        pokemonSelector.SetValueWithoutNotify(pokeNum);
        StartCoroutine(GetPokemonData(pokeNum));
    }
    /// <summary>
    /// Used to get individual data for a pokemon based on the number passed to it, and then fills in all the 
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private IEnumerator GetPokemonData(int num)
    {

        ClearData();
        //Request to get a Pokemons data from the API
        UnityWebRequest pokedexGet = UnityWebRequest.Get($"{baseURL}pokemon/{num}");

        yield return pokedexGet.SendWebRequest();

        //Checks to make sure that the web request connects if not logs out the error
        if (pokedexGet.result == UnityWebRequest.Result.ConnectionError || pokedexGet.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(pokedexGet.error);
            yield break;
        }
        //Parse the data from the web request using Simple JSON
        JSONNode pokemonInfo = JSON.Parse(pokedexGet.downloadHandler.text);

        StartCoroutine(GetSprite(pokemonInfo));
        GetTypes(pokemonInfo);
   

        string name = pokemonInfo["name"];
        string dexNum = $"#{num.ToString()}";


        pokeName.text = CapitalLetter(name);
        pokeNum.text = dexNum;

    }

    private void GetTypes(JSONNode pokemonInfo)
    {
        JSONNode pokeTypes = pokemonInfo["types"];
        string[] types = new string[pokeTypes.Count];
        for (int i = 0, j = pokeTypes.Count - 1; i < pokeTypes.Count; i++, j--)
        {
            types[j] = pokeTypes[i]["type"]["name"];
        }
        for (int i = 0; i < types.Length; i++)
        {
            typeNames[i].text = CapitalLetter(types[i]);
        }
    }

    /// <summary>
    /// Clears any data in the fields so that no leftover data from the last pokemon persists
    /// </summary>
    private void ClearData()
    {
        pokeName.text = "";
        pokeNum.text = "";
        typeNames[0].text = "";
        typeNames[1].text = "";
        pokeImage.texture = Texture2D.blackTexture;
    }

    /// <summary>
    /// Web request to get the sprite and display it
    /// </summary>
    /// <param name="pokemonInfo"></param>
    private IEnumerator GetSprite(JSONNode pokemonInfo)
    {
        UnityWebRequest spriteRequest = UnityWebRequestTexture.GetTexture(pokemonInfo["sprites"]["front_default"]);
        yield return spriteRequest.SendWebRequest();

        if (spriteRequest.result == UnityWebRequest.Result.ConnectionError || spriteRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(spriteRequest.error);
            yield break;
        }

        pokeImage.texture = DownloadHandlerTexture.GetContent(spriteRequest);
        pokeImage.texture.filterMode = FilterMode.Point;
    }

    /// <summary>
    /// Capitialises the first letter of the string passed to it
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string CapitalLetter(string str)
    {
        return char.ToUpper(str[0]) + str.Substring(1); 
    }
}
