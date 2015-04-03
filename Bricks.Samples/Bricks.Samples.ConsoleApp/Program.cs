#region

using System;
using System.Threading;

using Bricks.Core.Modularity;
using Bricks.Core.Regex;
using Bricks.SMS;

#endregion

namespace Bricks.Samples.ConsoleApp
{
	internal class Program
	{
		private static void Main()
		{
			using (IApplication application = new Application())
			{
				application.Initialize();
				var smsService = application.ServiceLocator.GetInstance<ISmsService>();
				var regexHelper = application.ServiceLocator.GetInstance<IRegexHelper>();
				while (true)
				{
					Console.Write(Resources.Console_Main_PhoneNumber);
					string phoneNumberSource = Console.ReadLine();
					string phoneNumber;
					if (regexHelper.TryParsePhoneNumber(phoneNumberSource, out phoneNumber))
					{
						Console.Write(Resources.Console_Main_Message);
						string message = Console.ReadLine();
						smsService.SendAsync(phoneNumber, message, CancellationToken.None);

						Console.WriteLine(Resources.Console_Main_OnceMore);
						ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
						if (consoleKeyInfo.Key != ConsoleKey.Y)
						{
							break;
						}
					}
					else
					{
						Console.WriteLine(Resources.Console_Main_InvalidPhoneNumber);
					}
				}
			}
		}
	}
}