using SistemaCitasMedicas.Models;

namespace SistemaCitasMedicas.Interfaces
{
    public class CitaRepository : ICitaRepository
    {
        private readonly List<CitaMedica> _citas = new List<CitaMedica>();

        public bool ExisteColisionHoraria(int medicoId, DateTime fechaHora)
        {
            return _citas.Any(c => c.Medico.Id == medicoId && c.FechaHora == fechaHora && c.Estado == "Agendada");
        }

        public void Guardar(CitaMedica cita)
        {
            _citas.Add(cita);
        }

        public CitaMedica? ObtenerPorId(int id)
        {
            return _citas.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<CitaMedica> ObtenerPorMedico(int medicoId)
        {
            return _citas.Where(c => c.Medico.Id == medicoId);
        }

        public IEnumerable<CitaMedica> ObtenerPorPaciente(int pacienteId)
        {
            return _citas.Where(c => c.Paciente.Id == pacienteId);
        }

        public IEnumerable<CitaMedica> ObtenerTodas()
        {
            return _citas;
        }
    }
}