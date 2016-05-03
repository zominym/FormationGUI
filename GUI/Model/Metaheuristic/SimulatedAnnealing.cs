using System;
using TrainingProblem;
using System.Collections.Generic;
using GUI;

namespace Metaheuristic
{
    public class SimulatedAnnealing
    {
        double t = 0.0;
        private Random rand = GUI.MainClass.rand;

        public SimulatedAnnealing()
        {
            
        }

        public Solution run(int iteration, double micro, double initialTemperature){
            t = initialTemperature;
            double µ = micro;//0.5
            int param = (int) Math.Floor(0.05*iteration);
            Solution xmin, xn, xnn, x0 = new Solution();
            xmin = x0;
            xn = x0;
            double fmin = xmin.Cost;
            for (int i = 0; i < iteration; i++)
            {
				MainClass.win.writeLine(MainClass.iterToString (i, xmin.Cost, xn.Cost, xmin.getUsedCities().Count), true);
				MainClass.win.setProgress ((double)i / (double)iteration);
				while (Gtk.Application.EventsPending ())
					Gtk.Application.RunIteration ();

                Solution y = xn.mutate(rand.Next(GUI.MainClass.getAgencies().Count));//xn.Neighbors2[rand.Next(xn.Neighbors2.Count)];
                double Δf = (y.Cost/1) - (xn.Cost/1);
                if (Δf <= 0)
                {
                    xnn = y;
                    if ((xnn.Cost / 1) < (xmin.Cost / 1))
                        xmin = xnn;
                }
                else {
                    if (rand.NextDouble() <= Math.Exp(-Δf / t))
                        xnn = y;
                    else
                        xnn = xn;
                }
                if ((xn.Cost == xnn.Cost) & (t <= 1))
                    break;
                xn = xnn;
                if (i % param == 0)
                    t = µ * t;
            }
            return xmin;
        }
    }
}