using System;

namespace TrainingProblem
{
	public class City
	{
		static public int ID = 0;
		protected int id;
		protected string name;
		protected string codepostal;
		public double longitude;
		public double latitude;

		public City () : this("\"example_id\";\"example_name\";\"example_codepostal\";9.99999;99.9999") {
			// NOTHING
		}

		public City (string csvLine){
			// "id";"nom";"codepostal";"longitude";"latitude"

            string[] csvTab = csvLine.Replace ("\"", "").Split (';');
			id = ID; ID++;
			name = csvTab [1];
			codepostal = csvTab [2];
            longitude = Double.Parse(csvTab[3]);
            latitude = Double.Parse(csvTab[4]);
		}

		public bool Equals(City c)
		{
			return c.id.Equals(this.id);
		}

		public override string ToString ()
		{
			return id + " : " + name + " ; " + codepostal + " (" + longitude + ";" + latitude + ")";
		}

		public double distanceTo(City c)
		{
			double[,] distTab = GUI.MainClass.distTab;
			double d = distTab[id, c.getId()];
			if (d != -1)
				return d;

			double lat1 = this.latitude;
			double lon1 = this.longitude;
			double lat2 = c.latitude;
			double lon2 = c.longitude;

			double theta = lon1 - lon2;
			double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
			dist = Math.Acos(dist);
			dist = rad2deg(dist) * 60 * 1.1515 * 1.609344;

			distTab[id, c.getId()] = dist;
			distTab[c.getId(), id] = dist;

			return dist;
		}

		protected double deg2rad(double deg) {
			return (deg * Math.PI / 180.0);
		}

		protected double rad2deg(double rad) {
			return (rad / Math.PI * 180.0);
		}

		public int getId() {
			return id;
		}

		public string getName() {
			return name;
		}

		public string getCodePostal() {
			return codepostal;
		}

		public double getLat() {
			return latitude;
		}

		public double getLong() {
			return longitude;
		}
	}
}

