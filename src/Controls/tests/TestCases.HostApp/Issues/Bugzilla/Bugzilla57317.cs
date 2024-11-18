namespace Maui.Controls.Sample.Issues;


[Issue(IssueTracker.Bugzilla, 57317, "Modifying Cell.ContextActions can crash on Android", PlatformAffected.Android)]
public class Bugzilla57317 : TestContentPage
{
	protected override void Init()
	{
		var tableView = new TableView();
		var tableSection = new TableSection();

		// Create a custom ViewCell to hold the TextCell and labels
		var viewCell = new ViewCell();
		var stackLayout = new StackLayout();

		var cellLabel = new Label
		{
			Text = "Cell Label",
			AutomationId = "CellLabel"
		};

		var menuItemLabel = new Label
		{
			Text = "Self-Deleting item Label",
			AutomationId = "SelfDeletingItemLabel"
		};

		// Create a TextCell
		var switchCell = new TextCell
		{
			Text = "Cell",
			AutomationId = "Cell"
		};

		var menuItem = new MenuItem
		{
			Text = "Self-Deleting item",
			Command = new Command(() => switchCell.ContextActions.RemoveAt(0)),
			IsDestructive = true
		};
		switchCell.ContextActions.Add(menuItem);

		// Add labels to the StackLayout
		stackLayout.Children.Add(cellLabel);
		stackLayout.Children.Add(menuItemLabel);

		// Add the StackLayout to the ViewCell
		viewCell.View = stackLayout;

		// Add the ViewCell to the TableSection
		tableSection.Add(viewCell);
		tableView.Root.Add(tableSection);
		Content = tableView;
	}
}
