using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mania;
using osu.Game.Scoring.Legacy;

namespace ManiaKeyPresses;

internal class LocalLegacyScoreDecoder(BeatmapStore beatmapStore) : LegacyScoreDecoder
{
    private static readonly ManiaRuleset Ruleset = new();

    protected override Ruleset GetRuleset(int rulesetId)
    {
        if (rulesetId != 3)
            throw new InvalidOperationException("Tried to decode non-mania replay");

        return Ruleset;
    }

    // TODO: move the beatmap store call out of here, this is weird
    protected override WorkingBeatmap GetBeatmap(string md5Hash)
    {
        var osuFile = beatmapStore.GetBeatmapOsuFile(md5Hash);

        using var stream = new MemoryStream(osuFile);

        return new SlimWorkingBeatmap(stream);
    }
}