using System;
using System.Collections.Generic;

namespace BaseballGameCreator
{
    /// <summary>
    /// Pure C# baseball pitch / at-bat simulation engine.
    /// Drives realistic per-zone outcome probabilities calibrated against
    /// MLB league-average rates (BA ~.250, BB% ~8.5%, K% ~22%, HR/H ~12%).
    ///
    /// Light-realism design:
    ///   * Strike zone modeled as a 3x3 grid (9 zones)
    ///   * A 10th "Ball" zone models a pitch thrown outside the zone
    ///     (true ball OR chase pitch the hitter goes after)
    ///   * Per-pitch outcome probabilities are zone-conditioned
    ///   * Hit-type mix (1B/2B/3B/HR) is zone-conditioned
    ///     (center cuts produce more HR/2B, low pitches more grounders, etc.)
    ///   * Count-aware adjustments shift the distribution:
    ///       - 2-strike counts: more whiffs / fouls, fewer hits
    ///       - 3-ball counts:   more contact / hits, fewer whiffs
    /// </summary>
    public enum StrikeZone
    {
        HighLeft,  HighMid,  HighRight,
        MidLeft,   MidMid,   MidRight,
        LowLeft,   LowMid,   LowRight,
        Ball       // pitch off the plate
    }

    public enum PitchOutcome
    {
        Hit,
        GroundOut,
        FlyOut,
        Foul,
        SwingingStrike,
        CalledStrike,
        Ball
    }

    public enum HitType
    {
        Single,
        Double,
        Triple,
        HomeRun
    }

    public static class BaseballSimulation
    {
        // Single shared RNG.  Creating `new Random()` per call (as the old
        // algorithm did) seeds from the system clock and on fast successive
        // calls can yield identical values -> very low-entropy simulation.
        private static readonly Random Rng = new Random();

        // --- Per-zone outcome distribution (sums to 1.0) ----------------------
        // Order:                Hit    GO     FO     Foul   SwStr  CStr   Ball
        private static readonly Dictionary<StrikeZone, double[]> Outcomes =
            new Dictionary<StrikeZone, double[]>
        {
            // Corner-high pitches: lots of called strikes, some whiffs, occasional power
            { StrikeZone.HighLeft,   new[] { 0.08, 0.05, 0.12, 0.18, 0.20, 0.35, 0.02 } },
            // High middle: classic fly-ball / power zone
            { StrikeZone.HighMid,    new[] { 0.16, 0.04, 0.20, 0.20, 0.13, 0.25, 0.02 } },
            { StrikeZone.HighRight,  new[] { 0.08, 0.05, 0.12, 0.18, 0.20, 0.35, 0.02 } },

            // Middle-left / right edges: drive zones
            { StrikeZone.MidLeft,    new[] { 0.13, 0.09, 0.10, 0.17, 0.15, 0.34, 0.02 } },
            // Heart of plate ("meatball"): meat of the swing - lots of hits, fewest whiffs
            { StrikeZone.MidMid,     new[] { 0.26, 0.10, 0.13, 0.18, 0.08, 0.23, 0.02 } },
            { StrikeZone.MidRight,   new[] { 0.13, 0.09, 0.10, 0.17, 0.15, 0.34, 0.02 } },

            // Low pitches: ground-out heavy, fewer fly outs
            { StrikeZone.LowLeft,    new[] { 0.08, 0.13, 0.04, 0.16, 0.22, 0.35, 0.02 } },
            { StrikeZone.LowMid,     new[] { 0.13, 0.20, 0.05, 0.17, 0.16, 0.27, 0.02 } },
            { StrikeZone.LowRight,   new[] { 0.08, 0.13, 0.04, 0.16, 0.22, 0.35, 0.02 } },

            // Off-the-plate pitch.  Most often a ball, sometimes a chase swing.
            { StrikeZone.Ball,       new[] { 0.04, 0.04, 0.03, 0.08, 0.15, 0.02, 0.64 } }
        };

        // --- Per-zone hit-type mix (sums to 1.0).  Order: 1B, 2B, 3B, HR -----
        private static readonly Dictionary<StrikeZone, double[]> HitMix =
            new Dictionary<StrikeZone, double[]>
        {
            { StrikeZone.HighLeft,   new[] { 0.60, 0.20, 0.02, 0.18 } },
            { StrikeZone.HighMid,    new[] { 0.50, 0.20, 0.02, 0.28 } }, // big power
            { StrikeZone.HighRight,  new[] { 0.60, 0.20, 0.02, 0.18 } },

            { StrikeZone.MidLeft,    new[] { 0.65, 0.22, 0.03, 0.10 } },
            { StrikeZone.MidMid,     new[] { 0.55, 0.22, 0.03, 0.20 } },
            { StrikeZone.MidRight,   new[] { 0.65, 0.22, 0.03, 0.10 } },

            { StrikeZone.LowLeft,    new[] { 0.72, 0.20, 0.05, 0.03 } }, // grounders
            { StrikeZone.LowMid,     new[] { 0.75, 0.18, 0.05, 0.02 } },
            { StrikeZone.LowRight,   new[] { 0.72, 0.20, 0.05, 0.03 } },

            { StrikeZone.Ball,       new[] { 0.78, 0.18, 0.03, 0.01 } }  // chase contact = weak
        };

        /// <summary>
        /// Resolve a single pitch given location and current count.
        /// </summary>
        public static PitchOutcome ResolvePitch(StrikeZone zone, int balls, int strikes)
        {
            double[] adjusted = ApplyCountModifier(Outcomes[zone], balls, strikes, zone);
            int idx = WeightedPick(adjusted);
            return (PitchOutcome)idx;
        }

        /// <summary>
        /// Resolve hit type for a batted ball, biased by location.
        /// </summary>
        public static HitType ResolveHitType(StrikeZone zone)
        {
            int idx = WeightedPick(HitMix[zone]);
            return (HitType)idx;
        }

        // --- Internals --------------------------------------------------------

        /// <summary>
        /// Light-realism count adjustment:
        ///   * 2-strike count   -> hitter defensive: +foul, +whiff, -hit
        ///   * 3-ball count     -> hitter aggressive on strikes: +hit, +foul, -whiff
        ///   * 3-2 full count   -> small bias toward foul (hitter protects)
        /// Adjustment is gentle (multiplicative tweaks then re-normalized).
        /// </summary>
        private static double[] ApplyCountModifier(double[] basep, int balls, int strikes, StrikeZone zone)
        {
            double[] p = (double[])basep.Clone();
            // Indices: 0=Hit 1=GO 2=FO 3=Foul 4=SwStr 5=CStr 6=Ball

            bool inZone = zone != StrikeZone.Ball;

            if (strikes >= 2)
            {
                p[0] *= 0.80; // fewer hits
                p[1] *= 0.95;
                p[2] *= 0.95;
                p[3] *= 1.25; // more fouls (protecting)
                p[4] *= 1.20; // more whiffs (chasing)
                p[5] *= 1.05;
            }
            if (balls >= 3)
            {
                p[0] *= 1.20; // more hits when ahead in count
                p[3] *= 1.10;
                p[4] *= 0.75; // fewer whiffs
                if (!inZone)
                {
                    // hitter takes the borderline ball if 3-0
                    if (strikes == 0)
                    {
                        p[6] *= 1.15;
                        p[0] *= 0.5;
                        p[4] *= 0.5;
                    }
                }
            }
            if (balls == 3 && strikes == 2)
            {
                p[3] *= 1.15; // full count -> lots of fouls
            }

            Normalize(p);
            return p;
        }

        private static int WeightedPick(double[] weights)
        {
            double r = Rng.NextDouble();
            double acc = 0.0;
            for (int i = 0; i < weights.Length; i++)
            {
                acc += weights[i];
                if (r <= acc) return i;
            }
            return weights.Length - 1;
        }

        private static void Normalize(double[] p)
        {
            double sum = 0.0;
            for (int i = 0; i < p.Length; i++) sum += p[i];
            if (sum <= 0.0) return;
            for (int i = 0; i < p.Length; i++) p[i] /= sum;
        }

        /// <summary>
        /// Map the user-facing hit type to its human-readable form.
        /// </summary>
        public static string HitTypeToString(HitType t)
        {
            switch (t)
            {
                case HitType.Single:  return "single.";
                case HitType.Double:  return "double.";
                case HitType.Triple:  return "triple.";
                case HitType.HomeRun: return "homerun.";
            }
            return "hit.";
        }
    }
}
