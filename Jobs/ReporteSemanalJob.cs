using Quartz;
using Naitv1.Services;
using System;
using System.Threading.Tasks;

namespace Naitv1.Jobs
{
    public class ReporteSemanalJob : IJob
    {
        private readonly GeneradorReportesService _generadorReportesService;
        private readonly ConfiguracionReporteService _configReporteService;

        public ReporteSemanalJob(GeneradorReportesService generadorReportesService,ConfiguracionReporteService configReporteService)
        {
            _generadorReportesService = generadorReportesService;
            _configReporteService = configReporteService;
        }

        public Task Execute(IJobExecutionContext context)
        {

            ///La logica es que apartir de DateTime.Now.Date, es decir, la fecha de hoy (con hora en 00:00), 
            ///y a partir de ahí busca el próximo día que coincida con el config.DiaObjetivo
            var config = _configReporteService.Obtener();

            DayOfWeek dia = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), config.DiaObjetivo);
            DateTime fecha = DateTime.Now.Date;

            while (fecha.DayOfWeek != dia)
                fecha = fecha.AddDays(1);

            fecha = fecha.AddHours(config.Hora).AddMinutes(config.Minuto);

            _generadorReportesService.CrearRegistro(fecha, config.Destinatario, config.Asunto);
            return Task.CompletedTask;
        }
    }
}