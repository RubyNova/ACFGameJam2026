using System;
using System.Collections.Generic;

namespace Saveables 
{
    [Serializable]
    public class LevelScore
    {
        public int Level;
        public int TimeRemaining;
        public int ItemsCrafted;
        public int ItemsDiscovered;
        public int CustomerHappinessPercentage;
        public float TotalScore;
    }

    [Serializable]
    public class RunData
    {
        public long StartDateTime;
        public List<LevelScore> levelScores;
    }
}