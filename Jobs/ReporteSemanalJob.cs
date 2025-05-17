using Quartz;
using Naitv1.Services;
using System;
using System.Threading.Tasks;

namespace Naitv1.Jobs
{
    public class ReporteSemanalJob : IJob
    {
        private readonly GeneradorReportesService _generadorReportesService;

        // Inyectamos el servicio GeneradorReportesService
        public ReporteSemanalJob(GeneradorReportesService generadorReportesService)
        {
            _generadorReportesService = generadorReportesService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            // Obtener la fecha programada (lunes a las 9 AM)
            DateTime fechaProgramada = DateTime.Now.Date.AddDays(1); // Fecha del lunes
            while (fechaProgramada.DayOfWeek != DayOfWeek.Monday)
            {
                fechaProgramada = fechaProgramada.AddDays(1); // Sumar días hasta llegar al lunes
            }
            fechaProgramada = fechaProgramada.AddHours(9); // Ajustar la hora a las 9 AM

            string destinatario = "Nelsonalvarez_2001@hotmail.com";
            string asunto = "Resumen Semanal de KPIs";
            _generadorReportesService.CrearRegistro(fechaProgramada, destinatario, asunto);

            return Task.CompletedTask;
        }
    }
}