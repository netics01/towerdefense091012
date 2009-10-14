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

namespace Galaga
{
	/// <summary>
	/// Interaction logic for UI_Botton_AreaControl.xaml
	/// </summary>
	public partial class UI_Botton_AreaControl : UserControl
	{
		protected int _menu_number = 0;
		
		public UI_Botton_AreaControl()
		{
			this.InitializeComponent();	
			UpdateState();
		}
		
		protected void UpdateState()
		{
			if ( _menu_number == 0 )	{ 
				VisualStateManager.GoToState(this, "none", true);
			}
			if ( _menu_number == 1 )	{ 
				VisualStateManager.GoToState(this, "Rollover", true);
			}
			if ( _menu_number == 2 )	{ 
				VisualStateManager.GoToState(this, "OnRelese", true);
			}
	}
		protected void bt_01(object sender, MouseEventArgs e) {
			_menu_number = 0;
			UpdateState();
		}
		
		protected void bt_02(object sender, MouseEventArgs e) {
			_menu_number = 1;
			UpdateState();
		}
}
}

