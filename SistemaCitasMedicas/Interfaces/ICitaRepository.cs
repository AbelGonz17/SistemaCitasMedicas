using SistemaCitasMedicas.Models;

namespace SistemaCitasMedicas.Interfaces
{
    public interface ICitaRepository
    {
        void Guardar(CitaMedica cita);
        CitaMedica? ObtenerPorId(int id);
        IEnumerable<CitaMedica> ObtenerTodas();
        IEnumerable<CitaMedica> ObtenerPorPaciente(int pacienteId);
        IEnumerable<CitaMedica> ObtenerPorMedico(int medicoId);
        bool ExisteColisionHoraria(int medicoId, DateTime fechaHora);
    }
}