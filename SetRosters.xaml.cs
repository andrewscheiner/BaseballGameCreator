using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BaseballGameCreator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SetRosters : Page
    {
        public static string awayTeamName;
        public static string homeTeamName;

        public static ArrayList awayTeamBatters = new ArrayList();
        public static ArrayList homeTeamBatters = new ArrayList();
        public static ArrayList awayTeamPBE = new ArrayList();
        public static ArrayList homeTeamPBE = new ArrayList();

        public SetRosters()
        {
            this.InitializeComponent();
        }

        public void saveLineups()
        {
            //Teams
            if (ATName.Text == "")
            {
                awayTeamName = "Away Team";
            }
            else
            {
				awayTeamName = ATName.Text;
			}

			if (HTName.Text == "")
			{
				homeTeamName = "Home Team";
			}
			else
			{
				homeTeamName = HTName.Text;
			}
			

            // Away batters
            if (ATBatter1.Text == "")
            {
                awayTeamBatters.Add("Away Leadoff Hitter");
            }
            else
            {
                awayTeamBatters.Add(ATBatter1.Text);
            }

            if (ATBatter2.Text == "")
            {
                awayTeamBatters.Add("Away Batter 2");
            }
            else
            {
                awayTeamBatters.Add(ATBatter2.Text);
            }

            if (ATBatter3.Text == "")
            {
                awayTeamBatters.Add("Away Batter 3");
            }
            else
            {
                awayTeamBatters.Add(ATBatter3.Text);
            }

            if (ATBatter4.Text == "")
            {
                awayTeamBatters.Add("Away Cleanup Hitter");
            }
            else
            {
                awayTeamBatters.Add(ATBatter4.Text);
            }

            if (ATBatter5.Text == "")
            {
                awayTeamBatters.Add("Away Batter 5");
            }
            else
            {
                awayTeamBatters.Add(ATBatter5.Text);
            }

            if (ATBatter6.Text == "")
            {
                awayTeamBatters.Add("Away Batter 6");
            }
            else
            {
                awayTeamBatters.Add(ATBatter6.Text);
            }

            if (ATBatter7.Text == "")
            {
                awayTeamBatters.Add("Away Batter 7");
            }
            else
            {
                awayTeamBatters.Add(ATBatter7.Text);
            }

            if (ATBatter8.Text == "")
            {
                awayTeamBatters.Add("Away Batter 8");
            }
            else
            {
                awayTeamBatters.Add(ATBatter8.Text);
            }

            if (ATBatter9.Text == "")
            {
                awayTeamBatters.Add("Away Batter 9");
            }
            else
            {
                awayTeamBatters.Add(ATBatter9.Text);
            }

            // Home batters
            if (HTBatter1.Text == "")
            {
                homeTeamBatters.Add("Home Leadoff Hitter");
            }
            else
            {
                homeTeamBatters.Add(HTBatter1.Text);
            }

            if (HTBatter2.Text == "")
            {
                homeTeamBatters.Add("Home Batter 2");
            }
            else
            {
                homeTeamBatters.Add(HTBatter2.Text);
            }

            if (HTBatter3.Text == "")
            {
                homeTeamBatters.Add("Home Batter 3");
            }
            else
            {
                homeTeamBatters.Add(HTBatter3.Text);
            }

            if (HTBatter4.Text == "")
            {
                homeTeamBatters.Add("Home Cleanup Hitter");
            }
            else
            {
                homeTeamBatters.Add(HTBatter4.Text);
            }

            if (HTBatter5.Text == "")
            {
                homeTeamBatters.Add("Home Batter 5");
            }
            else
            {
                homeTeamBatters.Add(HTBatter5.Text);
            }

            if (HTBatter6.Text == "")
            {
                homeTeamBatters.Add("Home Batter 6");
            }
            else
            {
                homeTeamBatters.Add(HTBatter6.Text);
            }

            if (HTBatter7.Text == "")
            {
                homeTeamBatters.Add("Home Batter 7");
            }
            else
            {
                homeTeamBatters.Add(HTBatter7.Text);
            }

            if (HTBatter8.Text == "")
            {
                homeTeamBatters.Add("Home Batter 8");
            }
            else
            {
                homeTeamBatters.Add(HTBatter8.Text);
            }

            if (HTBatter9.Text == "")
            {
                homeTeamBatters.Add("Home Batter 9");
            }
            else
            {
                homeTeamBatters.Add(HTBatter9.Text);
            }

            // Away pitchers + bench (PBE)
            if (ATSPitcher.Text == "")
            {
                awayTeamPBE.Add("Away SP");
            }
            else
            {
                awayTeamPBE.Add(ATSPitcher.Text);
            }

            if (ATRPitcher1.Text == "")
            {
                awayTeamPBE.Add("Away RP 1");
            }
            else
            {
                awayTeamPBE.Add(ATRPitcher1.Text);
            }

            if (ATRPitcher2.Text == "")
            {
                awayTeamPBE.Add("Away RP 2");
            }
            else
            {
                awayTeamPBE.Add(ATRPitcher2.Text);
            }

            if (ATRPitcher3.Text == "")
            {
                awayTeamPBE.Add("Away RP 3");
            }
            else
            {
                awayTeamPBE.Add(ATRPitcher3.Text);
            }

            if (ATSetupMan.Text == "")
            {
                awayTeamPBE.Add("Away Setup Man");
            }
            else
            {
                awayTeamPBE.Add(ATSetupMan.Text);
            }

            if (ATCloser.Text == "")
            {
                awayTeamPBE.Add("Away Closer");
            }
            else
            {
                awayTeamPBE.Add(ATCloser.Text);
            }

            if (ATBench1.Text == "")
            {
                awayTeamPBE.Add("Away Bench 1");
            }
            else
            {
                awayTeamPBE.Add(ATBench1.Text);
            }

            if (ATBench2.Text == "")
            {
                awayTeamPBE.Add("Away Bench 2");
            }
            else
            {
                awayTeamPBE.Add(ATBench2.Text);
            }

            if (ATBench3.Text == "")
            {
                awayTeamPBE.Add("Away Bench 3");
            }
            else
            {
                awayTeamPBE.Add(ATBench3.Text);
            }

            // Home pitchers + bench (PBE)
            if (HTSPitcher.Text == "")
            {
                homeTeamPBE.Add("Home SP");
            }
            else
            {
                homeTeamPBE.Add(HTSPitcher.Text);
            }

            if (HTRPitcher1.Text == "")
            {
                homeTeamPBE.Add("Home RP 1");
            }
            else
            {
                homeTeamPBE.Add(HTRPitcher1.Text);
            }

            if (HTRPitcher2.Text == "")
            {
                homeTeamPBE.Add("Home RP 2");
            }
            else
            {
                homeTeamPBE.Add(HTRPitcher2.Text);
            }

            if (HTRPitcher3.Text == "")
            {
                homeTeamPBE.Add("Home RP 3");
            }
            else
            {
                homeTeamPBE.Add(HTRPitcher3.Text);
            }

            if (HTSetupMan.Text == "")
            {
                homeTeamPBE.Add("Home Setup Man");
            }
            else
            {
                homeTeamPBE.Add(HTSetupMan.Text);
            }

            if (HTCloser.Text == "")
            {
                homeTeamPBE.Add("Home Closer");
            }
            else
            {
                homeTeamPBE.Add(HTCloser.Text);
            }

            if (HTBench1.Text == "")
            {
                homeTeamPBE.Add("Home Bench 1");
            }
            else
            {
                homeTeamPBE.Add(HTBench1.Text);
            }

            if (HTBench2.Text == "")
            {
                homeTeamPBE.Add("Home Bench 2");
            }
            else
            {
                homeTeamPBE.Add(HTBench2.Text);
            }

            if (HTBench3.Text == "")
            {
                homeTeamPBE.Add("Home Bench 3");
            }
            else
            {
                homeTeamPBE.Add(HTBench3.Text);
            }
        }

        public void nextScreen_Click(object sender, RoutedEventArgs e)
        {
            saveLineups();
            this.Frame.Navigate(typeof(BaseballGameScreen));
        }

        public static string getATName()
        {
            return awayTeamName;
        }

        public static string getHTName()
        {
            return homeTeamName;
        }
        public static ArrayList getATBatters()
        {
            return awayTeamBatters;
        }
        public static ArrayList getHTBatters()
        {
            return homeTeamBatters;
        }
        // PBE = Pitchers + Bench
        public static ArrayList getATPBE()
        {
            return awayTeamPBE;
        }
        public static ArrayList getHTPBE()
        {
            return homeTeamPBE;
        }
    }
}
