using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

[Category(UITestCategories.ListView)]
public class Bugzilla40704 : _IssuesUITest
{
	const string Collapse = "btnCollapse";
	

	public Bugzilla40704(TestDevice testDevice) : base(testDevice)
	{
	}

	public override string Issue => "Strange duplication of listview headers when collapsing/expanding sections";


	[Test]
	public void Bugzilla40704HeaderPresentTest()
	{
		App.WaitForElement("Menu - 0");
	}

	
	[Test]
	public void Bugzilla40704Test()
	{
		
		App.ScrollTo(Collapse);
		App.Tap(Collapse);
		Task.Delay(1000).Wait(); // Let the layout settle down

		App.ScrollDown(Collapse, ScrollStrategy.Gesture, 0.9, 500);
		App.Tap(Collapse);
		Task.Delay(1000).Wait(); // Let the layout settle down

		App.ScrollDown(Collapse, ScrollStrategy.Gesture, 0.9, 500);
		App.Tap(Collapse);

		App.WaitForElement("Menu - 2");
		App.WaitForElement("Menu - 1");
		App.WaitForElement("Menu - 0");
	}
}
