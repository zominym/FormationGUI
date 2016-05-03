using System;
using Gtk;
using TrainingProblem;
using System.Collections.Generic;
using System.IO;

namespace GUI
{
	class MainClass
	{

		public static string agenciesFile = "";
		public static string citiesFile = "";
		public static List<Agency> agencies = new List<Agency>();
		public static List<City> cities = new List<City>();
		public static MainWindow win;
		public static Random rand = new Random();
		static public double[,] distTab;
		public static bool writeToCSV = true;

		public static void Main (string[] args)
		{
			Application.Init ();
			win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}

		public static int loadAgencies()
		{
			City.ID = cities.Count;
			int count = 0;
			List<Agency> _agencies = new List<Agency>();

			StreamReader fs = new StreamReader(agenciesFile);

			String line = fs.ReadLine();

			while((line = fs.ReadLine()) != null)
			{
				_agencies.Add(new Agency(line));
				count++;
			}

			fs.Close();

			agencies = _agencies;
			return count;
		}

		public static int loadCities()
		{
			City.ID = 0;
			int count = 0;
			List<City> _cities = new List<City>();
			StreamReader fs = new StreamReader(citiesFile);

			String line = fs.ReadLine();

			while((line = fs.ReadLine()) != null)
			{
				_cities.Add(new City(line));
				count++;
			}

			fs.Close();

			cities = _cities;
			if (agencies.Count > 0)
				loadAgencies();
			return count;
		}

		public static void initDistTab() {
			if (cities.Count <= 0 || agencies.Count <= 0)
				return ;
			distTab = new double[cities.Count + agencies.Count, cities.Count + agencies.Count];
			for (int i = 0; i < cities.Count + agencies.Count; i++)
				for (int j = 0; j < cities.Count + agencies.Count; j++)
					distTab[i, j] = -1;
		}

		public static string iterToString(int iter, double best, double current, int centers) {
			return "ITER : " + iter.ToString("0000000") + "    BEST : " + best.ToString("00000000") + "    CURRENT : " + current.ToString("00000000") + "    CENTERS : " + centers.ToString("000");
		}

		public static List<Agency> getAgencies() {
			return agencies;
		}

		public static List<City> getCities() {
			return cities;
		}

		public static bool checkstart() {
			try {
				MainClass.initDistTab();
				Directory.CreateDirectory ("outputs");
			} catch (Exception) {
				return false;
			}

			if (cities.Count <= 0 || agencies.Count <= 0)
				return false;
			return true;
		}
	}
}
