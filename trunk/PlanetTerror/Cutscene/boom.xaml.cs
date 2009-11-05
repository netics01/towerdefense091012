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
	public partial class Boom : UserControl
	{
		Storyboard story;
		
		public Boom()
		{
			this.InitializeComponent();
			
			story = Resources["Core_Boom0_Storyboard"] as Storyboard;

			IsVisibleChanged += new DependencyPropertyChangedEventHandler(boom_IsVisibleChanged);
		}

		void boom_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			story.Begin();
		}
	}
}