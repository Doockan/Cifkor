using UnityEngine;

namespace Assets.Scripts.Core.Utils
{
    [System.Serializable]
    public class WeatherForecastDataModel
    {
        public string Temperature;
        public string IconUrl;
        public string ShortForecast;

        [System.Serializable]
        private class Root
        {
            public Properties properties;
        }

        [System.Serializable]
        private class Properties
        {
            public Period[] periods;
        }

        [System.Serializable]
        private class Period
        {
            public int temperature;
            public string icon;
            public string shortForecast;
        }

        public static WeatherForecastDataModel FromJson(string json)
        {
            var root = JsonUtility.FromJson<Root>(json);
            var firstPeriod = root?.properties?.periods != null && root.properties.periods.Length > 0
                ? root.properties.periods[0]
                : null;

            if (firstPeriod == null)
                return null;

            return new WeatherForecastDataModel
            {
                Temperature = firstPeriod.temperature + "F",
                IconUrl = firstPeriod.icon,
                ShortForecast = firstPeriod.shortForecast
            };
        }
    }
}