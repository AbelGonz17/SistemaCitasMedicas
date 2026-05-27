using SistemaCitasMedicas.Models;
using SistemaCitasMedicas.Utils;

namespace SistemaCitasMedicas.Services
{
    public class GestionPersonalServices
    {
        private readonly List<Paciente> _pacientes = new List<Paciente>();
        private readonly List<Medico> _medicos = new List<Medico>();
        private readonly List<Especialidad> _especialidades = new List<Especialidad>();

        public void RegistrarPaciente(Paciente paciente)
        {
            ValidarSoporte.ValidarTextoObligatorio(paciente.NombreCompleto, "Nombre del Paciente");
            ValidarSoporte.ValidarTextoObligatorio(paciente.Telefono, "Teléfono del Paciente");
            ValidarSoporte.ValidarTextoObligatorio(paciente.CorreoElectronico, "Correo Electrónico del Paciente");

            _pacientes.Add(paciente);
        }

        public void RegistrarEspecialidad(Especialidad especialidad)
        {
            ValidarSoporte.ValidarTextoObligatorio(especialidad.Nombre, "Nombre de la Especialidad");
            _especialidades.Add(especialidad);
        }

        public void RegistrarMedico(Medico medico)
        {
            ValidarSoporte.ValidarTextoObligatorio(medico.NombreCompleto, "Nombre del Médico");

            if(medico.EspecialidadAsignada == null || string.IsNullOrEmpty(medico.EspecialidadAsignada.Nombre))
            {
                throw new ArgumentException("El médico debe tener una especialidad valida asignada");
            }

            _medicos.Add(medico);
        }

        public IEnumerable<Paciente> ObtenerPacientes() => _pacientes;
        public IEnumerable<Medico> ObtenerMedicos() => _medicos;
    }
}