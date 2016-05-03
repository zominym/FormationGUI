using System;

namespace TrainingProblem
{
	public class Agency : City
	{

		protected int nbPers;

		public Agency(string csvLine) : base(csvLine)
		{
			nbPers = int.Parse(csvLine.Split (';')[5]);
		}

		public int getNbPers()
		{
			return nbPers;
		}

		public void setNbPers(int n)
		{
			nbPers = n;
		}
	}
}

