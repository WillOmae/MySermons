using System.Windows.Forms;
/* TODO
 * Implement timer that periodically erases messages/defaults the text
 * Implement colour codes
 */
namespace AppUI
{
    public class StatusBarMessages
    {
        public static ToolStripStatusLabel statusLabelUpdates, statusLabelShowing, statusLabelAction;
        static public void SetStatusBarMessageUpdates(string message)
        {
            statusLabelUpdates.Text = message;
            using (Timer timer = new Timer())
            {
                string textAtStart = statusLabelUpdates.Text;
                timer.Interval = 5000;
                timer.Tick += delegate
                {
                    if (statusLabelUpdates.Text == textAtStart)
                    {
                        statusLabelUpdates.Text = "Periodically check for updates e.g.weekly";
                    }
                    timer.Stop();
                };
                timer.Start();
            }
        }
        static public void SetStatusBarMessageShowing(string message)
        {
            statusLabelShowing.Text = "Showing: " + message;
        }
        static public void SetStatusBarMessageAction(string message)
        {
            statusLabelAction.Text = "Action: " + message;
            using (Timer timer = new Timer())
            {
                string textAtStart = statusLabelAction.Text;
                timer.Interval = 5000;
                timer.Tick += delegate
                {
                    if (statusLabelAction.Text == textAtStart)
                    {
                        statusLabelAction.Text = "Action: ";
                    }
                    timer.Stop();
                };
                timer.Start();
            }
        }
    }
}
