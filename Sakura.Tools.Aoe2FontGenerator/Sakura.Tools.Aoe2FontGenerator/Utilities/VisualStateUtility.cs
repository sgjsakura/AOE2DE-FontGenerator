using System.Windows;

namespace Sakura.Tools.Aoe2FontGenerator.Utilities
{
	/// <summary>
	/// Provides extension methods for visual state management. This class is static.
	/// </summary>
	public static class VisualStateUtility
	{
		/// <summary>
		/// Changes the state of the control.
		/// </summary>
		/// <param name="control">The control whose state should be changed.</param>
		/// <param name="stateName">The new state name.</param>
		/// <param name="useTransitions">If true, the state transition should be used.</param>
		/// <returns>A value that indicates if the <paramref name="control"/> has been changed to the target state.</returns>
		public static bool GoToState(this FrameworkElement control, string stateName, bool useTransitions) =>
			VisualStateManager.GoToState(control, stateName, useTransitions);

		/// <summary>
		/// Changes the state of the control.
		/// </summary>
		/// <param name="control">The control whose state should be changed.</param>
		/// <param name="state">The new state.</param>
		/// <param name="useTransitions">If true, the state transition should be used.</param>
		/// <returns>A value that indicates if the <paramref name="control"/> has been changed to the target state.</returns>
		public static void GoToState(this FrameworkElement control, VisualState state, bool useTransitions) =>
			VisualStateManager.GoToState(control, state.Name, useTransitions);

		/// <summary>
		/// Changes the state of the control. Using the external state defined in the target element.
		/// </summary>
		/// <param name="stateGroupsRoot">The control which defines the state group.</param>
		/// <param name="stateName">The new state name.</param>
		/// <param name="useTransitions">If true, the state transition should be used.</param>
		/// <returns>A value that indicates if the <paramref name="stateGroupsRoot"/> has been changed to the target state.</returns>
		public static void GoToElementState(this FrameworkElement stateGroupsRoot, string stateName, bool useTransitions) =>
			VisualStateManager.GoToElementState(stateGroupsRoot, stateName, useTransitions);

		/// <summary>
		/// Changes the state of the control. Using the external state defined in the target element.
		/// </summary>
		/// <param name="stateGroupsRoot">The control which defines the state group.</param>
		/// <param name="state">The new state.</param>
		/// <param name="useTransitions">If true, the state transition should be used.</param>
		/// <returns>A value that indicates if the <paramref name="stateGroupsRoot"/> has been changed to the target state.</returns>
		public static void GoToElementState(this FrameworkElement stateGroupsRoot, VisualState state, bool useTransitions) =>
			VisualStateManager.GoToElementState(stateGroupsRoot, state.Name, useTransitions);
	}
}
