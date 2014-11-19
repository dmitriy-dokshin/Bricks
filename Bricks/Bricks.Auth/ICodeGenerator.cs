namespace Bricks.Auth
{
	public interface ICodeGenerator
	{
		string CreateNumericCode(int length);
	}
}