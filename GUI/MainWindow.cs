using System;
using Gtk;
using GUI;
using Metaheuristic;

public partial class MainWindow: Gtk.Window
{
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build();

		fileChooserA.SetCurrentFolder("inputs/");
		FileFilter ffA = new FileFilter ();
		ffA.Name = "Agencies";
		ffA.AddPattern ("ListeAgences*.txt");
		fileChooserA.Filter = ffA;
//		fileChooserA.SetFilename ("inputs/ListeAgences_100.txt");
//		fileChooserA.SetUri ("ListeAgences_100.txt");



		fileChooserC.SetCurrentFolder("inputs/");
		FileFilter ffC = new FileFilter ();
		ffC.Name = "Cities";
		ffC.AddPattern ("LieuxPossibles*.txt");
		fileChooserC.Filter = ffC;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnButtonStartDClicked (object sender, EventArgs e)
	{
		if (! MainClass.checkstart()) {
			writeLine ("A problem occured, the algorithm could not be selected.\nPlease select both an Agencies and Cities files.");
			return;
		}
		Taboo taboo = new Taboo();
		writeLine ("Starting taboo");
		Solution s = taboo.run((int) buttonIterD.Value);
		if (MainClass.writeToCSV)
			s.writeToCSV ();
		textOutput.Buffer.Text = s.ToString();
	}

	protected void OnButtonStartSAClicked (object sender, EventArgs e)
	{
		if (! MainClass.checkstart()) {
			writeLine ("A problem occured, the algorithm could not be selected.\nPlease select both an Agencies and Cities files.");
			return;
		}
		SimulatedAnnealing sa = new SimulatedAnnealing();
		writeLine ("Starting taboo");
		Solution s = sa.run((int) buttonIterSA.Value, buttonReasonSA.Value, buttonTempSA.Value);
		if (MainClass.writeToCSV)
			s.writeToCSV ();
		textOutput.Buffer.Text = s.ToString();
	}

	protected void OnButtonStartGClicked (object sender, EventArgs e)
	{
		if (! MainClass.checkstart()) {
			writeLine ("A problem occured, the algorithm could not be selected.\nPlease select both an Agencies and Cities files.");
			return;
		}
		Genetic gen = new Genetic((int)  buttonIterG.Value, (int) buttonPopG.Value, buttonMutaG.Value, buttonMagicG.Value);
		writeLine ("Starting taboo");
		Solution s = gen.getSolution();
		if (MainClass.writeToCSV)
			s.writeToCSV ();
		textOutput.Buffer.Text = s.ToString();
	}

	protected void OnFileChooserASelectionChanged (object sender, EventArgs e)
	{
		MainClass.agenciesFile = fileChooserA.Filename;
		int nbAgences = MainClass.loadAgencies();
		if (nbAgences < 1) {
			textOutput.Buffer.Text += "An error occured while trying to load agencies, no agencies loaded.\n";
			return;
		}
//
		textOutput.Buffer.Text += "Successfully loaded " + nbAgences + " agencies.\n";
	}

	protected void OnFileChooserCSelectionChanged (object sender, EventArgs e)
	{
		MainClass.citiesFile = fileChooserC.Filename;
		int nbCities = MainClass.loadCities();
		if (nbCities < 1) {
			textOutput.Buffer.Text += "An error occured while trying to load cities, no cities loaded.\n";
			return;
		}

		textOutput.Buffer.Text += "Successfully loaded " + nbCities + " cities.\n";
	}

	protected void OnCheckbuttonCSVToggled (object sender, EventArgs e)
	{
		MainClass.writeToCSV = !MainClass.writeToCSV;
	}

	public void setProgress(double f) {
		progressBar.Fraction = f;
	}

	public void write(string msg, bool clear = false) {
		if (clear)
			textOutput.Buffer.Text = "";
		textOutput.Buffer.Text += msg;
	}

	public void writeLine(string msg, bool clear = false) {
		if (clear)
			textOutput.Buffer.Text = "";
		textOutput.Buffer.Text += msg + "\n";
	}
}
