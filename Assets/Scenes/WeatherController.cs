using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.IO;
//using Assets;L

public class WeatherController : MonoBehaviour
{
    private const string API_KEY = "4176e58953a958e5c778f1db8803f6e7";
    public string cityId;
    public GameObject snowSystem;
    public GameObject sunSystem;
    private WeatherInfo currentWeather;

    // Start is called before the first frame update
    void Start()
    {
        currentWeather = GetWeather();
        CheckSnowStatus();
        checkDayTimeStatus();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CheckSnowStatus()
    {
        Debug.Log(String.Format("Got weather for {0}, is {1}", cityId, currentWeather.weather[0].main));
        bool snowing = currentWeather.weather[0].main.Equals("Snow");
        if (snowing)
        {
            snowSystem.SetActive(true);
        }
        else
        {
            snowSystem.SetActive(false);
        }
    }
    private void checkDayTimeStatus()
    {
        if (isDayTime())
        {
            sunSystem.SetActive(true);
        }
        else
        {
            sunSystem.SetActive(false);
        }
    }

    private WeatherInfo GetWeather()
    {
        string url = String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}", cityId, API_KEY);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(jsonResponse);
        return info;
    }

    private bool isDayTime()
    {
        // do stuff
        return false;
    }


}

[Serializable]
public class SysInfo
{
    public int id;
    public string country;
    public int sunrise;
    public int sunset;

    public DateTime GetSunrise() {
        return new DateTime(sunrise);
    }

    public DateTime GetSunset() {
        return new DateTime(sunset);
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
    public SysInfo sys;
}
