using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Bugzilla43161 : _IssuesUITest
	{
		public Bugzilla43161(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "[iOS] Setting Accessory in ViewCellRenderer breaks layout";

		[Test]
		[Category(UITestCategories.Cells)]
		public void Bugzilla43161Test()
		{
			App.WaitForElement("Item_0");
			App.WaitForElement("Item_10");
			App.WaitForElement("Item_20");
		}
	}
}