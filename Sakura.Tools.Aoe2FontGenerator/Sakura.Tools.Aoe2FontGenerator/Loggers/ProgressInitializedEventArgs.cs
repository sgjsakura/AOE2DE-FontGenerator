using System;
using System.Collections.Generic;

namespace Sakura.Tools.Aoe2FontGenerator.Loggers
{
	/// <summary>
	/// Indicates that the <see cref="ProgressLogger.LevelInfos"/> has been initialized.
	/// </summary>
	public class ProgressInitializedEventArgs : EventArgs
	{
		/// <summary>
		/// The initialized <see cref="LevelInfo"/> collection.
		/// </summary>
		public IEnumerable<LevelInfo> LevelInfos { get; }

		/// <summary>
		/// Initialize a new instance of <see cref="ProgressInitializedEventArgs"/>.
		/// </summary>
		/// <param name="levelInfos">The value of the <see cref="LevelInfos"/> property.</param>
		public ProgressInitializedEventArgs(IEnumerable<LevelInfo> levelInfos)
		{
			LevelInfos = levelInfos;
		}
	}
}