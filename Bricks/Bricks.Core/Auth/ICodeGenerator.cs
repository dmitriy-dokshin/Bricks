namespace Bricks.Core.Auth
{
	public interface ICodeGenerator
	{
		string CreateNumericCode(int length);
	}
}