// AForge Framework
// XOR Problem solution using Multi-Layer Neural Network
//
// Copyright ?Andrew Kirillov, 2006
// andrew.kirillov@gmail.com
//

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.IO;

using AForge;
using AForge.Neuro;
using AForge.Neuro.Learning;
using AForge.Controls;

namespace XORProblem
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : Form
	{
		private GroupBox groupBox1;
		private Label label1;
		private TextBox learningRateBox;
		private Label label2;
		private TextBox alphaBox;
		private Label label3;
		private TextBox errorLimitBox;
		private Label label4;
		private ComboBox sigmoidTypeCombo;
		private TextBox currentErrorBox;
		private Label label11;
		private TextBox currentIterationBox;
		private Label label8;
		private Label label7;
		private Label label5;
		private Button stopButton;
		private Button startButton;
		private GroupBox groupBox2;
		private Chart errorChart;
		private CheckBox saveFilesCheck;
		private Label label6;
		private TextBox momentumBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		private double		learningRate = 0.1;
		private double		momentum = 0.0;
		private double		sigmoidAlphaValue = 2.0;
		private double		learningErrorLimit = 0.1;
		private int			sigmoidType = 0;
		private bool		saveStatisticsToFiles = false;

		private Thread	workerThread = null;
		private bool	needToStop = false;


		// Constructor
		public MainForm( )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// update controls
			UpdateSettings();

			// initialize charts
		    this.errorChart.AddDataSeries( "error", Color.Red, Chart.SeriesType.Line, 1 );
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (this.components != null) 
				{
				    this.components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.momentumBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.currentErrorBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.currentIterationBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.sigmoidTypeCombo = new System.Windows.Forms.ComboBox();
            this.errorLimitBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.alphaBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.learningRateBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.saveFilesCheck = new System.Windows.Forms.CheckBox();
            this.errorChart = new AForge.Controls.Chart();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.momentumBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.stopButton);
            this.groupBox1.Controls.Add(this.startButton);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.currentErrorBox);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.currentIterationBox);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.sigmoidTypeCombo);
            this.groupBox1.Controls.Add(this.errorLimitBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.alphaBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.learningRateBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(195, 260);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Neural Network";
            // 
            // momentumBox
            // 
            this.momentumBox.Location = new System.Drawing.Point(125, 45);
            this.momentumBox.Name = "momentumBox";
            this.momentumBox.Size = new System.Drawing.Size(60, 20);
            this.momentumBox.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(10, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 17);
            this.label6.TabIndex = 2;
            this.label6.Text = "Momentum:";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(110, 225);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 28;
            this.stopButton.Text = "S&top";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(25, 225);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 27;
            this.startButton.Text = "&Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(10, 211);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(175, 2);
            this.label5.TabIndex = 26;
            // 
            // currentErrorBox
            // 
            this.currentErrorBox.Location = new System.Drawing.Point(125, 185);
            this.currentErrorBox.Name = "currentErrorBox";
            this.currentErrorBox.ReadOnly = true;
            this.currentErrorBox.Size = new System.Drawing.Size(60, 20);
            this.currentErrorBox.TabIndex = 25;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(10, 187);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(121, 14);
            this.label11.TabIndex = 24;
            this.label11.Text = "Current average error:";
            // 
            // currentIterationBox
            // 
            this.currentIterationBox.Location = new System.Drawing.Point(125, 160);
            this.currentIterationBox.Name = "currentIterationBox";
            this.currentIterationBox.ReadOnly = true;
            this.currentIterationBox.Size = new System.Drawing.Size(60, 20);
            this.currentIterationBox.TabIndex = 23;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(10, 162);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 16);
            this.label8.TabIndex = 22;
            this.label8.Text = "Current iteration:";
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(10, 150);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(175, 2);
            this.label7.TabIndex = 21;
            // 
            // sigmoidTypeCombo
            // 
            this.sigmoidTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sigmoidTypeCombo.Items.AddRange(new object[] {
            "Unipolar",
            "Bipolar"});
            this.sigmoidTypeCombo.Location = new System.Drawing.Point(125, 120);
            this.sigmoidTypeCombo.Name = "sigmoidTypeCombo";
            this.sigmoidTypeCombo.Size = new System.Drawing.Size(60, 21);
            this.sigmoidTypeCombo.TabIndex = 9;
            // 
            // errorLimitBox
            // 
            this.errorLimitBox.Location = new System.Drawing.Point(125, 95);
            this.errorLimitBox.Name = "errorLimitBox";
            this.errorLimitBox.Size = new System.Drawing.Size(60, 20);
            this.errorLimitBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Learning error limit:";
            // 
            // alphaBox
            // 
            this.alphaBox.Location = new System.Drawing.Point(125, 70);
            this.alphaBox.Name = "alphaBox";
            this.alphaBox.Size = new System.Drawing.Size(60, 20);
            this.alphaBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Sigmoid\'s alpha value:";
            // 
            // learningRateBox
            // 
            this.learningRateBox.Location = new System.Drawing.Point(125, 20);
            this.learningRateBox.Name = "learningRateBox";
            this.learningRateBox.Size = new System.Drawing.Size(60, 20);
            this.learningRateBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Learning rate:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Sigmoid\'s type:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.saveFilesCheck);
            this.groupBox2.Controls.Add(this.errorChart);
            this.groupBox2.Location = new System.Drawing.Point(215, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(220, 260);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Error\'s dynamics";
            // 
            // saveFilesCheck
            // 
            this.saveFilesCheck.Location = new System.Drawing.Point(10, 233);
            this.saveFilesCheck.Name = "saveFilesCheck";
            this.saveFilesCheck.Size = new System.Drawing.Size(200, 18);
            this.saveFilesCheck.TabIndex = 1;
            this.saveFilesCheck.Text = "Save errors to files";
            // 
            // errorChart
            // 
            this.errorChart.Location = new System.Drawing.Point(10, 20);
            this.errorChart.Name = "errorChart";
            this.errorChart.Size = new System.Drawing.Size(200, 205);
            this.errorChart.TabIndex = 0;
            this.errorChart.Text = "chart1";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(452, 278);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "XOR Problem";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main( ) 
		{
			Application.Run( new MainForm( ) );
		}

		// On main form closing
		private void MainForm_Closing(object sender, CancelEventArgs e)
		{
			// check if worker thread is running
			if ( (this.workerThread != null ) && (this.workerThread.IsAlive ) )
			{
			    this.needToStop = true;
			    this.workerThread.Join( );
			}
		}

		// Update settings controls
		private void UpdateSettings( )
		{
		    this.learningRateBox.Text	= this.learningRate.ToString( );
		    this.momentumBox.Text		= this.momentum.ToString( );
		    this.alphaBox.Text			= this.sigmoidAlphaValue.ToString( );
		    this.errorLimitBox.Text		= this.learningErrorLimit.ToString( );
		    this.sigmoidTypeCombo.SelectedIndex = this.sigmoidType;

		    this.saveFilesCheck.Checked = this.saveStatisticsToFiles;
		}

		// Enable/disale controls
		private void EnableControls( bool enable )
		{
		    this.learningRateBox.Enabled		= enable;
		    this.momentumBox.Enabled			= enable;
		    this.alphaBox.Enabled			= enable;
		    this.errorLimitBox.Enabled		= enable;
		    this.sigmoidTypeCombo.Enabled	= enable;
		    this.saveFilesCheck.Enabled		= enable;

		    this.startButton.Enabled			= enable;
		    this.stopButton.Enabled			= !enable;
		}

		// On "Start" button click
		private void startButton_Click(object sender, EventArgs e)
		{
			// get learning rate
			try
			{
			    this.learningRate = Math.Max( 0.00001, Math.Min( 1, double.Parse(this.learningRateBox.Text ) ) );
			}
			catch
			{
			    this.learningRate = 0.1;
			}
			// get momentum
			try
			{
			    this.momentum = Math.Max( 0, Math.Min( 0.5, double.Parse(this.momentumBox.Text ) ) );
			}
			catch
			{
			    this.momentum = 0;
			}
			// get sigmoid's alpha value
			try
			{
			    this.sigmoidAlphaValue = Math.Max( 0.01, Math.Min( 100, double.Parse(this.alphaBox.Text ) ) );
			}
			catch
			{
			    this.sigmoidAlphaValue = 2;
			}
			// get learning error limit
			try
			{
			    this.learningErrorLimit = Math.Max( 0, double.Parse(this.errorLimitBox.Text ) );
			}
			catch
			{
			    this.learningErrorLimit = 0.1;
			}
			// get sigmoid's type
		    this.sigmoidType = this.sigmoidTypeCombo.SelectedIndex;

		    this.saveStatisticsToFiles = this.saveFilesCheck.Checked;

			// update settings controls
			UpdateSettings( );

			// disable all settings controls
			EnableControls( false );

			// run worker thread
		    this.needToStop = false;
		    this.workerThread = new Thread( new ThreadStart( SearchSolution ) );
		    this.workerThread.Start( );
		}

		// On "Stop" button click
		private void stopButton_Click(object sender, EventArgs e)
		{
			// stop worker thread
		    this.needToStop = true;
		    this.workerThread.Join( );
		    this.workerThread = null;
		}

		// Worker thread
		void SearchSolution( )
		{
			// initialize input and output values
			double[][] input = null;
			double[][] output = null;

			if (this.sigmoidType == 0 )
			{
				// unipolar data
				input = new double[4][] {
											new double[] {0, 0},
											new double[] {0, 1},
											new double[] {1, 0},
											new double[] {1, 1}
										};
				output = new double[4][] {
											 new double[] {0},
											 new double[] {1},
											 new double[] {1},
											 new double[] {0}
										 };
			}
			else
			{
				// biipolar data
				input = new double[4][] {
											new double[] {-1, -1},
											new double[] {-1,  1},
											new double[] { 1, -1},
											new double[] { 1,  1}
										};
				output = new double[4][] {
											 new double[] {-1},
											 new double[] { 1},
											 new double[] { 1},
											 new double[] {-1}
										 };
			}

			// create perceptron
			var	network = new ActivationNetwork(
				(this.sigmoidType == 0 ) ? 
					(IActivationFunction) new SigmoidFunction(this.sigmoidAlphaValue ) :
					(IActivationFunction) new BipolarSigmoidFunction(this.sigmoidAlphaValue ),
				2, 2, 1 );
			// create teacher
			var teacher = new BackPropagationLearning( network );
			// set learning rate and momentum
			teacher.LearningRate	= this.learningRate;
			teacher.Momentum		= this.momentum;

			// iterations
			var iteration = 1;

			// statistic files
			StreamWriter errorsFile = null;

			try
			{
				// check if we need to save statistics to files
				if (this.saveStatisticsToFiles )
				{
					// open files
					errorsFile	= File.CreateText( "errors.csv" );
				}
				
				// erros list
				var errorsList = new ArrayList( );

				// loop
				while ( !this.needToStop )
				{
					// run epoch of learning procedure
					var error = teacher.RunEpoch( input, output );
					errorsList.Add( error );

					// save current error
					if ( errorsFile != null )
					{
						errorsFile.WriteLine( error );
					}				

					// show current iteration & error
				    this.currentIterationBox.Text = iteration.ToString( );
				    this.currentErrorBox.Text = error.ToString( );
					iteration++;

					// check if we need to stop
					if ( error <= this.learningErrorLimit )
						break;
				}

				// show error's dynamics
				var errors = new double[errorsList.Count, 2];

				for ( int i = 0, n = errorsList.Count; i < n; i++ )
				{
					errors[i, 0] = i;
					errors[i, 1] = (double) errorsList[i];
				}

			    this.errorChart.RangeX = new DoubleRange( 0, errorsList.Count - 1 );
			    this.errorChart.UpdateDataSeries( "error", errors );
			}
			catch ( IOException )
			{
				MessageBox.Show( "Failed writing file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
			finally
			{
				// close files
				if ( errorsFile != null )
					errorsFile.Close( );
			}

			// enable settings controls
			EnableControls( true );
		}

        private void MainForm_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }
    }
}
