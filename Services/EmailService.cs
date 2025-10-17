using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using MAC.Services;

namespace MAC.Services
{

    public class EmailService
    {
        public async Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpo)
        {
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("Sistema de Momentos Contables", "micaela.reyes@uabc.edu.mx"));
            mensaje.To.Add(new MailboxAddress("", destinatario));
            mensaje.Subject = asunto;

            mensaje.Body = new TextPart("plain")
            {
                Text = cuerpo
            };

            using var cliente = new SmtpClient();
            await cliente.ConnectAsync("smtp.office365.com", 587, SecureSocketOptions.StartTls);
            await cliente.AuthenticateAsync("mreyesr@baja.gob.mx", "mr3y3s#23");
            await cliente.SendAsync(mensaje);
            await cliente.DisconnectAsync(true);
        }
    }
}