using SistemaCitasMedicas.Interfaces;
using SistemaCitasMedicas.Models;

namespace SistemaCitasMedicas.Infrastructure
{
    public class NotificacionCorreo : IRecordatorioCanal
    {
        public void Enviar(CitaMedica cita, string mensaje)
        {
            Console.WriteLine($"[CANAL CORREO] Notificación enviada con éxito a: {cita.Paciente.CorreoElectronico}");
            Console.WriteLine($"[MENSAJE]: \"{mensaje}\"");
            Console.WriteLine(new string('-', 50));
        }
    }
}