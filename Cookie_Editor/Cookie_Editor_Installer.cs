using System;

using System.ComponentModel;

using System.Configuration.Install;

using System.ServiceProcess;




namespace Cookie_Editor
{
    
	[RunInstaller(true)]
    
	public partial class Cookie_Editor_Installer : Installer
    
	{
        
		ServiceInstaller _service_installer = new ServiceInstaller();
        
		ServiceProcessInstaller _process_installer = new ServiceProcessInstaller();
        

		public Cookie_Editor_Installer()
        
		{
            
			InitializeComponent();
                  
			_process_installer.Account = ServiceAccount.LocalSystem; 	// тип учетной записи от которой запускать
 приложение           
            
			_process_installer.Password = null;
            
			_process_installer.Username = null;

            
         
			_service_installer.StartType = ServiceStartMode.Manual; 				// вид запуска 
            
			_service_installer.ServiceName = "listener"; 						// имя службы в списке
            
			_service_installer.Description = "Приложение просмотра cookie-файлов браузера."; 	// описание

            
			_service_installer.AfterInstall += _installer_AfterInstall; 				// метод работы после установки
            
			_service_installer.AfterRollback += _installer_AfterRollback; 				// метод работы после ошибки
            
			_service_installer.AfterUninstall += _installer_AfterUninstall; 			// метод работы после удаления

            

			Installers.Add(_service_installer);
            
			Installers.Add(_process_installer);
           



        
		}


        

		private void _installer_AfterUninstall(object sender, InstallEventArgs e)
        
		{
            
			Console.ForegroundColor = ConsoleColor.Blue;
            
			Console.WriteLine("Приложение удалено!", ConsoleColor.Blue);
            
			Console.ForegroundColor = ConsoleColor.White;
        
		}

        
		private void _installer_AfterRollback(object sender, InstallEventArgs e)
        
		{
            
			Console.ForegroundColor = ConsoleColor.Red;
            
			Console.WriteLine("Не удалось удалить приложение! Попробуйте позже.");
            
			Console.ForegroundColor = ConsoleColor.White;
        
		}

        
		private void _installer_AfterInstall(object sender, InstallEventArgs e)
        
		{
            
			Console.ForegroundColor = ConsoleColor.Green;
            
			Console.WriteLine("Приложение успешно установлено!");
            
			Console.ForegroundColor = ConsoleColor.White;
        
		}
    
	}

}
