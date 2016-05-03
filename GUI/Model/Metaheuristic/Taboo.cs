using System;
using TrainingProblem;
using System.Collections.Generic;
using System.IO;
using GUI;
using Gtk;

namespace Metaheuristic
{
	public class Taboo
	{

		StreamWriter sw;
		Random rand = GUI.MainClass.rand;


		public Taboo()
		{
			sw = new StreamWriter(File.Create("outputs/" + DateTime.Now.ToString("ITERS dd_mm_yy HH-mm-ss") + ".csv"));
		}

		public Solution run(int nbIter) {
			int nbVoisins = GUI.MainClass.getAgencies().Count;
			Solution min = new Solution("peu de centres");
			Solution s = min;
			sw.WriteLine("\"cout\";\"distanceTotale\";\"nbcentres\"");
			sw.WriteLine(s.toCSVShort());
            
            for (int i = 0; i < nbIter; i++) {
				s = visit(s);
				if (s.Cost < min.Cost)
					min = s;


				sw.WriteLine(s.toCSVShort());
				MainClass.win.writeLine(GUI.MainClass.iterToString(i, s.Cost, min.Cost, s.getUsedCities().Count), true);
				MainClass.win.setProgress ((double)i / (double)nbIter);
				while (Gtk.Application.EventsPending())
					Gtk.Application.RunIteration();
			}
			sw.Close();
			return s;
		}

		public Solution visit2(Solution s) {
			Solution min = null;
			for (int i = 0; i < s._tuples.Length; i++) {
				Solution sp = s.mutate2(i);
				if (i == 0)
					min = sp;
				if (sp.Cost < min.Cost)
					min = sp;
			}
			return min;
		}

		public Solution visit(Solution s) {
			Solution min = null;
			List<City> gUC = s.getUsedCities();
			for (int i = 0; i < s._tuples.Length; i++) {
				Solution sp = s.mutate(i, gUC);
				if (i == 0)
					min = sp;
				if (sp.Cost < min.Cost)
					min = sp;
			}
			foreach (City c in gUC) {
				List<City> cities = GUI.MainClass.getCities();
				Solution sp;
				do {
					City city = cities[rand.Next(cities.Count)];
					sp = s.give(c, city);
				} while(sp == null);
				if (sp.Cost < min.Cost)
					min = sp;
			}
			return min;
		}

	}
}

