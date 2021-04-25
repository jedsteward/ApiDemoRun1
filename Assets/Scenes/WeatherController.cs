using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.IO;
//using Assets;L

public class WeatherController : MonoBehaviour
{
    private const string API_KEY = "4176e58953a958e5c778f1db8803f6e7";
    public string CityId;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("here");
        WeatherInfo info = GetWeather();
        Debug.Log(info.weather[0].main);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private WeatherInfo GetWeather()
    {
        string url = String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}", CityId, API_KEY);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(jsonResponse);
        return info;
    }
}

[Serializable]
public class Weather
{
    public int id;
    public string main;
}
[Serializable]
public class WeatherInfo
{
    public int id;

    public List<Weather> weather;
}
