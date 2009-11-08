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

using PlanetTerror.Util;


namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Boom
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class Boom : UserControl
	{
		//===============================================================================================================================================
		//	프로퍼티
		public bool Done { get; protected set; }

		//===============================================================================================================================================
		//	필드
		Storyboard story;
		
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Boom()
		{
			this.InitializeComponent();

			Done = false;
			story = Resources["Core_Boom0_Storyboard"] as Storyboard;
			story.Completed += new EventHandler(story_Completed);

			IsVisibleChanged += new DependencyPropertyChangedEventHandler(boom_IsVisibleChanged);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void story_Completed(object sender, EventArgs e)
		{
			Done = true;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void boom_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if( this.GetVisible() )
			{
				story.Begin();
				Done = false;
			}			
		}
	}
}