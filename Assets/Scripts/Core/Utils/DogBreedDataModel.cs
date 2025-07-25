using System;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Utils
{
    [Serializable]
    public class DogBreedDataModel
    {
        [SerializeField] private BreedData data;
        public BreedData Data => data;


        public static DogBreedDataModel FromJson(string json)
        {
            return JsonUtility.FromJson<DogBreedDataModel>(json);
        }
    }

    public class DogBreedDataListModel
    {
        [SerializeField] private List<BreedData> data;
        public List<BreedData> Data => data;


        public static DogBreedDataListModel FromJson(string json)
        {
            return JsonUtility.FromJson<DogBreedDataListModel>(json);
        }
    }

    [Serializable]
    public class BreedData
    {
        [SerializeField] private string id;
        [SerializeField] private string type;
        [SerializeField] private BreedAttributes attributes;

        public string Id => id;
        public string Type => type;
        public BreedAttributes Attributes => attributes;
    }

    [Serializable]
    public class BreedAttributes
    {
        [SerializeField] private string name;
        [SerializeField] private string description;
        [SerializeField] private bool hypoallergenic;
        [SerializeField] private LifeData life;
        [SerializeField] private WeightData male_weight;
        [SerializeField] private WeightData female_weight;

        public string Name => name;
        public string Description => description;
        public bool Hypoallergenic => hypoallergenic;
        public LifeData Life => life;
        public WeightData MaleWeight => male_weight;
        public WeightData FemaleWeight => female_weight;
    }

    [Serializable]
    public class LifeData
    {
        [SerializeField] private int min;
        [SerializeField] private int max;
        public int Min => min;
        public int Max => max;
    }

    [Serializable]
    public class WeightData
    {
        [SerializeField] private int min;
        [SerializeField] private int max;
        public int Min => min;
        public int Max => max;
    }
}