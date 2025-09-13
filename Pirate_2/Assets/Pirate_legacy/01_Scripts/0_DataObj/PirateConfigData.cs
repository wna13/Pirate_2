using System.Collections.Generic;
using Sept.Data;
using UnityEngine;

namespace PirateConfig.Data
{
    [CreateAssetMenu(fileName = nameof(PirateConfigData), menuName = "Data/" + nameof(PirateConfigData))]
    public class PirateConfigData : SingletonScriptableObject<PirateConfigData>
    {
        [Header("Player")]
        public float defaultMaxSpeed = 7f;
        public float cannonfirePower = 25f;
        public List<int> needEXP = new List<int>();
        public List<int> expRewardCountByLevel = new List<int>();
        public List<float> camFov = new List<float>();
        public List<float> colZsize = new List<float>();

    }

    public static class ShipData
    {
        private static PirateConfigData pirateConfigData => PirateConfigData.Instance;

        public static class DefaultStat
        {
            public static float DefaultMaxSpeed => pirateConfigData.defaultMaxSpeed;
            public static float CannonFirePower => pirateConfigData.cannonfirePower;
            public static List<int> NeedExp => pirateConfigData.needEXP;
            public static List<int> EXPRewardCountByLevel => pirateConfigData.expRewardCountByLevel;
            public static List<float> CamFov => pirateConfigData.camFov;
            public static List<float> ColZsize => pirateConfigData.colZsize;

        }

    }
}

