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
    public GameObject label;
    private WeatherInfo currentWeather;

    // Start is called before the first frame update
    void Start()
    {
        currentWeather = GetWeather();
        WriteName();
        CheckSnowStatus();
        checkDayTimeStatus();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void WriteName()
    {
        TextMesh textObject = label.GetComponent<TextMesh>();
        textObject.text = currentWeather.name;
        label.SetActive(true);
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
        Debug.Log(String.Format("resp is {0}", jsonResponse));
        WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(jsonResponse);
        var output = JsonUtility.ToJson(info, true);
        Debug.Log(output);
        return info;
    }

    private bool isDayTime()
    {
        long sunRise = currentWeather.sys.sunrise;
        long sunSet = currentWeather.sys.sunset;
        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()/1000;
        Debug.Log(String.Format("Got sunRise {0}", sunRise));
        Debug.Log(String.Format("Got sunSet {0}", sunSet));
        Debug.Log(String.Format("Got currentTime {0}", currentTime));

        return currentTime<sunSet&&currentTime>sunRise;
    }


}

[Serializable]
public class SysInfo
{
    private int id;
    public string country;
    public long sunrise;
    public long sunset;
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
    public string name;
    public List<Weather> weather;
    public SysInfo sys;
}
