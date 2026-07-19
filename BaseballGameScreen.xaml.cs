using System;
using System.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BaseballGameCreator
{
    /// <summary>
    /// Main game screen.  All pure simulation math lives in
    /// <see cref="BaseballSimulation"/>; this class is the UWP UI shell:
    /// it owns the game state (count, outs, score, runners) and translates
    /// button clicks into <c>BaseballSimulation.ResolvePitch(...)</c> calls.
    ///
    /// REFACTOR NOTES (2026):
    ///   * Strike zone expanded from 5 to 9 buttons (3x3 grid) plus a Ball button.
    ///   * Probability tables moved into BaseballSimulation (MLB-calibrated).
    ///   * Count-aware simulation (2-strike defense, 3-ball aggression).
    ///   * Removed ~1800 lines of duplicated away*/home* methods - replaced by
    ///     team-parameterized helpers (HitOccurred, GroundOut, Walk, ...).
    /// </summary>
    public sealed partial class BaseballGameScreen : Page
    {
        // Roster + bullpen + bench arrays loaded from SetRosters (unchanged API)
        public static ArrayList awayTeamBattersX = new ArrayList();
        public static ArrayList homeTeamBattersX = new ArrayList();
        public static ArrayList awayTeamPBEX = new ArrayList();
        public static ArrayList homeTeamPBEX = new ArrayList();

        // Game state
        public static int inn, awayBatterOrder, homeBatterOrder;
        public static int strikeCount, ballCount, outCount;
        public static int atScore, htScore;
        public static int fbCol, sbCol, tbCol;  // 0 = empty, 1 = runner
        public static int awayPitchesThrown, homePitchesThrown;
        public static double inn2;

        // UI element references (static for cross-page access by the engine)
        public static Windows.UI.Xaml.Shapes.Rectangle firstB, secondB, thirdB;
        public static Ellipse s1, s2, b1, b2, b3, o1, o2;
        public static TextBlock actionBarX, ATScoreTX, HTScoreTX, TBInd, InnTB;

        // Persistence
        public static Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        public static Windows.Storage.StorageFile gameLog;

        // Pitcher tracking
        public static string currentAwayPitcher, currentHomePitcher;

        // Final game log payload + team-name cache (consumed by GameLogScreen)
        public static string FINALGAMELOG;
        public static string ATNameXYZ, HTNameXYZ;

        // Team names from SetRosters
        public static string setRosters_getATName, setRosters_getHTName;

        public BaseballGameScreen()
        {
            this.InitializeComponent();
            gameStart();
        }

        // ----- accessor methods consumed by GameLogScreen --------------------
        public static string getFINALGAMELOG()        { return FINALGAMELOG; }
        public static string getAwayTeamNameForGL()   { return ATNameXYZ; }
        public static string getHomeTeamNameForGL()   { return HTNameXYZ; }

        // ---------------------------------------------------------------------
        public async void gameStart()
        {
            gameLog = await storageFolder.CreateFileAsync(
                "GameLog.txt",
                Windows.Storage.CreationCollisionOption.ReplaceExisting);

            awayTeamBattersX = SetRosters.getATBatters();
            homeTeamBattersX = SetRosters.getHTBatters();
            awayTeamPBEX     = SetRosters.getATPBE();
            homeTeamPBEX     = SetRosters.getHTPBE();

            setRosters_getATName = SetRosters.getATName();
            setRosters_getHTName = SetRosters.getHTName();

            // Team-name display (handle short names safely)
            string aShort = setRosters_getATName.Length < 3
                ? setRosters_getATName
                : setRosters_getATName.Substring(0, 3);
            string hShort = setRosters_getHTName.Length < 3
                ? setRosters_getHTName
                : setRosters_getHTName.Substring(0, 3);

            ATNameT.Text = aShort;  ATNameXYZ = aShort;  awayPlayerA.PlaceholderText = aShort;
            HTNameT.Text = hShort;  HTNameXYZ = hShort;  homePlayerA.PlaceholderText = hShort;

            for (int i = 1; i < awayTeamPBEX.Count; i++) awayPlayerA.Items.Add(awayTeamPBEX[i]);
            for (int j = 1; j < homeTeamPBEX.Count; j++) homePlayerA.Items.Add(homeTeamPBEX[j]);

            inn = 1; inn2 = inn;

            // Cache UI element references
            actionBarX = ActionBar; TBInd = topbotIndicator; InnTB = currentGameInning;
            firstB = firstBase; secondB = secondBase; thirdB = thirdBase;
            s1 = strike1; s2 = strike2;
            b1 = ball1; b2 = ball2; b3 = ball3;
            o1 = out1; o2 = out2;

            fbCol = 0; sbCol = 0; tbCol = 0;

            awayBatterOrder = 0; homeBatterOrder = 0;
            strikeCount = 0; ballCount = 0; outCount = 0;
            atScore = 0; htScore = 0;

            ATScoreTX = ATScoreT; HTScoreTX = HTScoreT;
            ATScoreTX.Text = atScore.ToString();
            HTScoreTX.Text = htScore.ToString();

            currentlyPText.Text  = " " + homeTeamPBEX[0].ToString();
            currentAwayPitcher   = " " + awayTeamPBEX[0].ToString();
            currentHomePitcher   = " " + homeTeamPBEX[0].ToString();
            atBatText.Text       = " " + awayTeamBattersX[GetABO()].ToString();
            onDeckText.Text      = " " + awayTeamBattersX[GetABO1()].ToString();
            inTheHoleText.Text   = " " + awayTeamBattersX[GetABO2()].ToString();
        }

        // ====================================================================
        // BASE / COUNT INDICATOR HELPERS
        // ====================================================================
        public static void clearShapes()
        {
            outCount = 0; strikeCount = 0; ballCount = 0;
            clearBases();
            s1.Fill = new SolidColorBrush(Colors.Black);
            s2.Fill = new SolidColorBrush(Colors.Black);
            b1.Fill = new SolidColorBrush(Colors.Black);
            b2.Fill = new SolidColorBrush(Colors.Black);
            b3.Fill = new SolidColorBrush(Colors.Black);
            o1.Fill = new SolidColorBrush(Colors.Black);
            o2.Fill = new SolidColorBrush(Colors.Black);
        }

        public static void clearBases()
        {
            firstB.Fill  = new SolidColorBrush(Colors.Black);
            secondB.Fill = new SolidColorBrush(Colors.Black);
            thirdB.Fill  = new SolidColorBrush(Colors.Black);
            fbCol = 0; sbCol = 0; tbCol = 0;
        }

        public static void clearStrikesBalls()
        {
            strikeCount = 0; ballCount = 0;
            s1.Fill = new SolidColorBrush(Colors.Black);
            s2.Fill = new SolidColorBrush(Colors.Black);
            b1.Fill = new SolidColorBrush(Colors.Black);
            b2.Fill = new SolidColorBrush(Colors.Black);
            b3.Fill = new SolidColorBrush(Colors.Black);
        }

        public static void Green_FirstBase()  { firstB.Fill  = new SolidColorBrush(Colors.Green); fbCol = 1; }
        public static void Green_SecondBase() { secondB.Fill = new SolidColorBrush(Colors.Green); sbCol = 1; }
        public static void Green_ThirdBase()  { thirdB.Fill  = new SolidColorBrush(Colors.Green); tbCol = 1; }
        public static void Black_FirstBase()  { firstB.Fill  = new SolidColorBrush(Colors.Black); fbCol = 0; }
        public static void Black_SecondBase() { secondB.Fill = new SolidColorBrush(Colors.Black); sbCol = 0; }
        public static void Black_ThirdBase()  { thirdB.Fill  = new SolidColorBrush(Colors.Black); tbCol = 0; }

        // ====================================================================
        // BASE-STATE SCENARIOS  (preserved for clarity / external compatibility)
        // ====================================================================
        public static bool scenario_NoOneOn()       { return fbCol == 0 && sbCol == 0 && tbCol == 0; }
        public static bool scenario_OnFirst()       { return fbCol == 1 && sbCol == 0 && tbCol == 0; }
        public static bool scenario_OnSecond()      { return fbCol == 0 && sbCol == 1 && tbCol == 0; }
        public static bool scenario_OnThird()       { return fbCol == 0 && sbCol == 0 && tbCol == 1; }
        public static bool scenario_OnFirstSecond() { return fbCol == 1 && sbCol == 1 && tbCol == 0; }
        public static bool scenario_OnFirstThird()  { return fbCol == 1 && sbCol == 0 && tbCol == 1; }
        public static bool scenario_OnSecondThird() { return fbCol == 0 && sbCol == 1 && tbCol == 1; }
        public static bool scenario_BasesLoaded()   { return fbCol == 1 && sbCol == 1 && tbCol == 1; }

        // ====================================================================
        // SCORE / OUT / WALK / HIT HANDLERS  (team-parameterized)
        //
        // team: "A" = away, "H" = home.
        // ====================================================================
        private static void AddRuns(string team, int runs)
        {
            if (team == "A") { atScore += runs; ATScoreTX.Text = atScore.ToString(); }
            else             { htScore += runs; HTScoreTX.Text = htScore.ToString(); }
        }

        public static void HitOccurred(string team, HitType type)
        {
            switch (type)
            {
                case HitType.Single:  SingleOccurred(team); break;
                case HitType.Double:  DoubleOccurred(team); break;
                case HitType.Triple:  TripleOccurred(team); break;
                case HitType.HomeRun: HROccurred(team);     break;
            }
        }

        // --- Single ----------------------------------------------------------
        private static void SingleOccurred(string team)
        {
            // Runners advance one base; a runner from 3rd scores.  Runners from
            // 2nd score on a single (~60% IRL; we keep the original "always" for
            // simplicity).  Bases-loaded single scores one run.
            int runs = 0;
            if (scenario_NoOneOn())            { Green_FirstBase(); }
            else if (scenario_OnFirst())       { Green_FirstBase(); Green_SecondBase(); }
            else if (scenario_OnSecond())      { Green_FirstBase(); Green_ThirdBase(); Black_SecondBase(); }
            else if (scenario_OnThird())       { Green_FirstBase(); Black_ThirdBase(); runs = 1; }
            else if (scenario_OnFirstSecond()) { Green_FirstBase(); Green_SecondBase(); Green_ThirdBase(); }
            else if (scenario_OnFirstThird())  { Green_FirstBase(); Green_SecondBase(); Black_ThirdBase(); runs = 1; }
            else if (scenario_OnSecondThird()) { Green_FirstBase(); Black_SecondBase(); Green_ThirdBase(); runs = 1; }
            else if (scenario_BasesLoaded())   { Green_FirstBase(); Green_SecondBase(); Green_ThirdBase(); runs = 1; }
            AddRuns(team, runs);
        }

        // --- Double ----------------------------------------------------------
        private static void DoubleOccurred(string team)
        {
            int runs = 0;
            if (scenario_NoOneOn())            { Green_SecondBase(); }
            else if (scenario_OnFirst())       { Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); }
            else if (scenario_OnSecond())      { Green_SecondBase(); Black_ThirdBase(); runs = 1; }
            else if (scenario_OnThird())       { Green_SecondBase(); Black_ThirdBase(); runs = 1; }
            else if (scenario_OnFirstSecond()) { Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); runs = 1; }
            else if (scenario_OnFirstThird())  { Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); runs = 1; }
            else if (scenario_OnSecondThird()) { Green_SecondBase(); Black_ThirdBase(); runs = 2; }
            else if (scenario_BasesLoaded())   { Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); runs = 2; }
            AddRuns(team, runs);
        }

        // --- Triple ----------------------------------------------------------
        private static void TripleOccurred(string team)
        {
            int runs = 0;
            if (scenario_NoOneOn())            { Green_ThirdBase(); }
            else if (scenario_OnFirst())       { Black_FirstBase(); Green_ThirdBase(); runs = 1; }
            else if (scenario_OnSecond())      { Black_SecondBase(); Green_ThirdBase(); runs = 1; }
            else if (scenario_OnThird())       { Green_ThirdBase(); runs = 1; }
            else if (scenario_OnFirstSecond()) { Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); runs = 2; }
            else if (scenario_OnFirstThird())  { Black_FirstBase(); Green_ThirdBase(); runs = 2; }
            else if (scenario_OnSecondThird()) { Black_SecondBase(); Green_ThirdBase(); runs = 2; }
            else if (scenario_BasesLoaded())   { Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); runs = 3; }
            AddRuns(team, runs);
        }

        // --- Home Run --------------------------------------------------------
        private static void HROccurred(string team)
        {
            int runs = 1; // batter always
            if (scenario_OnFirst() || scenario_OnSecond() || scenario_OnThird())            runs = 2;
            else if (scenario_OnFirstSecond() || scenario_OnFirstThird() || scenario_OnSecondThird()) runs = 3;
            else if (scenario_BasesLoaded()) runs = 4;
            AddRuns(team, runs);
            clearBases();
        }

        // --- Ground Out  (light-realism: keep original double-play heuristic)-
        public static void GroundOut(string team)
        {
            if (scenario_OnFirst())
            { Black_FirstBase(); Black_SecondBase(); Black_ThirdBase(); outCount += 2; }
            else if (scenario_OnFirstSecond())
            { Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); outCount += 2; }
            else if (scenario_OnFirstThird())
            { Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); outCount += 2; }
            else if (scenario_BasesLoaded())
            { Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); outCount += 2; }
            else
            { outCount++; }
        }

        // --- Walk ------------------------------------------------------------
        public static void Walk(string team)
        {
            int runs = 0;
            if (scenario_NoOneOn())            { Green_FirstBase(); }
            else if (scenario_OnFirst())       { Green_FirstBase(); Green_SecondBase(); }
            else if (scenario_OnSecond())      { Green_FirstBase(); Green_SecondBase(); }
            else if (scenario_OnThird())       { Green_FirstBase(); }
            else if (scenario_OnFirstSecond()) { Green_FirstBase(); Green_SecondBase(); Green_ThirdBase(); }
            else if (scenario_OnFirstThird())  { Green_FirstBase(); Green_SecondBase(); }
            else if (scenario_OnSecondThird()) { Green_FirstBase(); }
            else if (scenario_BasesLoaded())   { runs = 1; } // bases stay loaded, run scored
            AddRuns(team, runs);
        }

        // ====================================================================
        // COUNT INDICATOR UPDATERS
        // ====================================================================
        public static void whenFoulOccurs()
        {
            // Fouls add a strike only if strikes < 2 (MLB rule)
            if (strikeCount < 2)
            {
                strikeCount++;
                if (strikeCount == 1)
                    s1.Fill = new SolidColorBrush(Colors.Orange);
                else
                {
                    s1.Fill = new SolidColorBrush(Colors.Orange);
                    s2.Fill = new SolidColorBrush(Colors.Orange);
                }
            }
        }

        public static void whenOutOccurs()
        {
            if (outCount == 1)
                o1.Fill = new SolidColorBrush(Colors.Red);
            else if (outCount == 2)
            {
                o1.Fill = new SolidColorBrush(Colors.Red);
                o2.Fill = new SolidColorBrush(Colors.Red);
            }
            else if (outCount >= 3)
            {
                o1.Fill = new SolidColorBrush(Colors.Black);
                o2.Fill = new SolidColorBrush(Colors.Black);
            }
        }

        public static void whenStrikeOccurs()
        {
            if (strikeCount == 1)
                s1.Fill = new SolidColorBrush(Colors.Orange);
            else if (strikeCount == 2)
            {
                s1.Fill = new SolidColorBrush(Colors.Orange);
                s2.Fill = new SolidColorBrush(Colors.Orange);
            }
            else if (strikeCount >= 3)
            {
                clearStrikesBalls();
            }
        }

        public static void whenBallOccurs()
        {
            if (ballCount == 1)
                b1.Fill = new SolidColorBrush(Colors.Blue);
            else if (ballCount == 2)
            {
                b1.Fill = new SolidColorBrush(Colors.Blue);
                b2.Fill = new SolidColorBrush(Colors.Blue);
            }
            else if (ballCount == 3)
            {
                b1.Fill = new SolidColorBrush(Colors.Blue);
                b2.Fill = new SolidColorBrush(Colors.Blue);
                b3.Fill = new SolidColorBrush(Colors.Blue);
            }
            else if (ballCount >= 4)
            {
                clearStrikesBalls();
            }
        }

        // ====================================================================
        // BATTING ORDER ROTATION
        // ====================================================================
        public static int GetABO()  { if (awayBatterOrder >= 9) awayBatterOrder = 0; return awayBatterOrder; }
        public static int GetHBO()  { if (homeBatterOrder >= 9) homeBatterOrder = 0; return homeBatterOrder; }
        public static int GetABO1() { return (awayBatterOrder + 1) < 9 ? awayBatterOrder + 1 : 0; }
        public static int GetHBO1() { return (homeBatterOrder + 1) < 9 ? homeBatterOrder + 1 : 0; }
        public static int GetABO2()
        {
            if (awayBatterOrder + 2 < 9) return awayBatterOrder + 2;
            if (awayBatterOrder + 2 < 10) return 0;
            return 1;
        }
        public static int GetHBO2()
        {
            if (homeBatterOrder + 2 < 9) return homeBatterOrder + 2;
            if (homeBatterOrder + 2 < 10) return 0;
            return 1;
        }

        // ====================================================================
        // STRIKE-ZONE BUTTON HANDLERS  (one per location)
        // Each forwards to the central PitchClicked() with a specific zone.
        // ====================================================================
        public void HL_Click(object s, RoutedEventArgs e) { PitchClicked(StrikeZone.HighLeft); }
        public void HM_Click(object s, RoutedEventArgs e) { PitchClicked(StrikeZone.HighMid); }
        public void HR_Click(object s, RoutedEventArgs e) { PitchClicked(StrikeZone.HighRight); }
        public void ML_Click(object s, RoutedEventArgs e) { PitchClicked(StrikeZone.MidLeft); }
        public void MM_Click(object s, RoutedEventArgs e) { PitchClicked(StrikeZone.MidMid); }
        public void MR_Click(object s, RoutedEventArgs e) { PitchClicked(StrikeZone.MidRight); }
        public void LL_Click(object s, RoutedEventArgs e) { PitchClicked(StrikeZone.LowLeft); }
        public void LM_Click(object s, RoutedEventArgs e) { PitchClicked(StrikeZone.LowMid); }
        public void LR_Click(object s, RoutedEventArgs e) { PitchClicked(StrikeZone.LowRight); }
        public void Ball_Click(object s, RoutedEventArgs e) { PitchClicked(StrikeZone.Ball); }

        /// <summary>
        /// Central pitch-click handler.  Drives the at-bat for the team currently
        /// hitting (odd inning = away, even inning = home), updates UI state.
        /// </summary>
        private void PitchClicked(StrikeZone zone)
        {
            bool awayBatting = (inn % 2 != 0);

            // Resolve the pitch outcome FIRST (this may change outCount and
            // therefore flip the inning), then refresh on-screen state.
            ResolveCurrentPitch(zone, awayBatting ? "A" : "H");

            if (awayBatting)
            {
                currentlyPText.Text    = " " + currentHomePitcher;
                homePitchesThrown++;
                pitchesThrownText.Text = " " + homePitchesThrown.ToString();
                atBatText.Text         = " " + awayTeamBattersX[GetABO()].ToString();
                onDeckText.Text        = " " + awayTeamBattersX[GetABO1()].ToString();
                inTheHoleText.Text     = " " + awayTeamBattersX[GetABO2()].ToString();
                TBInd.Text             = "TOP ";
            }
            else
            {
                currentlyPText.Text    = " " + currentAwayPitcher;
                awayPitchesThrown++;
                pitchesThrownText.Text = " " + awayPitchesThrown.ToString();
                atBatText.Text         = " " + homeTeamBattersX[GetHBO()].ToString();
                onDeckText.Text        = " " + homeTeamBattersX[GetHBO1()].ToString();
                inTheHoleText.Text     = " " + homeTeamBattersX[GetHBO2()].ToString();
                TBInd.Text             = "BOT ";
            }

            inn2 = inn;
            inn2 = Math.Ceiling(inn2 / 2);
            InnTB.Text = " " + inn2.ToString();
        }

        /// <summary>
        /// Per-pitch outcome dispatch.  Pulls a pitch outcome from
        /// <see cref="BaseballSimulation"/> and applies it to game state.
        /// </summary>
        private static void ResolveCurrentPitch(StrikeZone zone, string team)
        {
            // End-of-inning guard:  3 outs flips inning, clears counts and bases.
            if (outCount >= 3)
            {
                AdvanceInning(team);
                return;
            }

            ArrayList lineup = (team == "A") ? awayTeamBattersX : homeTeamBattersX;
            int order       = (team == "A") ? GetABO() : GetHBO();
            string batter   = lineup[order].ToString();

            PitchOutcome outcome = BaseballSimulation.ResolvePitch(zone, ballCount, strikeCount);

            switch (outcome)
            {
                case PitchOutcome.Hit:
                {
                    HitType ht = BaseballSimulation.ResolveHitType(zone);
                    actionBarX.Text = batter + " hits a " + BaseballSimulation.HitTypeToString(ht);
                    addActionToGameLog();
                    HitOccurred(team, ht);
                    clearStrikesBalls();
                    AdvanceBatter(team);
                    break;
                }
                case PitchOutcome.GroundOut:
                {
                    actionBarX.Text = batter + " hits a groundout.";
                    addActionToGameLog();
                    GroundOut(team); whenOutOccurs(); clearStrikesBalls();
                    AdvanceBatter(team);
                    break;
                }
                case PitchOutcome.FlyOut:
                {
                    actionBarX.Text = batter + " hits a flyout.";
                    addActionToGameLog();
                    outCount++; whenOutOccurs(); clearStrikesBalls();
                    AdvanceBatter(team);
                    break;
                }
                case PitchOutcome.Foul:
                {
                    actionBarX.Text = batter + " hits a foul ball.";
                    addActionToGameLog();
                    whenFoulOccurs();
                    break;
                }
                case PitchOutcome.SwingingStrike:
                {
                    strikeCount++;
                    if (strikeCount == 3)
                    {
                        actionBarX.Text = batter + " struck out swinging.";
                        addActionToGameLog();
                        outCount++; whenOutOccurs(); clearStrikesBalls();
                        AdvanceBatter(team);
                    }
                    else
                    {
                        actionBarX.Text = batter + " swings and misses.";
                        addActionToGameLog();
                        whenStrikeOccurs();
                    }
                    break;
                }
                case PitchOutcome.CalledStrike:
                {
                    strikeCount++;
                    if (strikeCount == 3)
                    {
                        actionBarX.Text = batter + " struck out looking.";
                        addActionToGameLog();
                        outCount++; whenOutOccurs(); clearStrikesBalls();
                        AdvanceBatter(team);
                    }
                    else
                    {
                        actionBarX.Text = batter + " looks at a strike.";
                        addActionToGameLog();
                        whenStrikeOccurs();
                    }
                    break;
                }
                case PitchOutcome.Ball:
                {
                    ballCount++;
                    if (ballCount == 4)
                    {
                        actionBarX.Text = batter + " walked.";
                        addActionToGameLog();
                        Walk(team); clearStrikesBalls();
                        AdvanceBatter(team);
                    }
                    else
                    {
                        actionBarX.Text = batter + " looks at a ball.";
                        addActionToGameLog();
                        whenBallOccurs();
                    }
                    break;
                }
            }
        }

        private static void AdvanceBatter(string team)
        {
            if (team == "A") awayBatterOrder++;
            else             homeBatterOrder++;
        }

        private static async void AdvanceInning(string team)
        {
            inn++;
            await Windows.Storage.FileIO.AppendTextAsync(gameLog,
                "UPDATE- INN: " + TBInd.Text + inn2.ToString() + ", " +
                setRosters_getATName.Substring(0, Math.Min(3, setRosters_getATName.Length)) +
                " " + atScore.ToString() + ": " +
                setRosters_getHTName.Substring(0, Math.Min(3, setRosters_getHTName.Length)) +
                ": " + htScore.ToString() + "\n");

            // The old algorithm decremented the *other* team's pitch counter
            // because a pitch was "counted" before the inning flipped; preserve
            // that behavior so per-pitcher counts stay accurate.
            if (team == "A") awayPitchesThrown--;
            else             homePitchesThrown--;

            clearShapes();
        }

        // ====================================================================
        // END-OF-GAME
        // ====================================================================
        private async void GameOver_Click(object sender, RoutedEventArgs e)
        {
            await Windows.Storage.FileIO.AppendTextAsync(gameLog, "Game ended at inning: " + TBInd.Text + inn2.ToString() + "\n");
            await Windows.Storage.FileIO.AppendTextAsync(gameLog, setRosters_getATName + " - Score: " + atScore.ToString() + "\n");
            await Windows.Storage.FileIO.AppendTextAsync(gameLog, setRosters_getHTName + " - Score: " + htScore.ToString() + "\n");
            await Windows.Storage.FileIO.AppendTextAsync(gameLog, currentAwayPitcher + " - Pitchcount: " + awayPitchesThrown.ToString() + "\n");
            await Windows.Storage.FileIO.AppendTextAsync(gameLog, currentHomePitcher + " - Pitchcount: " + homePitchesThrown.ToString() + "\n");

            FINALGAMELOG = await Windows.Storage.FileIO.ReadTextAsync(gameLog);
            this.Frame.Navigate(typeof(GameLogScreen));
        }

        public async static void addActionToGameLog()
        {
            await Windows.Storage.FileIO.AppendTextAsync(gameLog, actionBarX.Text + "\n");
        }

        // ====================================================================
        // PITCHER / BENCH SWAPS  (unchanged logic, formatting cleaned up)
        // ====================================================================
        private async void awayPlayerA_SC(object sender, SelectionChangedEventArgs e)
        {
            if (awayPlayerA.SelectedItem == null) return;
            string player = awayPlayerA.SelectedItem.ToString();

            for (int i = 1; i <= 5; i++) // bullpen slots
            {
                if (i < awayTeamPBEX.Count && player.Equals(awayTeamPBEX[i].ToString()))
                {
                    await Windows.Storage.FileIO.AppendTextAsync(gameLog,
                        currentAwayPitcher + " has been replaced by " + player + ". " +
                        currentAwayPitcher + " threw: " + awayPitchesThrown.ToString() + " pitches.\n");
                    currentAwayPitcher = player;
                    awayPitchesThrown  = 0;
                    awayPlayerA.PlaceholderText = SafeShort(setRosters_getATName);
                    return;
                }
            }
            for (int i = 6; i <= 8; i++) // bench slots
            {
                if (i < awayTeamPBEX.Count && player.Equals(awayTeamPBEX[i].ToString()))
                {
                    await Windows.Storage.FileIO.AppendTextAsync(gameLog,
                        awayTeamBattersX[GetABO()].ToString() + " has been replaced by " + player + ".\n");
                    awayTeamBattersX[GetABO()] = player;
                    awayPlayerA.PlaceholderText = SafeShort(setRosters_getATName);
                    return;
                }
            }
        }

        private async void homePlayerA_SC(object sender, SelectionChangedEventArgs e)
        {
            if (homePlayerA.SelectedItem == null) return;
            string player = homePlayerA.SelectedItem.ToString();

            for (int i = 1; i <= 5; i++) // bullpen
            {
                if (i < homeTeamPBEX.Count && player.Equals(homeTeamPBEX[i].ToString()))
                {
                    await Windows.Storage.FileIO.AppendTextAsync(gameLog,
                        currentHomePitcher + " has been replaced by " + player + ". " +
                        currentHomePitcher + " threw: " + homePitchesThrown.ToString() + " pitches.\n");
                    currentHomePitcher = player;
                    homePitchesThrown  = 0;
                    homePlayerA.PlaceholderText = SafeShort(setRosters_getHTName);
                    return;
                }
            }
            for (int i = 6; i <= 8; i++) // bench
            {
                if (i < homeTeamPBEX.Count && player.Equals(homeTeamPBEX[i].ToString()))
                {
                    await Windows.Storage.FileIO.AppendTextAsync(gameLog,
                        homeTeamBattersX[GetHBO()].ToString() + " has been replaced by " + player + ".\n");
                    homeTeamBattersX[GetHBO()] = player;
                    homePlayerA.PlaceholderText = SafeShort(setRosters_getHTName);
                    return;
                }
            }
        }

        private static string SafeShort(string name)
        {
            if (string.IsNullOrEmpty(name)) return "";
            return name.Length < 3 ? name : name.Substring(0, 3);
        }
    }
}
