using System;
using TrainingProblem;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GUI;

namespace Metaheuristic
{
    public class Genetic
    {
        private int _iterations, _populationSize;
		private static Random rand = GUI.MainClass.rand;
		private double ProbaMutation;
		private double ProbaMagic;


		public Genetic(int iterations, int populationSize, double ProbaMutation, double ProbaMagic) {
            _iterations = iterations;
            _populationSize = populationSize;
			this.ProbaMutation = ProbaMutation;
			this.ProbaMagic = ProbaMagic;
        }

        int PopulationSize {
            get { return _populationSize;}
        }

        int Iterations {
            get { return _iterations; }
        }

        public List<Solution> buildPopulation() {
            List<Solution> population;
            population = new List<Solution>();
            for (int i = 0; i < PopulationSize; i++)
            {
                population.Add(new Solution());
            }
            return population;
        }

        public Solution getBestSolution(List<Solution> population){
			Solution min = population.First();
            double costMin = min.Cost;
            double curentCost;
            foreach (Solution solution in population) {
                curentCost = solution.Cost;
                if (curentCost < costMin)
                {
                    min = solution;
                    costMin = curentCost;
                }
                    
            }
            return min;
        }

        public List<Solution> RouletteVladof(List<Solution> population, int nbToTake)
        {
            List<Solution> result = new List<Solution>();
            Dictionary<double, Solution> wheel = new Dictionary<double, Solution>();

            double T = 0, sum = 0;
            foreach (Solution sol in population)
            {
                T += sol.Cost;
            }
            double last = 0;
            foreach (Solution sol in population)
            {
                sum += (T - sol.Cost);
                wheel.Add(last + (T - sol.Cost), sol);
                last = wheel.Last().Key;
            }
            double aleaJactaEst;
            for (int i = 0; i < nbToTake; ++i)
            {
                aleaJactaEst = rand.NextDouble() * sum;
                foreach (double key in wheel.Keys)
                {
                    if (key >= aleaJactaEst)
                    {
                        result.Add(wheel[key]);
                        break;
                    }
                }
                    
            }

            return result;
        }

        public Solution getSolution() {
            List<Solution> nextPopulation, elite, crossResult, currentPopulation = buildPopulation();
            Solution bestSolution = getBestSolution(currentPopulation), currentBest;
            int bestSolutionCost = (int) bestSolution.Cost;

            for (int i = 0; i < Iterations; i++) {
                // Creation of the new population
                nextPopulation = new List<Solution>();

                // Selection of the representative
                elite = RouletteVladof(currentPopulation, currentPopulation.Count / 2);
                
                // Reproduction
                while(nextPopulation.Count < currentPopulation.Count) {
                    // Combinaison
                    if((crossResult = elite[rand.Next(elite.Count)].crossover(elite[rand.Next(elite.Count)])) != null)
                        nextPopulation.AddRange(crossResult);

                    // Mutation
                    if (nextPopulation.Count > 0)
                    {
                        if(rand.NextDouble() > (1 - ProbaMagic))
						    nextPopulation[rand.Next(nextPopulation.Count)].trick();
                        if(rand.NextDouble() > (1 - ProbaMutation))
                            nextPopulation[rand.Next(nextPopulation.Count)].badassMutation();
                    }
                        
                }

                // Next generation is the new generation
                currentPopulation = nextPopulation;
                
                // See who's the bigest
                currentBest = getBestSolution(currentPopulation);
                int currentBestCost = (int) currentBest.Cost;

				GUI.MainClass.win.writeLine(GUI.MainClass.iterToString(i, bestSolutionCost, currentBestCost, currentBest.nbCenters), true);
				MainClass.win.setProgress ((double)i / (double)_iterations);
				while (Gtk.Application.EventsPending())
					Gtk.Application.RunIteration();
                // Take the legend
                if (bestSolutionCost > currentBestCost)
                {
                    bestSolution = getBestSolution(currentPopulation);
                    bestSolutionCost = (int) bestSolution.Cost;
                }
                    
            }

            // Show the legend
            return bestSolution;
        }
    }
}

