using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using System.Text;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BaseballGameCreator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BaseballGameScreen : Page
    {
        public static ArrayList awayTeamBattersX = new ArrayList();
        public static ArrayList homeTeamBattersX = new ArrayList();
        public static ArrayList awayTeamPBEX = new ArrayList();
        public static ArrayList homeTeamPBEX = new ArrayList();

        public static int botSX, topSX, leftSX, rightSX, midSX, ballRR, aPRandom, hPRandom, aBRandom, hBRandom; 
        public static int inn, awayBatterOrder, homeBatterOrder, strikeCount, ballCount, outCount, atScore, htScore, fbCol, sbCol, tbCol;
        public static int awayPitchesThrown, homePitchesThrown;
        public static double inn2;

        public static Windows.UI.Xaml.Shapes.Rectangle firstB, secondB, thirdB;
        public static Ellipse s1, s2, s3, b1, b2, b3, b4, o1, o2, o3;

        public static object important;

        public static TextBlock actionBarX, ATScoreTX, HTScoreTX, TBInd, InnTB;

        public static Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        public static Windows.Storage.StorageFile gameLog;

        public static string currentAwayPitcher, currentHomePitcher; 

        /*public static string[] XY = new string[1];
        public static List<string> XZ = new List<string>();
        public static List<string> XYZ = new List<string>();*/
        
        public static string FINALGAMELOG;
        public static string ATNameXYZ, HTNameXYZ;

        //public static string path1, pathA, pathB, pathX;

        public BaseballGameScreen()
        {
            this.InitializeComponent();

            gameStart();
        }

        public static string getFINALGAMELOG()
        { return FINALGAMELOG; }
        public static string getAwayTeamNameForGL()
        { return ATNameXYZ; }
        public static string getHomeTeamNameForGL()
        { return HTNameXYZ; }

        public async void gameStart()
        {
            gameLog = await storageFolder.CreateFileAsync("GameLog.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);

            /*pathA = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            pathB = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            path1 = System.IO.Path.Combine(pathB, "GameLog.txt");
            pathX = @"C:\Users\ahm53\AppData\Local\Packages\ae6043be-cfa4-4db1-9a89-328d3abf3fde_js1exkfqt36te\LocalState\GameLog.txt";*/

            awayTeamBattersX = SetRosters.getATBatters(); homeTeamBattersX = SetRosters.getHTBatters(); awayTeamPBEX = SetRosters.getATPBE(); homeTeamPBEX = SetRosters.getHTPBE();

            ATNameT.Text = SetRosters.getATName().Substring(0, 3);
            HTNameT.Text = SetRosters.getHTName().Substring(0, 3);
            ATNameXYZ = SetRosters.getATName().Substring(0, 3);
            HTNameXYZ = SetRosters.getHTName().Substring(0, 3);

            awayPlayerA.PlaceholderText = SetRosters.getATName().Substring(0, 3);
            for (int i = 1; i < awayTeamPBEX.Count; i++)
                awayPlayerA.Items.Add(awayTeamPBEX[i]);
            for (int j = 1; j < homeTeamPBEX.Count; j++)
                homePlayerA.Items.Add(homeTeamPBEX[j]);
            homePlayerA.PlaceholderText = SetRosters.getHTName().Substring(0, 3);

            inn = 1; inn2 = inn;

            actionBarX = ActionBar; TBInd = topbotIndicator; InnTB = currentGameInning;

            firstB = firstBase; secondB = secondBase; thirdB = thirdBase;

            s1 = strike1; s2 = strike2; b1 = ball1; b2 = ball2; b3 = ball3; o1 = out1; o2 = out2;

            fbCol = 0; sbCol = 0; tbCol = 0; //black = 0, green = 1, red = 2; blue = 3, orange = 4

            awayBatterOrder = 0; homeBatterOrder = 0; strikeCount = 0; ballCount = 0; outCount = 0; atScore = 0; htScore = 0;

            ATScoreTX = ATScoreT; HTScoreTX = HTScoreT;

            ATScoreTX.Text = atScore.ToString(); HTScoreTX.Text = htScore.ToString();

            currentlyPText.Text = " " + homeTeamPBEX[0].ToString();
            currentAwayPitcher = " " + awayTeamPBEX[0].ToString();
            currentHomePitcher = " " + homeTeamPBEX[0].ToString();
            atBatText.Text = " " + awayTeamBattersX[GetABO()].ToString();
            onDeckText.Text = " " + awayTeamBattersX[GetABO1()].ToString();
            inTheHoleText.Text = " " + awayTeamBattersX[GetABO2()].ToString();
        }

        public static void clearShapes()
        {
            outCount = 0; strikeCount = 0; ballCount = 0;
            clearBases();
            s1.Fill = new SolidColorBrush(Colors.Black); s2.Fill = new SolidColorBrush(Colors.Black); 
            b1.Fill = new SolidColorBrush(Colors.Black); b2.Fill = new SolidColorBrush(Colors.Black); b3.Fill = new SolidColorBrush(Colors.Black); 
            o1.Fill = new SolidColorBrush(Colors.Black); o2.Fill = new SolidColorBrush(Colors.Black); 
        }

        public static void clearBases()
        {
            firstB.Fill = new SolidColorBrush(Colors.Black); secondB.Fill = new SolidColorBrush(Colors.Black); thirdB.Fill = new SolidColorBrush(Colors.Black);
            fbCol = 0; sbCol = 0; tbCol = 0;        
        }
        
        public static void clearStrikesBalls()
        {
            strikeCount = 0; ballCount = 0;
            s1.Fill = new SolidColorBrush(Colors.Black); s2.Fill = new SolidColorBrush(Colors.Black);
            b1.Fill = new SolidColorBrush(Colors.Black); b2.Fill = new SolidColorBrush(Colors.Black); b3.Fill = new SolidColorBrush(Colors.Black);
        }

        public static void Green_FirstBase()
        { firstB.Fill = new SolidColorBrush(Colors.Green); fbCol = 1; }
        public static void Green_SecondBase()
        { secondB.Fill = new SolidColorBrush(Colors.Green); sbCol = 1; }
        public static void Green_ThirdBase()
        { thirdB.Fill = new SolidColorBrush(Colors.Green); tbCol = 1; }
        public static void Black_FirstBase()
        { firstB.Fill = new SolidColorBrush(Colors.Black); fbCol = 0; }
        public static void Black_SecondBase()
        { secondB.Fill = new SolidColorBrush(Colors.Black); sbCol = 0; }
        public static void Black_ThirdBase()
        { thirdB.Fill = new SolidColorBrush(Colors.Black); tbCol = 0; }

        private async void GameOver_Click(object sender, RoutedEventArgs e)
        {
            await Windows.Storage.FileIO.AppendTextAsync(gameLog, "Game ended at inning: " + TBInd.Text + inn2.ToString() + "\n");
            await Windows.Storage.FileIO.AppendTextAsync(gameLog, SetRosters.getATName() + " - Score: " + atScore.ToString() + "\n");
            await Windows.Storage.FileIO.AppendTextAsync(gameLog, SetRosters.getHTName() + " - Score: " + htScore.ToString() + "\n");
            await Windows.Storage.FileIO.AppendTextAsync(gameLog, currentAwayPitcher + " - Pitchcount: " + awayPitchesThrown.ToString() + "\n");
            await Windows.Storage.FileIO.AppendTextAsync(gameLog, currentHomePitcher + " - Pitchcount: " + homePitchesThrown.ToString() + "\n");

            FINALGAMELOG = await Windows.Storage.FileIO.ReadTextAsync(gameLog);
            this.Frame.Navigate(typeof(GameLogScreen));
        }

        public async static void addActionToGameLog()
        {
            await Windows.Storage.FileIO.AppendTextAsync(gameLog, actionBarX.Text + "\n");
        }

        private async void awayPlayerA_SC(object sender, SelectionChangedEventArgs e)
        {
            string player = awayPlayerA.SelectedItem.ToString();
            if(player.Equals(awayTeamPBEX[1].ToString())) //bullpen
            {
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, currentAwayPitcher + " has been replaced by " + player + ". " + currentAwayPitcher + " threw: " + awayPitchesThrown.ToString() + " pitches." + "\n");
                currentAwayPitcher = player;
                awayPitchesThrown = 0;
                awayPlayerA.PlaceholderText = SetRosters.getATName().Substring(0, 3);
            }
            else if (player.Equals(awayTeamPBEX[2].ToString())) //bullpen
            {
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, currentAwayPitcher + " has been replaced by " + player + ". " + currentAwayPitcher + " threw: " + awayPitchesThrown.ToString() + " pitches." + "\n");
                currentAwayPitcher = player;
                awayPitchesThrown = 0;
                awayPlayerA.PlaceholderText = SetRosters.getATName().Substring(0, 3);
            }
            else if (player.Equals(awayTeamPBEX[3].ToString())) //bullpen
            {
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, currentAwayPitcher + " has been replaced by " + player + ". " + currentAwayPitcher + " threw: " + awayPitchesThrown.ToString() + " pitches." + "\n");
                currentAwayPitcher = player;
                awayPitchesThrown = 0;
                awayPlayerA.PlaceholderText = SetRosters.getATName().Substring(0, 3);
            }
            else if (player.Equals(awayTeamPBEX[4].ToString())) //bullpen
            {
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, currentAwayPitcher + " has been replaced by " + player + ". " + currentAwayPitcher + " threw: " + awayPitchesThrown.ToString() + " pitches." + "\n");
                currentAwayPitcher = player;
                awayPitchesThrown = 0;
                awayPlayerA.PlaceholderText = SetRosters.getATName().Substring(0, 3);
            } 
            else if (player.Equals(awayTeamPBEX[5].ToString())) //bullpen
            {
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, currentAwayPitcher + " has been replaced by " + player + ". " + currentAwayPitcher + " threw: " + awayPitchesThrown.ToString() + " pitches." + "\n");
                currentAwayPitcher = player;
                awayPitchesThrown = 0;
                awayPlayerA.PlaceholderText = SetRosters.getATName().Substring(0, 3);
            }
            else if (player.Equals(awayTeamPBEX[6].ToString())) //bench
            {
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, awayTeamBattersX[GetABO()].ToString() + " has been replaced by " + player + "." + "\n");
                awayTeamBattersX[GetABO()] = player;
                awayPlayerA.PlaceholderText = SetRosters.getATName().Substring(0, 3);
            }
            else if (player.Equals(awayTeamPBEX[7].ToString())) //bench
            {
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, awayTeamBattersX[GetABO()].ToString() + " has been replaced by " + player + "." + "\n");
                awayTeamBattersX[GetABO()] = player;
                awayPlayerA.PlaceholderText = SetRosters.getATName().Substring(0, 3);
            }
            else if (player.Equals(awayTeamPBEX[8].ToString())) //bench
            {
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, awayTeamBattersX[GetABO()].ToString() + " has been replaced by " + player + "." + "\n");
                awayTeamBattersX[GetABO()] = player;
                awayPlayerA.PlaceholderText = SetRosters.getATName().Substring(0, 3);
            }
        }

        private async void homePlayerA_SC(object sender, SelectionChangedEventArgs e)
        {
            string player = homePlayerA.SelectedItem.ToString();
            if (player.Equals(homeTeamPBEX[1].ToString())) //bullpen
            {
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, currentHomePitcher + " has been replaced by " + player + ". " + currentHomePitcher + " threw: " + homePitchesThrown.ToString() + " pitches." + "\n");
                currentHomePitcher = player;
                homePitchesThrown = 0;
                //homePlayerA.Items.RemoveAt(0);
                homePlayerA.PlaceholderText = SetRosters.getHTName().Substring(0, 3);
            }
            else if (player.Equals(homeTeamPBEX[2].ToString())) //bullpen
            {
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, currentHomePitcher + " has been replaced by " + player + ". " + currentHomePitcher + " threw: " + homePitchesThrown.ToString() + " pitches." + "\n");
                currentHomePitcher = player;
                homePitchesThrown = 0;
                homePlayerA.PlaceholderText = SetRosters.getHTName().Substring(0, 3);
            }
            else if (player.Equals(homeTeamPBEX[3].ToString())) //bullpen
            {
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, currentHomePitcher + " has been replaced by " + player + ". " + currentHomePitcher + " threw: " + homePitchesThrown.ToString() + " pitches." + "\n");
                currentHomePitcher = player;
                homePitchesThrown = 0;
                homePlayerA.PlaceholderText = SetRosters.getHTName().Substring(0, 3);
            }
            else if (player.Equals(homeTeamPBEX[4].ToString())) //bullpen
            {
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, currentHomePitcher + " has been replaced by " + player + ". " + currentHomePitcher + " threw: " + homePitchesThrown.ToString() + " pitches." + "\n");
                currentHomePitcher = player;
                homePitchesThrown = 0;
                homePlayerA.PlaceholderText = SetRosters.getHTName().Substring(0, 3);
            }
            else if (player.Equals(homeTeamPBEX[5].ToString())) //bullpen
            {
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, currentHomePitcher + " has been replaced by " + player + ". " + currentHomePitcher + " threw: " + homePitchesThrown.ToString() + " pitches." + "\n");
                currentHomePitcher = player;
                homePitchesThrown = 0;
                homePlayerA.PlaceholderText = SetRosters.getHTName().Substring(0, 3);
            }
            else if (player.Equals(homeTeamPBEX[6].ToString())) //bench
            {
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, homeTeamBattersX[GetHBO()].ToString() + " has been replaced by " + player + "." + "\n");
                homeTeamBattersX[GetHBO()] = player;
                homePlayerA.PlaceholderText = SetRosters.getHTName().Substring(0, 3);
            }
            else if (player.Equals(homeTeamPBEX[7].ToString())) //bench
            {
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, homeTeamBattersX[GetHBO()].ToString() + " has been replaced by " + player + "." + "\n");
                homeTeamBattersX[GetHBO()] = player;
                homePlayerA.PlaceholderText = SetRosters.getHTName().Substring(0, 3);
            }
            else if (player.Equals(homeTeamPBEX[8].ToString())) //bench
            {
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, homeTeamBattersX[GetHBO()].ToString() + " has been replaced by " + player + "." + "\n");
                homeTeamBattersX[GetHBO()] = player;
                homePlayerA.PlaceholderText = SetRosters.getHTName().Substring(0, 3);
            }
        }

        public static int midSRandom()
        { Random rnd = new Random(); midSX = rnd.Next(1, 51); return midSX; }
        public static int botSRandom()
        { Random rnd = new Random(); botSX = rnd.Next(1, 51); return botSX; }
        public static int topSRandom()
        { Random rnd = new Random(); topSX = rnd.Next(1, 51); return topSX; }

        public static int leftSRandom()
        { Random rnd = new Random(); leftSX = rnd.Next(1, 51); return leftSX; }
        public static int rightSRandom()
        { Random rnd = new Random(); rightSX = rnd.Next(1, 51); return rightSX; }
        public static int BallButtonRandom()
        { Random rnd = new Random(); ballRR = rnd.Next(1, 51); return ballRR; }

        public static bool scenario_NoOneOn()
        {
            if (fbCol==0)
            {
                if (sbCol==0)
                {
                    if (tbCol==0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public static bool scenario_OnFirst()
        {
            if (fbCol==1)
            {
                if (sbCol==0)
                {
                    if (tbCol==0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public static bool scenario_OnFirstSecond()
        {
            if (fbCol==1)
            {
                if (sbCol==1)
                {
                    if (tbCol==0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public static bool scenario_OnFirstThird()
        {
            if (fbCol==1)
            {
                if (sbCol==0)
                {
                    if (tbCol==1)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public static bool scenario_BasesLoaded()
        {
            if (fbCol==1)
            {
                if (sbCol==1)
                {
                    if (tbCol==1)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public static bool scenario_OnSecond()
        {
            if (fbCol==0)
            {
                if (sbCol==1)
                {
                    if (tbCol==0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public static bool scenario_OnSecondThird()
        {
            if (fbCol==0)
            {
                if (sbCol==1)
                {
                    if (tbCol==1)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public static bool scenario_OnThird()
        {
            if (fbCol==0)
            {
                if (sbCol==0)
                {
                    if (tbCol==1)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public static string generateAwayHitType()
        {
            Random rnd = new Random();
            int x = rnd.Next(0, 50);
            if (x < 26)
            {
                awaySingleOccurred();
                return "single.";
            }
            else if (x > 25 && x < 41)
            {
                awayDoubleOccurred();
                return "double.";
            }
            else if (x > 40 && x < 46)
            {
                awayTripleOccurred();
                return "triple.";
            }
            else
            {
                awayHROccurred();
                return "homerun.";
            }
        }

        public static void awaySingleOccurred()
        {
            if (scenario_NoOneOn()) //0
            {
                Green_FirstBase(); Black_SecondBase(); Black_ThirdBase();
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnFirst()) //1
            {
                Green_FirstBase(); Green_SecondBase(); Black_ThirdBase();
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnSecond()) //2
            {
                Green_FirstBase(); Black_SecondBase(); Green_ThirdBase();
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnThird()) //3
            {
                Green_FirstBase(); Black_SecondBase(); Black_ThirdBase(); atScore++;
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnFirstSecond()) //4
            {
                Green_FirstBase(); Green_SecondBase(); Green_ThirdBase();
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnFirstThird()) //5
            {
                Green_FirstBase(); Green_SecondBase(); Black_ThirdBase(); atScore++;
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnSecondThird()) //6
            {
                Green_FirstBase(); Black_SecondBase(); Green_ThirdBase(); atScore++;
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_BasesLoaded()) //7
            {
                Green_FirstBase(); Green_SecondBase(); Green_ThirdBase(); atScore++;
                ATScoreTX.Text = atScore.ToString();
            }
        }

        public static void awayDoubleOccurred()
        {
            if (scenario_NoOneOn()) //0
            {
                Black_FirstBase(); Green_SecondBase(); Black_ThirdBase();
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnFirst()) //1
            {
                Black_FirstBase(); Green_SecondBase(); Green_ThirdBase();
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnSecond()) //2
            {
                Black_FirstBase(); Green_SecondBase(); Black_ThirdBase(); atScore++;
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnThird()) //3
            {
                Black_FirstBase(); Green_SecondBase(); Black_ThirdBase(); atScore++;
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnFirstSecond()) //4
            {
                Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); atScore++;
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnFirstThird()) //5
            {
                Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); atScore++;
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnSecondThird()) //6
            {
                Black_FirstBase(); Green_SecondBase(); Black_ThirdBase(); atScore += 2;
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_BasesLoaded()) //7
            {
                Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); atScore += 2;
                ATScoreTX.Text = atScore.ToString();
            }
        }

        public static void awayTripleOccurred()
        {
            if (scenario_NoOneOn()) //0
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase();
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnFirst()) //1
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); atScore++;
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnSecond()) //2
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); atScore++;
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnThird()) //3
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); atScore++;
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnFirstSecond()) //4
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); atScore += 2;
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnFirstThird()) //5
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); atScore += 2;
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnSecondThird()) //6
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); atScore += 2;
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_BasesLoaded()) //7
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); atScore += 3;
                ATScoreTX.Text = atScore.ToString();
            }
        }

        public static void awayHROccurred()
        {
            if (scenario_NoOneOn()) //0
            {
                atScore++; ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnFirst()) //1
            {
                atScore += 2; ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnSecond()) //2
            {
                atScore += 2; ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnThird()) //3
            {
                atScore += 2; ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnFirstSecond()) //4
            {
                atScore += 3; ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnFirstThird()) //5
            {
                atScore += 3; ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnSecondThird()) //6
            {
                atScore += 3; ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_BasesLoaded()) //7
            {
                atScore += 4; ATScoreTX.Text = atScore.ToString();
            }
            clearBases();
        }

        public static void awayGroundOut()
        {
            if (scenario_OnFirst())
            {
                Black_FirstBase(); Black_SecondBase(); Black_ThirdBase(); outCount += 2;
            }
            else if (scenario_OnFirstSecond())
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); outCount += 2;
            }
            else if (scenario_OnFirstThird())
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); outCount += 2;
            }
            else if (scenario_BasesLoaded())
            {
                Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); outCount += 2;
            }
            else
            {
                outCount++;
            }
        }

        public static void awayWalk()
        {
            if (scenario_NoOneOn()) //0
            {
                Green_FirstBase(); Black_SecondBase(); Black_ThirdBase();
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnFirst()) //1
            {
                Green_FirstBase(); Green_SecondBase(); Black_ThirdBase();
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnSecond()) //2
            {
                Green_FirstBase(); Green_SecondBase(); Black_ThirdBase();
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnThird()) //3
            {
                Green_FirstBase(); Black_SecondBase(); Green_ThirdBase();
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnFirstSecond()) //4
            {
                Green_FirstBase(); Green_SecondBase(); Green_ThirdBase();
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnFirstThird()) //5
            {
                Green_FirstBase(); Green_SecondBase(); Green_ThirdBase();
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_OnSecondThird()) //6
            {
                Green_FirstBase(); Green_SecondBase(); Green_ThirdBase();
                ATScoreTX.Text = atScore.ToString();
            }
            else if (scenario_BasesLoaded()) //7
            {
                Green_FirstBase(); Green_SecondBase(); Green_ThirdBase(); atScore++;
                ATScoreTX.Text = atScore.ToString();
            }
        }

        public static string generateHomeHitType()
        {
            Random rnd = new Random();
            int x = rnd.Next(0, 50);
            if (x < 26)
            {
                homeSingleOccurred();
                return "single.";
            }
            else if (x > 25 && x < 41)
            {
                homeDoubleOccurred();
                return "double.";
            }
            else if (x > 40 && x < 46)
            {
                homeTripleOccurred();
                return "triple.";
            }
            else
            {
                homeHROccurred();
                return "homerun.";
            }
        }

        public static void homeSingleOccurred()
        {
            if (scenario_NoOneOn()) //0
            {
                Green_FirstBase(); Black_SecondBase(); Black_ThirdBase();
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnFirst()) //1
            {
                Green_FirstBase(); Green_SecondBase(); Black_ThirdBase();
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnSecond()) //2
            {
                Green_FirstBase(); Black_SecondBase(); Green_ThirdBase();
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnThird()) //3
            {
                Green_FirstBase(); Black_SecondBase(); Black_ThirdBase(); htScore++;
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnFirstSecond()) //4
            {
                Green_FirstBase(); Green_SecondBase(); Green_ThirdBase();
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnFirstThird()) //5
            {
                Green_FirstBase(); Green_SecondBase(); Black_ThirdBase(); htScore++;
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnSecondThird()) //6
            {
                Green_FirstBase(); Black_SecondBase(); Green_ThirdBase(); htScore++;
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_BasesLoaded()) //7
            {
                Green_FirstBase(); Green_SecondBase(); Green_ThirdBase(); htScore++;
                HTScoreTX.Text = htScore.ToString();
            }
        }

        public static void homeDoubleOccurred()
        {
            if (scenario_NoOneOn()) //0
            {
                Black_FirstBase(); Green_SecondBase(); Black_ThirdBase();
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnFirst()) //1
            {
                Black_FirstBase(); Green_SecondBase(); Green_ThirdBase();
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnSecond()) //2
            {
                Black_FirstBase(); Green_SecondBase(); Black_ThirdBase(); htScore++;
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnThird()) //3
            {
                Black_FirstBase(); Green_SecondBase(); Black_ThirdBase(); htScore++;
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnFirstSecond()) //4
            {
                Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); htScore++;
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnFirstThird()) //5
            {
                Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); htScore++;
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnSecondThird()) //6
            {
                Black_FirstBase(); Green_SecondBase(); Black_ThirdBase(); htScore += 2;
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_BasesLoaded()) //7
            {
                Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); htScore += 2;
                HTScoreTX.Text = htScore.ToString();
            }
        }

        public static void homeTripleOccurred()
        {
            if (scenario_NoOneOn()) //0
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase();
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnFirst()) //1
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); htScore++;
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnSecond()) //2
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); htScore++;
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnThird()) //3
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); htScore++;
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnFirstSecond()) //4
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); htScore += 2;
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnFirstThird()) //5
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); htScore += 2;
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnSecondThird()) //6
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); htScore += 2;
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_BasesLoaded()) //7
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); htScore += 3;
                HTScoreTX.Text = htScore.ToString();
            }
        }

        public static void homeHROccurred()
        {
            if (scenario_NoOneOn())
            {
                htScore++; HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnFirst()) //1
            {
                htScore += 2; HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnSecond()) //2
            {
                htScore += 2; HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnThird()) //3
            {
                htScore += 2; HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnFirstSecond()) //4
            {
                htScore += 3; HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnFirstThird()) //5
            {
                htScore += 3; HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnSecondThird()) //6
            {
                htScore += 3; HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_BasesLoaded()) //7
            {
                htScore += 4; HTScoreTX.Text = htScore.ToString();
            }
            clearBases();
        }

        public static void homeGroundOut()
        {
            if (scenario_OnFirst())
            {
                Black_FirstBase(); Black_SecondBase(); Black_ThirdBase(); outCount += 2;
            }
            else if (scenario_OnFirstSecond())
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); outCount += 2;
            }
            else if (scenario_OnFirstThird())
            {
                Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); outCount += 2;
            }
            else if (scenario_BasesLoaded())
            {
                Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); outCount += 2;
            }
            else
            {
                outCount++;
            }
        }

        public static void homeWalk()
        {
            if (scenario_NoOneOn()) //0
            {
                Green_FirstBase(); Black_SecondBase(); Black_ThirdBase();
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnFirst()) //1
            {
                Green_FirstBase(); Green_SecondBase(); Black_ThirdBase();
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnSecond()) //2
            {
                Green_FirstBase(); Green_SecondBase(); Black_ThirdBase();
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnThird()) //3
            {
                Green_FirstBase(); Black_SecondBase(); Green_ThirdBase();
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnFirstSecond()) //4
            {
                Green_FirstBase(); Green_SecondBase(); Green_ThirdBase();
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnFirstThird()) //5
            {
                Green_FirstBase(); Green_SecondBase(); Green_ThirdBase();
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_OnSecondThird()) //6
            {
                Green_FirstBase(); Green_SecondBase(); Green_ThirdBase();
                HTScoreTX.Text = htScore.ToString();
            }
            else if (scenario_BasesLoaded()) //7
            {
                Green_FirstBase(); Green_SecondBase(); Green_ThirdBase(); htScore++;
                HTScoreTX.Text = htScore.ToString();
            }
        }

        public static void whenFoulOccurs()
        {
            strikeCount++;
            if (strikeCount == 1)
                s1.Fill = new SolidColorBrush(Colors.Orange);
            else if (strikeCount == 2)
            {
                s1.Fill = new SolidColorBrush(Colors.Orange);
                s2.Fill = new SolidColorBrush(Colors.Orange);
            }
            else if (strikeCount == 3)
            {
                strikeCount--;
                s1.Fill = new SolidColorBrush(Colors.Orange);
                s2.Fill = new SolidColorBrush(Colors.Orange);
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
            else if (outCount == 3)
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
            else if (strikeCount == 3)
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
            else if (ballCount == 4)
            {
                clearStrikesBalls();
            }
        }

        public void MStrike_Click(object sender, RoutedEventArgs e)
        {
            important = 0;
            if (inn % 2 != 0)
            {
                TheAwayAlgorithm();
                currentlyPText.Text = " " + currentHomePitcher;
                homePitchesThrown++;
                pitchesThrownText.Text = " " + homePitchesThrown.ToString();
                atBatText.Text = " " + awayTeamBattersX[GetABO()].ToString();
                onDeckText.Text = " " + awayTeamBattersX[GetABO1()].ToString();
                inTheHoleText.Text = " " + awayTeamBattersX[GetABO2()].ToString();
                TBInd.Text = "TOP ";
                inn2 = inn;
                inn2 = Math.Ceiling(inn2 / 2);
                InnTB.Text = " " + inn2.ToString();
            }
            else
            {
                TheHomeAlgorithm();
                currentlyPText.Text = " " + currentAwayPitcher;
                awayPitchesThrown++;
                pitchesThrownText.Text = " " + awayPitchesThrown.ToString();
                atBatText.Text = " " + homeTeamBattersX[GetHBO()].ToString();
                onDeckText.Text = " " + homeTeamBattersX[GetHBO1()].ToString();
                inTheHoleText.Text = " " + homeTeamBattersX[GetHBO2()].ToString();
                TBInd.Text = "BOT ";
                inn2 = inn;
                inn2 = Math.Ceiling(inn2 / 2);
                InnTB.Text = " " + inn2.ToString();
            }
        }
        public void BStrike_Click(object sender, RoutedEventArgs e)
        {
            important = 1;
            if (inn % 2 != 0)
            {
                TheAwayAlgorithm();
                currentlyPText.Text = " " + currentHomePitcher;
                homePitchesThrown++;
                pitchesThrownText.Text = " " + homePitchesThrown.ToString();
                atBatText.Text = " " + awayTeamBattersX[GetABO()].ToString();
                onDeckText.Text = " " + awayTeamBattersX[GetABO1()].ToString();
                inTheHoleText.Text = " " + awayTeamBattersX[GetABO2()].ToString();
                TBInd.Text = "TOP ";
                inn2 = inn;
                inn2 = Math.Ceiling(inn2 / 2);
                InnTB.Text = " " + inn2.ToString();
            }
            else
            {
                TheHomeAlgorithm();
                currentlyPText.Text = " " + currentAwayPitcher;
                awayPitchesThrown++;
                pitchesThrownText.Text = " " + awayPitchesThrown.ToString();
                atBatText.Text = " " + homeTeamBattersX[GetHBO()].ToString();
                onDeckText.Text = " " + homeTeamBattersX[GetHBO1()].ToString();
                inTheHoleText.Text = " " + homeTeamBattersX[GetHBO2()].ToString();
                TBInd.Text = "BOT ";
                inn2 = inn;
                inn2 = Math.Ceiling(inn2 / 2);
                InnTB.Text = " " + inn2.ToString();
            }
        }
        public void TStrike_Click(object sender, RoutedEventArgs e)
        {
            important = 2;
            if (inn % 2 != 0)
            {
                TheAwayAlgorithm();
                currentlyPText.Text = " " + currentHomePitcher;
                homePitchesThrown++;
                pitchesThrownText.Text = " " + homePitchesThrown.ToString();
                atBatText.Text = " " + awayTeamBattersX[GetABO()].ToString();
                onDeckText.Text = " " + awayTeamBattersX[GetABO1()].ToString();
                inTheHoleText.Text = " " + awayTeamBattersX[GetABO2()].ToString();
                TBInd.Text = "TOP ";
                inn2 = inn;
                inn2 = Math.Ceiling(inn2 / 2);
                InnTB.Text = " " + inn2.ToString();
            }
            else
            {
                TheHomeAlgorithm();
                currentlyPText.Text = " " + currentAwayPitcher;
                awayPitchesThrown++;
                pitchesThrownText.Text = " " + awayPitchesThrown.ToString();
                atBatText.Text = " " + homeTeamBattersX[GetHBO()].ToString();
                onDeckText.Text = " " + homeTeamBattersX[GetHBO1()].ToString();
                inTheHoleText.Text = " " + homeTeamBattersX[GetHBO2()].ToString();
                TBInd.Text = "BOT ";
                inn2 = inn;
                inn2 = Math.Ceiling(inn2 / 2);
                InnTB.Text = " " + inn2.ToString();
            }
        }
        public void LStrike_Click(object sender, RoutedEventArgs e)
        {
            important = 3;
            if (inn % 2 != 0)
            {
                TheAwayAlgorithm();
                currentlyPText.Text = " " + currentHomePitcher;
                homePitchesThrown++;
                pitchesThrownText.Text = " " + homePitchesThrown.ToString();
                atBatText.Text = " " + awayTeamBattersX[GetABO()].ToString();
                onDeckText.Text = " " + awayTeamBattersX[GetABO1()].ToString();
                inTheHoleText.Text = " " + awayTeamBattersX[GetABO2()].ToString();
                TBInd.Text = "TOP ";
                inn2 = inn;
                inn2 = Math.Ceiling(inn2 / 2);
                InnTB.Text = " " + inn2.ToString();
            }
            else
            {
                TheHomeAlgorithm();
                currentlyPText.Text = " " + currentAwayPitcher;
                awayPitchesThrown++;
                pitchesThrownText.Text = " " + awayPitchesThrown.ToString();
                atBatText.Text = " " + homeTeamBattersX[GetHBO()].ToString();
                onDeckText.Text = " " + homeTeamBattersX[GetHBO1()].ToString();
                inTheHoleText.Text = " " + homeTeamBattersX[GetHBO2()].ToString();
                TBInd.Text = "BOT ";
                inn2 = inn;
                inn2 = Math.Ceiling(inn2 / 2);
                InnTB.Text = " " + inn2.ToString();
            }
        }
        public void RStrike_Click(object sender, RoutedEventArgs e)
        {
            important = 4;
            if (inn % 2 != 0)
            {
                TheAwayAlgorithm();
                currentlyPText.Text = " " + currentHomePitcher;
                homePitchesThrown++;
                pitchesThrownText.Text = " " + homePitchesThrown.ToString();
                atBatText.Text = " " + awayTeamBattersX[GetABO()].ToString();
                onDeckText.Text = " " + awayTeamBattersX[GetABO1()].ToString();
                inTheHoleText.Text = " " + awayTeamBattersX[GetABO2()].ToString();
                TBInd.Text = "TOP ";
                inn2 = inn;
                inn2 = Math.Ceiling(inn2 / 2);
                InnTB.Text = " " + inn2.ToString();
            }
            else
            {
                TheHomeAlgorithm();
                currentlyPText.Text = " " + currentAwayPitcher;
                awayPitchesThrown++;
                pitchesThrownText.Text = " " + awayPitchesThrown.ToString();
                atBatText.Text = " " + homeTeamBattersX[GetHBO()].ToString();
                onDeckText.Text = " " + homeTeamBattersX[GetHBO1()].ToString();
                inTheHoleText.Text = " " + homeTeamBattersX[GetHBO2()].ToString();
                TBInd.Text = "BOT ";
                inn2 = inn;
                inn2 = Math.Ceiling(inn2 / 2);
                InnTB.Text = " " + inn2.ToString();
            }
        }

        public void Ball_Click(object sender, RoutedEventArgs e)
        {
            important = 5;
            if (inn % 2 != 0)
            {
                TheAwayAlgorithm();
                currentlyPText.Text = " " + currentHomePitcher;
                homePitchesThrown++;
                pitchesThrownText.Text = " " + homePitchesThrown.ToString();
                atBatText.Text = " " + awayTeamBattersX[GetABO()].ToString();
                onDeckText.Text = " " + awayTeamBattersX[GetABO1()].ToString();
                inTheHoleText.Text = " " + awayTeamBattersX[GetABO2()].ToString();
                TBInd.Text = "TOP ";
                inn2 = inn;
                inn2 = Math.Ceiling(inn2 / 2);
                InnTB.Text = " " + inn2.ToString();
            }
            else
            {
                TheHomeAlgorithm();
                currentlyPText.Text = " " + currentAwayPitcher;
                awayPitchesThrown++;
                pitchesThrownText.Text = " " + awayPitchesThrown.ToString();
                atBatText.Text = " " + homeTeamBattersX[GetHBO()].ToString();
                onDeckText.Text = " " + homeTeamBattersX[GetHBO1()].ToString();
                inTheHoleText.Text = " " + homeTeamBattersX[GetHBO2()].ToString();
                TBInd.Text = "BOT ";
                inn2 = inn;
                inn2 = Math.Ceiling(inn2 / 2);
                InnTB.Text = " " + inn2.ToString();
            }
        }

        public static int GetABO()
        {
            if (awayBatterOrder < 9)
                return awayBatterOrder;
            else
                awayBatterOrder = 0;
            return awayBatterOrder;
        }
        public static int GetHBO()
        {
            if (homeBatterOrder < 9)
                return homeBatterOrder;
            else
                homeBatterOrder = 0;
            return homeBatterOrder;
        }
        public static int GetABO1()
        {
            if (awayBatterOrder + 1 < 9)
                return awayBatterOrder + 1;
            else
                return 0;
        }
        public static int GetHBO1()
        {
            if (homeBatterOrder + 1 < 9)
                return homeBatterOrder + 1;
            else
                return 0; 
        }
        public static int GetABO2()
        {
            if (awayBatterOrder + 2 < 9)
                return awayBatterOrder + 2;
            else if (awayBatterOrder + 2 < 10)
                return 0;
            else
                return 1;
        }
        public static int GetHBO2()
        {
            if (homeBatterOrder + 2 < 9)
                return homeBatterOrder + 2;
            else if (homeBatterOrder + 2 < 10)
                return 0;
            else
                return 1;
        }

        public async static void TheAwayAlgorithm()
        {
            if (outCount < 3)
            {
                if (important.Equals(0))
                {
                    int midS1 = midSRandom();
                    if (midS1 < 29)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a " + generateAwayHitType();
                        addActionToGameLog();
                        clearStrikesBalls();
                        awayBatterOrder++;
                    }
                    else if (midS1 > 28 && midS1 < 34)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a groundout.";
                        addActionToGameLog();
                        awayGroundOut(); whenOutOccurs(); clearStrikesBalls();
                        awayBatterOrder++;
                    }
                    else if (midS1 > 33 && midS1 < 39)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a flyout.";
                        addActionToGameLog();
                        outCount++; whenOutOccurs(); clearStrikesBalls();
                        awayBatterOrder++;
                    }
                    else if (midS1 > 38 && midS1 < 40)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a foul ball.";
                        addActionToGameLog();
                        whenFoulOccurs();
                    }
                    else if (midS1 > 39 && midS1 < 50)
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            awayBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " swings and misses.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                    else
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            awayBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " looks at a strike.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                }
                else if (important.Equals(1))
                {
                    int botS1 = botSRandom();
                    if (botS1 < 16)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a " + generateAwayHitType();
                        addActionToGameLog();
                        clearStrikesBalls();
                        awayBatterOrder++;
                    }
                    else if (botS1 > 15 && botS1 < 19)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a groundout.";
                        addActionToGameLog();
                        awayGroundOut(); whenOutOccurs(); clearStrikesBalls();
                        awayBatterOrder++;
                    }
                    else if (botS1 > 18 && botS1 < 34)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a flyout.";
                        addActionToGameLog();
                        outCount++; whenOutOccurs(); clearStrikesBalls();
                        awayBatterOrder++;
                    }
                    else if (botS1 > 33 && botS1 < 39)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a foul ball.";
                        addActionToGameLog();
                        whenFoulOccurs();
                    }
                    else if (botS1 > 38 && botS1 < 47)
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            awayBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " swings and misses.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                    else
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            awayBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " looks at a strike.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                }
                else if (important.Equals(2))
                {
                    int topS1 = topSRandom();
                    if (topS1 < 13)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a " + generateAwayHitType();
                        addActionToGameLog();
                        clearStrikesBalls();
                        awayBatterOrder++;
                    }
                    else if (topS1 > 12 && topS1 < 23)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a groundout.";
                        addActionToGameLog();
                        awayGroundOut(); whenOutOccurs(); clearStrikesBalls();
                        awayBatterOrder++;
                    }
                    else if (topS1 > 22 && topS1 < 31)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a flyout.";
                        addActionToGameLog();
                        outCount++; whenOutOccurs(); clearStrikesBalls();
                        awayBatterOrder++;
                    }
                    else if (topS1 > 30 && topS1 < 43)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a foul ball.";
                        addActionToGameLog();
                        whenFoulOccurs();
                    }
                    else if (topS1 > 42 && topS1 < 49)
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            awayBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " swings and misses.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                    else
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            awayBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " looks at a strike.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                }
                else if (important.Equals(3))
                {
                    int leftS1 = leftSRandom();
                    if (leftS1 < 11)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a " + generateAwayHitType();
                        addActionToGameLog();
                        clearStrikesBalls();
                        awayBatterOrder++;
                    }
                    else if (leftS1 > 10 && leftS1 < 21)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a groundout.";
                        addActionToGameLog();
                        awayGroundOut(); whenOutOccurs(); clearStrikesBalls();
                        awayBatterOrder++;
                    }
                    else if (leftS1 > 20 && leftS1 < 26)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a flyout.";
                        addActionToGameLog();
                        outCount++; whenOutOccurs(); clearStrikesBalls();
                        awayBatterOrder++;
                    } 
                    else if (leftS1 > 25 && leftS1 < 36)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a foul ball.";
                        addActionToGameLog();
                        whenFoulOccurs();
                    }
                    else if (leftS1 > 35 && leftS1 < 46)
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            awayBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " swings and misses.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                    else
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            awayBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " looks at a strike.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                }
                else if (important.Equals(4))
                {
                    int rightS1 = rightSRandom();
                    if (rightS1 < 11)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a " + generateAwayHitType();
                        addActionToGameLog();
                        clearStrikesBalls();
                        awayBatterOrder++;
                    }
                    else if (rightS1 > 10 && rightS1 < 21)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a groundout.";
                        addActionToGameLog();
                        awayGroundOut(); whenOutOccurs(); clearStrikesBalls();
                        awayBatterOrder++;
                    }
                    else if (rightS1 > 20 && rightS1 < 26)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a flyout.";
                        addActionToGameLog();
                        outCount++; whenOutOccurs(); clearStrikesBalls();
                        awayBatterOrder++;
                    }
                    else if (rightS1 > 25 && rightS1 < 36)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a foul ball.";
                        addActionToGameLog();
                        whenFoulOccurs();
                    }
                    else if (rightS1 > 35 && rightS1 < 46)
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            awayBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " swings and misses.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                    else
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            awayBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " looks at a strike.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                }
                else if (important.Equals(5))
                {
                    int ballQ = BallButtonRandom();
                    if (ballQ < 4)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a " + generateAwayHitType();
                        addActionToGameLog();
                        clearStrikesBalls();
                        awayBatterOrder++;
                    }
                    else if (ballQ > 3 && ballQ < 8)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a groundout.";
                        addActionToGameLog();
                        awayGroundOut(); whenOutOccurs(); clearStrikesBalls();
                        awayBatterOrder++;
                    }
                    else if (ballQ > 7 && ballQ < 12)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a flyout.";
                        addActionToGameLog();
                        outCount++; whenOutOccurs(); clearStrikesBalls();
                        awayBatterOrder++;
                    }
                    else if (ballQ > 11 && ballQ < 17)
                    {
                        actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " hits a foul ball.";
                        addActionToGameLog();
                        whenFoulOccurs();
                    }
                    else if (ballQ > 16 && ballQ < 38)
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            awayBatterOrder++;
                        }
                        else 
                        { 
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " swings and misses.";
                            addActionToGameLog();
                            whenStrikeOccurs(); 
                        }
                    }  
                    else
                    {
                        ballCount++;
                        if (ballCount == 4)
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " walked";
                            addActionToGameLog();
                            awayWalk(); clearStrikesBalls();
                            awayBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = awayTeamBattersX[GetABO()].ToString() + " looks at a ball.";
                            addActionToGameLog();
                            whenBallOccurs(); 
                        }
                    }
                }
            }
            else
            {
                inn++;
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, "UPDATE- INN: " + TBInd.Text + inn2.ToString() + ", " + SetRosters.getATName().Substring(0, 3) + " " + atScore.ToString() + ": " + SetRosters.getHTName().Substring(0, 3) + ": " + htScore.ToString() + "\n");
                homePitchesThrown--;
                clearShapes();
            }
        }

        public async static void TheHomeAlgorithm()
        {
            if (outCount < 3)
            {
                if (important.Equals(0))
                {
                    int midS1 = midSRandom();
                    if (midS1 < 29)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a " + generateHomeHitType();
                        addActionToGameLog();
                        clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (midS1 > 28 && midS1 < 34)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a groundout.";
                        addActionToGameLog();
                        homeGroundOut(); whenOutOccurs(); clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (midS1 > 33 && midS1 < 39)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a flyout.";
                        addActionToGameLog();
                        outCount++; whenOutOccurs(); clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (midS1 > 38 && midS1 < 40)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a foul ball.";
                        addActionToGameLog();
                        whenFoulOccurs();
                    }
                    else if (midS1 > 39 && midS1 < 50)
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            homeBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " swings and misses.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                    else
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            homeBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " looks at a strike.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                }
                else if (important.Equals(1))
                {
                    int botS1 = botSRandom();
                    if (botS1 < 16)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a " + generateHomeHitType();
                        addActionToGameLog();
                        clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (botS1 > 15 && botS1 < 19)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a groundout.";
                        addActionToGameLog();
                        homeGroundOut(); whenOutOccurs(); clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (botS1 > 18 && botS1 < 34)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a flyout.";
                        addActionToGameLog();
                        outCount++; whenOutOccurs(); clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (botS1 > 33 && botS1 < 39)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a foul ball.";
                        addActionToGameLog();
                        whenFoulOccurs();
                    }
                    else if (botS1 > 38 && botS1 < 47)
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            homeBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " swings and misses.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                    else
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            homeBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " looks at a strike.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                }
                else if (important.Equals(2))
                {
                    int topS1 = topSRandom();
                    if (topS1 < 13)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a " + generateHomeHitType();
                        addActionToGameLog();
                        clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (topS1 > 12 && topS1 < 23)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a groundout.";
                        addActionToGameLog();
                        homeGroundOut(); whenOutOccurs(); clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (topS1 > 22 && topS1 < 31)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a flyout.";
                        addActionToGameLog();
                        outCount++; whenOutOccurs(); clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (topS1 > 30 && topS1 < 43)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a foul ball.";
                        addActionToGameLog();
                        whenFoulOccurs();
                    }
                    else if (topS1 > 42 && topS1 < 49)
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            homeBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " swings and misses.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                    else
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            homeBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " looks at a strike.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                }
                else if (important.Equals(3))
                {
                    int leftS1 = leftSRandom();
                    if (leftS1 < 11)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a " + generateHomeHitType();
                        addActionToGameLog();
                        clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (leftS1 > 10 && leftS1 < 21)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a groundout.";
                        addActionToGameLog();
                        homeGroundOut(); whenOutOccurs(); clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (leftS1 > 20 && leftS1 < 26)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a flyout.";
                        addActionToGameLog();
                        outCount++; whenOutOccurs(); clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (leftS1 > 25 && leftS1 < 36)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a foul ball.";
                        addActionToGameLog();
                        whenFoulOccurs();
                    }
                    else if (leftS1 > 35 && leftS1 < 46)
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            homeBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " swings and misses.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                    else
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            homeBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " looks at a strike.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                }
                else if (important.Equals(4))
                {
                    int rightS1 = rightSRandom();
                    if (rightS1 < 11)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a " + generateHomeHitType();
                        addActionToGameLog();
                        clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (rightS1 > 10 && rightS1 < 21)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a groundout.";
                        addActionToGameLog();
                        homeGroundOut(); whenOutOccurs(); clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (rightS1 > 20 && rightS1 < 26)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a flyout.";
                        addActionToGameLog();
                        outCount++; whenOutOccurs(); clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (rightS1 > 25 && rightS1 < 36)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a foul ball.";
                        addActionToGameLog();
                        whenFoulOccurs();
                    }
                    else if (rightS1 > 35 && rightS1 < 46)
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            homeBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " swings and misses.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                    else
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            homeBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " looks at a strike.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                }
                else if (important.Equals(5))
                {
                    int ballQ = BallButtonRandom();
                    if (ballQ < 4)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a " + generateHomeHitType();
                        addActionToGameLog();
                        clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (ballQ > 3 && ballQ < 8)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a groundout.";
                        addActionToGameLog();
                        homeGroundOut(); whenOutOccurs(); clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (ballQ > 7 && ballQ < 12)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a flyout.";
                        addActionToGameLog();
                        outCount++; whenOutOccurs(); clearStrikesBalls();
                        homeBatterOrder++;
                    }
                    else if (ballQ > 11 && ballQ < 17)
                    {
                        actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " hits a foul ball.";
                        addActionToGameLog();
                        whenFoulOccurs();
                    }
                    else if (ballQ > 16 && ballQ < 38)
                    {
                        strikeCount++;
                        if (strikeCount == 3)
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " struck out.";
                            addActionToGameLog();
                            outCount++; whenOutOccurs(); clearStrikesBalls();
                            homeBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " swings and misses.";
                            addActionToGameLog();
                            whenStrikeOccurs();
                        }
                    }
                    else
                    {
                        ballCount++;
                        if (ballCount == 4)
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " walked.";
                            addActionToGameLog();
                            homeWalk(); clearStrikesBalls();
                            homeBatterOrder++;
                        }
                        else
                        {
                            actionBarX.Text = homeTeamBattersX[GetHBO()].ToString() + " looks at a ball.";
                            addActionToGameLog();
                            whenBallOccurs();
                        }
                    }
                }
            }
            else
            {
                inn++;
                await Windows.Storage.FileIO.AppendTextAsync(gameLog, "UPDATE- INN: " + TBInd.Text + inn2.ToString() + ", " + SetRosters.getATName().Substring(0, 3) + " " + atScore.ToString() + ": " + SetRosters.getHTName().Substring(0, 3) + ": " + htScore.ToString() + "\n");
                awayPitchesThrown--;
                clearShapes();
            }
        }
    } 
}