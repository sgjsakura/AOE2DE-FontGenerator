using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Sakura.Tools.Aoe2FontGenerator.Utilities;

namespace Sakura.Tools.Aoe2FontGenerator.Loggers
{
	/// <summary>
	/// Provide ability to generate progress changing notification and log information.
	/// </summary>
	public class ProgressLogger : IDisposable
	{

		/// <summary>
		/// Initialize a new instance of <see cref="ProgressLogger"/>.
		/// </summary>
		public ProgressLogger()
		{
			Worker = new BackgroundWorker { WorkerReportsProgress = true };
			Worker.DoWork += Worker_DoWork;
			Worker.ProgressChanged += Worker_ProgressChanged;
			Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
		}

		#region Internal Service Instance

		/// <summary>
		/// The internal <see cref="BackgroundWorker"/> used to interaction between threads.
		/// </summary>
		private BackgroundWorker Worker { get; }

		#endregion

		#region Event Objects

		/// <summary>
		/// Raises when new log is outputted.
		/// </summary>
		public event EventHandler<LogOutputtedEventArgs> LogOutputted;

		/// <summary>
		/// raises when <see cref="StartAsync"/> is called. This event is raised in a standalone thread.
		/// </summary>
		public event EventHandler DoWork;

		/// <summary>
		/// Raises when work is completed.
		/// </summary>
		public event EventHandler<RunWorkerCompletedEventArgs> Completed;

		/// <summary>
		/// Raises when <see cref="InitializeProgress"/> is called.
		/// </summary>
		public event EventHandler<ProgressInitializedEventArgs> ProgressInitialized;

		#endregion

		#region Internal BackgroundWorker Event Handler


		private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			// end all progress
			EndProgress();

			Completed?.Invoke(this, e);
		}

		/// <summary>
		/// Handle the <see cref="BackgroundWorker.DoWork"/> event to make custom callback.
		/// </summary>
		/// <param name="sender">The sender or the event.</param>
		/// <param name="e">The argument of the event.</param>
		private void Worker_DoWork(object sender, DoWorkEventArgs e)
		{
			DoWork?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Handle the <see cref="BackgroundWorker.ProgressChanged"/> event to make custom callback.
		/// </summary>
		/// <param name="sender">The sender or the event.</param>
		/// <param name="e">The argument of the event.</param>
		private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			switch (e.UserState)
			{
				case LogOutputtedEventArgs args:
					LogOutputted?.Invoke(this, args);
					break;
				case ProgressInitializedEventArgs args:
					ProgressInitialized?.Invoke(this, args);
					break;
			}
		}

		#endregion

		#region Core Methods and properties

		/// <summary>
		/// The internal array used for recording all levels.
		/// </summary>
		private ObservableCollection<LevelInfo> LevelInfos { get; set; } = new ObservableCollection<LevelInfo>();

		/// <summary>
		/// Internal method used to call the <see cref="BackgroundWorker.ReportProgress(int, object)"/> in order to transport message to the main thread.
		/// </summary>
		/// <param name="e">The extra message instance.</param>
		private void ReportProgressOrLog(EventArgs e)
		{
			Worker.ReportProgress(0, e);
		}

		/// <summary>
		/// End all progress. This method is called automatically after <see cref="BackgroundWorker.RunWorkerCompleted"/> is called.
		/// </summary>
		private void EndProgress()
		{
			foreach (var levelInfo in LevelInfos)
			{
				// Set current value to max value.
				levelInfo.CurrentValue = levelInfo.MaxValue;
			}
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Get the last level.
		/// </summary>
		private LevelInfo LastLevel => LevelInfos[LevelInfos.Count - 1];


		/// <summary>
		/// Checks the level index to ensure it is not out or range and gets the level information.
		/// </summary>
		/// <param name="level">The level index.</param>
		/// <returns>The related <see cref="LevelInfo"/> instance.</returns>
		/// <exception cref="InvalidOperationException">The <see cref="InitializeProgress"/> method is not called before.</exception>
		/// <exception cref="ArgumentOutOfRangeException">The <paramref name="level"/> value is out or range.</exception>
		private LevelInfo CheckLevelIndex(int level)
		{
			if (LevelInfos == null || LevelInfos.Count == 0)
			{
				throw new InvalidOperationException(App.Current.FormatResString("LoggerInitializeNotCalledException", nameof(InitializeProgress)));
			}

			if (level < 0 || level >= LevelInfos.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(level), level, App.Current.FormatResString("LoggerLevelOutOfRangeException"));
			}

			return LevelInfos[level];
		}

		#endregion

		#region Public Interfaces

		#region Worker Generals

		/// <summary>
		/// Get a value that indicates if the worker is currently working.
		/// </summary>
		public bool IsWorking => Worker.IsBusy;

		/// <summary>
		/// Start the worker.
		/// </summary>
		public void StartAsync()
		{
			Worker.RunWorkerAsync();
		}

		#endregion

		#region Logs

		/// <summary>
		/// Generates a log output and raises the <see cref="LogOutputted"/> event.
		/// </summary>
		/// <param name="level">The level of the log message.</param>
		/// <param name="message">The content of the log message.</param>
		public void Log(LogLevel level, [Localizable(true)] string message)
		{
			var args = new LogOutputtedEventArgs(level, message);
			ReportProgressOrLog(args);
		}

		#endregion

		#region Progress Management

		/// <summary>
		/// Initialize this instance and prepare to receive progress changes.
		/// </summary>
		/// <param name="levelCount">The total count for progress levels.</param>
		public void InitializeProgress(int levelCount)
		{
			if (levelCount <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(levelCount), levelCount, App.Current.FormatResString("LoggerLevelCountOutOfRangeException"));
			}

			LevelInfos = new ObservableCollection<LevelInfo>(Utility.Repeat(() => new LevelInfo(), levelCount));

			// Raise events.
			ReportProgressOrLog(new ProgressInitializedEventArgs(LevelInfos));
		}

		/// <summary>
		/// The the progress maximum value of the specified level.
		/// </summary>
		/// <param name="level">The level index.</param>
		/// <param name="message">The message of the level.</param>
		/// <param name="maxValue">The maximum value of this level. If the value is <see cref="double.NaN"/>, it means the current level is indeterminate.</param>
		public void SetProgressLevel(int level, string message, double maxValue)
		{
			var currentLevel = CheckLevelIndex(level);

			// Set message.
			currentLevel.Message = message;

			// Max value should never be NaN or it may damage the final "finished" state, so we apply the NaN value to the current value here 
			if (double.IsNaN(maxValue))
			{
				currentLevel.MaxValue = 100;
				currentLevel.CurrentValue = double.NaN;
			}
			else
			{
				currentLevel.MaxValue = maxValue;
				currentLevel.CurrentValue = 0;
			}

		}

		/// <summary>
		/// Advance the current progress to the next stage.
		/// </summary>
		/// <param name="level">The level index.</param>
		/// <param name="length">The length of the current stage.</param>
		public void AdvanceProgressStage(int level, double length)
		{
			// Update the current level
			var currentLevel = CheckLevelIndex(level);


			// Update current value to the current stage end
			currentLevel.CurrentValue = currentLevel.CurrentStageStart + currentLevel.CurrentStageLength;

			// Prepare to the next stage
			currentLevel.CurrentStageStart = currentLevel.CurrentValue;
			currentLevel.CurrentStageLength = length;

			// Clear all sub levels
			for (var i = level + 1; i < LevelInfos.Count; i++)
			{
				LevelInfos[i].Reset();
			}
		}

		/// <summary>
		/// Add the current value of the last level with the specified increment value.
		/// </summary>
		/// <param name="increment">The increment of the last level.</param>
		public void AdvanceCurrentStageProgress(double increment) =>
			UpdateCurrentStageProgress(LastLevel.CurrentValue + increment);

		/// <summary>
		/// Update the current value of the last level with the specified value.
		/// </summary>
		/// <param name="value">The new value of the last level.</param>
		public void UpdateCurrentStageProgress(double value)
		{
			void UpdateLevelFromNext(int level)
			{
				var currentLevel = LevelInfos[level];
				var nextLevel = LevelInfos[level + 1];

				if (double.IsNaN(nextLevel.CurrentValue))
				{
					return;
				}

				currentLevel.CurrentValue = currentLevel.CurrentStageStart +
											currentLevel.CurrentStageLength *
											(nextLevel.CurrentValue / nextLevel.MaxValue);
			}

			if (double.IsNaN(value))
			{

			}
			else
			{
				// Update current level
				LastLevel.CurrentValue = value;

				// Update parent levels.
				for (var i = LevelInfos.Count - 2; i >= 0; i--)
				{
					UpdateLevelFromNext(i);
				}
			}
		}

		#endregion

		#endregion

		#region IDisposable Support

		private void ReleaseUnmanagedResources()
		{
			// TODO release unmanaged resources here
		}

		private void Dispose(bool disposing)
		{
			ReleaseUnmanagedResources();

			if (disposing)
			{
				Worker?.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~ProgressLogger()
		{
			Dispose(false);
		}

		#endregion
	}
}
