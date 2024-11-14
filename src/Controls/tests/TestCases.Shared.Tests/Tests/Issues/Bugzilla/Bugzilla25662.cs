using NUnit.Framework;
using UITest.Appium;
using UITest.Core;
namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Bugzilla25662 : _IssuesUITest
	{
		public Bugzilla25662(TestDevice testDevice) : base(testDevice)
		{
		}
		public override string Issue => "Setting IsEnabled does not disable SwitchCell";
		[Test]
		[Category(UITestCategories.Cells)]
		public void Bugzilla25662Test()
		{
			App.WaitForElement("OneLabel");
			App.Tap("OneLabel");
			App.WaitForNoElement("FAIL");
		}
	}
}
