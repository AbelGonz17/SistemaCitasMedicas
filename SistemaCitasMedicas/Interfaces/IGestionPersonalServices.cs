using SistemaCitasMedicas.Models;

namespace SistemaCitasMedicas.Interfaces
{
    public interface IGestionPersonalServices
    {
        IEnumerable<Medico> ObtenerMedicos();
        IEnumerable<Paciente> ObtenerPacientes();
        void RegistrarEspecialidad(Especialidad especialidad);
        void RegistrarMedico(Medico medico);
        void RegistrarPaciente(Paciente paciente);
    }
}