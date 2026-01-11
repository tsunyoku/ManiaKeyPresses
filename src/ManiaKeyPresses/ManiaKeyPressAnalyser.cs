using osu.Game.Rulesets.Mania.Replays;
using osu.Game.Scoring;
using osu.Game.Utils;

namespace ManiaKeyPresses;

public class ManiaKeyPressAnalyser
{
    private readonly Score _score;

    public ManiaKeyPressAnalyser(string osrFile, string osuClientId, string osuClientSecret, string? beatmapCacheDirectory = null)
    {
        var oauthStore = new OAuthStore(osuClientId, osuClientSecret);
        var beatmapStore = new BeatmapStore(oauthStore, beatmapCacheDirectory);
        var scoreDecoder = new LocalLegacyScoreDecoder(beatmapStore);
        
        using var stream = File.OpenRead(osrFile);
        _score = scoreDecoder.Parse(stream);
    }

    public KeyPressAnalysis AnalyseReplay()
    {
        const int keyCount = 18;

        var keyStates = Enumerable.Range(0, keyCount)
            .Select(_ => new KeyState())
            .ToArray();

        double previousTime = 0;
        var frameIndex = 0;

        foreach (var replayFrame in _score.Replay.Frames.Cast<ManiaReplayFrame>())
        {
            var activeKeys = replayFrame.Actions
                .Select(x => (int)x)
                .Order()
                .ToArray();

            var timeDelta = frameIndex == 0 ? 0 : (int)(replayFrame.Time - previousTime);

            for (var key = 0; key < keyCount; key++)
            {
                var state = keyStates[key];
                var isPressed = activeKeys.Contains(key);
            
                switch (state.IsHeld, isPressed)
                {
                    // Key was held, is still held
                    case (true, true):
                        state.CurrentHoldTime += timeDelta;
                        break;
                    
                    // Key is being released
                    case (true, false):
                        state.HoldTimes.Add(state.CurrentHoldTime + timeDelta);
                        state.CurrentHoldTime = 0;
                        state.IsHeld = false;
                        break;
                    
                    // Key is being pressed
                    case (false, true):
                        state.IsHeld = true;
                        state.CurrentHoldTime = 0;
                        break;
                    
                    // Key was and is unheld
                    case (false, false):
                        break;
                }
            }

            previousTime = replayFrame.Time;
            frameIndex++;
        }

        var rateMultiplier = ModUtils.CalculateRateWithMods(_score.ScoreInfo.Mods);

        var histogram = keyStates
            .Where(state => state.HoldTimes.Any())
            .Select(state => CreateHistogram(state.HoldTimes, rateMultiplier))
            .ToArray();

        return new KeyPressAnalysis(
            histogram.Select(x => x.Times).ToArray(),
            histogram.Select(x => x.Counts).ToArray(),
            _score);
    }
    
    private static (int[] Times, int[] Counts) CreateHistogram(List<int> durations, double rateMultiplier)
    {
        var maxDuration = durations.Max();
        var counts = new int[maxDuration + 1];
    
        foreach (var duration in durations.Where(d => d >= 0))
        {
            counts[duration]++;
        }
    
        var times = Enumerable.Range(0, maxDuration + 1)
            .Select(t => (int)(t / rateMultiplier))
            .ToArray();
    
        return (times, counts);
    }
}

file class KeyState
{
    public bool IsHeld { get; set; }
    public int CurrentHoldTime { get; set; }
    public List<int> HoldTimes { get; } = [];
}

public record KeyPressAnalysis(int[][] HoldTimes, int[][] HoldTimeCounts, Score Score);