using Naitv1.Services;
using System.Net;
using System.Net.Mail;

//Clase que hereda interface configura la comunicacion entre servicio SMTP y el destinatario y emisor
public class SmtpEmailService : IEmailServices
{
    public void Enviar(string destinatario, string asunto, string htmlCuerpo)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("TUCORREO@gmail.com", "TU_CLAVE_DE_APP"),
            EnableSsl = true,
        };

        var mensaje = new MailMessage
        {
            From = new MailAddress("TUCORREO@gmail.com", "NAITV"),
            Subject = asunto,
            Body = htmlCuerpo,
            IsBodyHtml = true
        };

        mensaje.To.Add(destinatario);

        smtpClient.Send(mensaje);
    }
}
