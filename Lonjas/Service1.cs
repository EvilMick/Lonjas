using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace Lonjas
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer1 = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer1 = new Timer();
            this.timer1.Interval = 180000;
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
            timer1.Enabled = true;
            Libreria.writeError("Actualizacion de precios de Albacete y Talavera.");
        }
        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            Extractor ex = new Extractor();
            ex.Albacete();
            ex.DatosVac();
            Libreria.writeError("Lectura completada.");

        }

        protected override void OnStop()
        {
            timer1.Enabled = false;
            Libreria.writeError("El servicio se ha detenido");
        }
    }
}
