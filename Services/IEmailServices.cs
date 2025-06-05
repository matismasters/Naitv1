namespace Naitv1.Services
{
    public interface IEmailServices
    { 
        void Enviar(string destinatario, string asunto, string htmlCuerpo);
    }
}
