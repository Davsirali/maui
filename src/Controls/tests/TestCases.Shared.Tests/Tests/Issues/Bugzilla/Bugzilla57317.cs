using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Bugzilla57317 : _IssuesUITest
{
	const string Success = "Success";
	const string BtnAdd = "btnAdd";

	public Bugzilla57317(TestDevice testDevice) : base(testDevice)
	{
	}

	public override string Issue => "Modifying Cell.ContextActions can crash on Android";

	[Test]
	[Category(UITestCategories.TableView)]
	public void Bugzilla57317Test()
	{
		App.WaitForFirstElement("CellLabel");

		// Perform a long press to activate the context menu
		App.TouchAndHold("CellLabel");

		App.WaitForFirstElement("SelfDeletingItemLabel");
		App.Tap("SelfDeletingItemLabel");
	}
}