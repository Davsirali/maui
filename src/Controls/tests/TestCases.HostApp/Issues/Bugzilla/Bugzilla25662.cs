using Microsoft.Maui.Controls;
namespace Maui.Controls.Sample.Issues
{
	[Issue(IssueTracker.Bugzilla, 25662, "Setting IsEnabled does not disable SwitchCell")]
	public class Bugzilla25662 : TestContentPage
	{
		class MySwitch : ViewCell
		{
			public MySwitch()
			{
				var switchControl = new Switch
				{
					IsEnabled = false
				};
				switchControl.SetBinding(Switch.IsToggledProperty, new Binding("."));
				switchControl.AutomationId = "SwitchCell";
				var label = new Label
				{
					Text = "One",
					AutomationId = "OneLabel"
				};
				View = new StackLayout
				{
					Children = { switchControl, label }
				};
			}
		}
		protected override void Init()
		{
			var list = new ListView
			{
				ItemsSource = new[] { "One", "Two", "Three" },
				ItemTemplate = new DataTemplate(typeof(MySwitch))
			};
			Content = list;
		}
	}
}
