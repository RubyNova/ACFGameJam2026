using System;
using System.Collections.Generic;

namespace Saveables 
{
    public class LevelScore
    {
        public int Level { get; set; } = 0;
        public int TimeRemaining { get; set; } = 0;
        public int ItemsCrafted { get; set; } = 0;
        public int ItemsDiscovered { get; set; } = 0;
        public int CustomerHappinessPercentage { get; set; } = 0;
        public float TotalScore { get; set; } = 0;
    }

    public class RunData
    {
        public DateTime StartDateTime { get; set; } = DateTime.UtcNow;
        public List<LevelScore> levelScores { get; set; } = new();
    }
}