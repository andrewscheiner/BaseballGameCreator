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
            awayTeamName = ATName.Text;
            homeTeamName = HTName.Text;

            awayTeamBatters.Add(ATBatter1.Text);
            awayTeamBatters.Add(ATBatter2.Text);
            awayTeamBatters.Add(ATBatter3.Text);
            awayTeamBatters.Add(ATBatter4.Text);
            awayTeamBatters.Add(ATBatter5.Text);
            awayTeamBatters.Add(ATBatter6.Text);
            awayTeamBatters.Add(ATBatter7.Text);
            awayTeamBatters.Add(ATBatter8.Text);
            awayTeamBatters.Add(ATBatter9.Text);

            homeTeamBatters.Add(HTBatter1.Text);
            homeTeamBatters.Add(HTBatter2.Text);
            homeTeamBatters.Add(HTBatter3.Text);
            homeTeamBatters.Add(HTBatter4.Text);
            homeTeamBatters.Add(HTBatter5.Text);
            homeTeamBatters.Add(HTBatter6.Text);
            homeTeamBatters.Add(HTBatter7.Text);
            homeTeamBatters.Add(HTBatter8.Text);
            homeTeamBatters.Add(HTBatter9.Text);

            awayTeamPBE.Add(ATSPitcher.Text);
            awayTeamPBE.Add(ATRPitcher1.Text);
            awayTeamPBE.Add(ATRPitcher2.Text);
            awayTeamPBE.Add(ATRPitcher3.Text);
            awayTeamPBE.Add(ATSetupMan.Text);
            awayTeamPBE.Add(ATCloser.Text);
            awayTeamPBE.Add(ATBench1.Text);
            awayTeamPBE.Add(ATBench2.Text);
            awayTeamPBE.Add(ATBench3.Text);

            homeTeamPBE.Add(HTSPitcher.Text);
            homeTeamPBE.Add(HTRPitcher1.Text);
            homeTeamPBE.Add(HTRPitcher2.Text);
            homeTeamPBE.Add(HTRPitcher3.Text);
            homeTeamPBE.Add(HTSetupMan.Text);
            homeTeamPBE.Add(HTCloser.Text);
            homeTeamPBE.Add(HTBench1.Text);
            homeTeamPBE.Add(HTBench2.Text);
            homeTeamPBE.Add(HTBench3.Text);
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
