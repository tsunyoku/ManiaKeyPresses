using ManiaKeyPresses.Analysers;
using osu.Game.Scoring;

namespace ManiaKeyPresses.Extensions;

public static class ScoreExtensions
{
    public static KeyPressAnalysis AnalyseKeyPresses(this Score score)
    {
        KeyPressAnalyser analyser = score.ScoreInfo.RulesetID switch
        {
            3 => new ManiaKeyPressAnalyser(score),
            1 => new TaikoKeyPressAnalyser(score),
            0 => new OsuKeyPressAnalyser(score),
            _ => throw new InvalidOperationException("Unsupported ruleset")
        };

        return analyser.AnalyseReplay();
    }
}