using Cookie_Editor.Services;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace Cookie_Editor{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
       
     
        static void Main()
        {
            ServiceBase[] ServicesToRun = new ServiceBase[]
            {
                new Cookie_Editor()            
            };
            ServiceBase.Run(ServicesToRun);                      
        }
    }
}
