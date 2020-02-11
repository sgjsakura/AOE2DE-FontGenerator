using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Sakura.Tools.Aoe2FontGenerator.Loggers
{
	/// <summary>
	/// Used to recording all information of a single progress level for the <see cref="ProgressLogger"/>.
	/// </summary>
	public class LevelInfo : INotifyPropertyChanged
	{
		#region Properties

		/// <summary>
		/// Backend field for the <see cref="MaxValue"/> property.
		/// </summary>
		private double _maxValue;
		/// <summary>
		/// Backend field for the <see cref="CurrentValue"/> property.
		/// </summary>
		private double _currentValue;
		/// <summary>
		/// Backend field for the <see cref="CurrentStageStart"/> property.
		/// </summary>
		private double _currentStageStart;
		/// <summary>
		/// Backend field for the <see cref="CurrentStageLength"/> property.
		/// </summary>
		private double _currentStageLength;
		/// <summary>
		/// Backend field for the <see cref="Message"/> property.
		/// </summary>
		private string _message;

		/// <summary>
		/// The max value of this level. 
		/// </summary>
		public double MaxValue
		{
			get => _maxValue;
			set
			{
				if (value.Equals(_maxValue)) return;
				_maxValue = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// The current value of this level. If the value is <see cref="double.NaN"/>, it means the current progress cannot be determinable.
		/// </summary>
		public double CurrentValue
		{
			get => _currentValue;
			set
			{
				if (value.Equals(_currentValue)) return;
				_currentValue = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// The start value of the current stage.
		/// </summary>
		public double CurrentStageStart
		{
			get => _currentStageStart;
			set
			{
				if (value.Equals(_currentStageStart)) return;
				_currentStageStart = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// The max length of the current stage. It is used when  to calculated the the <see cref="CurrentValue"/>.
		/// </summary>
		public double CurrentStageLength
		{
			get => _currentStageLength;
			set
			{
				if (value.Equals(_currentStageLength)) return;
				_currentStageLength = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// The message which describe the current level.
		/// </summary>
		public string Message
		{
			get => _message;
			set
			{
				if (value == _message) return;
				_message = value;
				OnPropertyChanged();
			}
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Reset this instance to the empty state.
		/// </summary>
		public void Reset()
		{
			CurrentValue = 0;
			MaxValue = 0;
			CurrentStageStart = 0;
			CurrentStageLength = 0;
			Message = null;
		}

		#endregion

		#region  NPC Support

		/// <summary>
		/// Raises when property is changed.
		/// </summary>
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises the <see cref="PropertyChanged"/> event.
		/// </summary>
		/// <param name="propertyName">The changed property name. Default value is the caller member name.</param>
		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}