﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace PlanetTerror
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public App()
		{
			Setting.Initialize();
		}
	}
}
