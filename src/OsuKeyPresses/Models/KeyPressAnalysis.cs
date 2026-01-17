using osu.Game.Rulesets.Scoring;
using OsuScore = osu.Game.Scoring.Score;

namespace OsuKeyPresses.Models;

public record KeyPressAnalysis(int[][] HoldTimes, int[][] HoldTimeCounts, OsuScore Score)
{
    private IReadOnlyDictionary<int, int>[] GetHoldTimeCountsByKey()
    {
        var result = new List<IReadOnlyDictionary<int, int>>();
    
        for (var keyIndex = 0; keyIndex < HoldTimeCounts.Length; keyIndex++)
        {
            var keyHoldTimes = new Dictionary<int, int>();
        
            for (var i = 0; i < HoldTimes[keyIndex].Length; i++)
            {
                var holdTime = HoldTimes[keyIndex][i];
                var count = HoldTimeCounts[keyIndex][i];
            
                if (count > 0)
                    keyHoldTimes[holdTime] = count;
            }
        
            result.Add(keyHoldTimes);
        }
    
        return result.ToArray();
    }

    public ScoreAnalysis Export(User? user)
    {
        var holdTimeCounts = GetHoldTimeCountsByKey();

        var holdTimes = new List<ExportableHoldTime[]>();

        foreach (var keyHoldTimes in holdTimeCounts)
        {
            var exportableHoldTimes = keyHoldTimes
                .Select(kvp => new ExportableHoldTime
                {
                    HoldTime = kvp.Key,
                    TotalCount = kvp.Value
                })
                .ToArray();
            
            holdTimes.Add(exportableHoldTimes);
        }
        
        return new ScoreAnalysis
        {
            HoldTimes = holdTimes.ToArray(),
            User = user is not null ? new ExportableUser
            {
                Id = user.Id,
                Username = user.Username,
                Country = user.Country.Name,
                GlobalRank = user.Statistics?.GlobalRank,
                Pp = user.Statistics?.Pp,
            } : null,
            Score = new ExportableScore
            {
                Id = Score.ScoreInfo.OnlineID > 0 ? Score.ScoreInfo.OnlineID : null,
                LegacyScoreId = Score.ScoreInfo.LegacyOnlineID > 0 ? Score.ScoreInfo.LegacyOnlineID : null,
                LegacyTotalScore = Score.ScoreInfo.LegacyTotalScore,
                Accuracy = Score.ScoreInfo.Accuracy,
                MaxCombo = Score.ScoreInfo.MaxCombo,
                Ruleset =  Score.ScoreInfo.Ruleset.ShortName,
                Statistics = new ExportableScoreStatistics
                {
                    PerfectCount = Score.ScoreInfo.RulesetID == 3 ? Score.ScoreInfo.Statistics.GetValueOrDefault(HitResult.Perfect) : null,
                    GreatCount = Score.ScoreInfo.Statistics.GetValueOrDefault(HitResult.Great),
                    GoodCount = Score.ScoreInfo.RulesetID == 3 ? Score.ScoreInfo.Statistics.GetValueOrDefault(HitResult.Good) : null,
                    OkCount = Score.ScoreInfo.Statistics.GetValueOrDefault(HitResult.Ok),
                    MehCount = Score.ScoreInfo.RulesetID != 1 ? Score.ScoreInfo.Statistics.GetValueOrDefault(HitResult.Meh) : null,
                    MissCount = Score.ScoreInfo.Statistics.GetValueOrDefault(HitResult.Miss),
                }
            }
        };
    }
}