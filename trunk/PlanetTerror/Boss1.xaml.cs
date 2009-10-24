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

using PlanetTerror.Util;


namespace PlanetTerror
{
	public partial class Boss1 : UserControl
	{
		public Boss1()
		{
			this.InitializeComponent();

			background_Image.SetVisible(false);

			Loaded += new RoutedEventHandler(Boss1_Loaded);

		}

		//===============================================================================================================================================
		//	핸들러
		void Boss1_Loaded(object sender, RoutedEventArgs e)
		{
			VisualStateManager.GoToState(this, "MoveState", false);
		}
	}
}