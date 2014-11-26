namespace Bricks.Core.Environment
{
	/// <summary>
	/// Contains methods and properties for working with application's environment.
	/// </summary>
	public interface IEnvironment
	{
		/// <summary>
		/// Returns the application root.
		/// </summary>
		string RootPath { get; }
	}
}