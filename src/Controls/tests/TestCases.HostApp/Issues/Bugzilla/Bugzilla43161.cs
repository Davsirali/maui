namespace Maui.Controls.Sample.Issues
{
	[Issue(IssueTracker.Bugzilla, 43161, "[iOS] Setting Accessory in ViewCellRenderer breaks layout", PlatformAffected.iOS)]
	public class Bugzilla43161 : TestContentPage
	{
		const string Instructions = "On iOS, all three of the following ListViews should have ListItems labeled with numbers and a right arrow. If any of the ListViews does not contain numbers, this test has failed.";
		const string ListView1 = "Accessory with Context Actions";
		const string ListView2 = "Accessory with RecycleElement";
		const string ListView3 = "Accessory with RetainElement";

		public class Item
		{
			public string Text { get; set; }
			public string AutomationId { get; set; }
		}

		public class AccessoryViewCell : ViewCell
		{
			public AccessoryViewCell()
			{
				var label = new Label();
				label.SetBinding(Label.TextProperty, "Text");
				label.SetBinding(Label.AutomationIdProperty, "AutomationId");
				View = label;
			}
		}

		public class AccessoryViewCellWithContextActions : AccessoryViewCell
		{
			public AccessoryViewCellWithContextActions()
			{
				var label = new Label();
				label.SetBinding(Label.TextProperty, "Text");
				label.SetBinding(Label.AutomationIdProperty, "AutomationId");
				View = label;

				var delete = new MenuItem { Text = "Delete", AutomationId = "DeleteMenuItem" };
				ContextActions.Add(delete);
			}
		}

		protected override void Init()
		{
			var label = new Label { Text = Instructions, AutomationId = "InstructionsLabel" };

			var items1 = Enumerable.Range(0, 9).Select(i => new Item { Text = $"Item {i}", AutomationId = $"Item_{i}" }).ToList();
			var items2 = Enumerable.Range(10, 19).Select(i => new Item { Text = $"Item {i}", AutomationId = $"Item_{i}" }).ToList();
			var items3 = Enumerable.Range(20, 29).Select(i => new Item { Text = $"Item {i}", AutomationId = $"Item_{i}" }).ToList();

			var listView = new ListView
			{
				ItemTemplate = new DataTemplate(typeof(AccessoryViewCellWithContextActions)),
				ItemsSource = items1,
				Header = ListView1
			};
			var listView2 = new ListView(ListViewCachingStrategy.RecycleElement)
			{
				ItemTemplate = new DataTemplate(typeof(AccessoryViewCell)),
				ItemsSource = items2,
				Header = ListView2
			};
			var listView3 = new ListView
			{
				ItemTemplate = new DataTemplate(typeof(AccessoryViewCell)),
				ItemsSource = items3,
				Header = ListView3
			};

			Content = new StackLayout
			{
				Children = { label, listView, listView2, listView3 }
			};
		}
	}
}