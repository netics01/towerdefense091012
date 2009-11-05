using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace PlanetTerror
{
	/// <summary>
	/// Interaction logic for boom.xaml
	/// </summary>
	public partial class boom : UserControl
	{
		Storyboard story;
		
		public boom()
		{
			this.InitializeComponent();
			
			story = (Storyboard)Resources.FindName("Core_Boom0_Storyboard");
		}

		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			story.Begin();
		}
	}
}