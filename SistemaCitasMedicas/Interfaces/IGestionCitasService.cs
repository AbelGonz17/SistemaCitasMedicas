using SistemaCitasMedicas.Models;

namespace SistemaCitasMedicas.Interfaces
{
    public interface IGestionCitasService
    {
        void AgendarCita(CitaMedica cita);
        void CancelarCita(int citaId);
        IEnumerable<CitaMedica> ConsultarPorMedico(int medicoId);
        IEnumerable<CitaMedica> ConsultarPorPaciente(int pacienteId);
        void ReprogramarCita(int citaId, DateTime nuevaFechaHora);
    }
}