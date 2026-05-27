using SistemaCitasMedicas.Interfaces;
using SistemaCitasMedicas.Models;
using SistemaCitasMedicas.Utils;

namespace SistemaCitasMedicas.Services
{
    public class GestionCitasService
    {
        private readonly ICitaRepository _citaRepository;
        private readonly IEnumerable<IRecordatorioCanal> _canalesNotificacion;

        public GestionCitasService(ICitaRepository citaRepository, IEnumerable<IRecordatorioCanal> canalesNotificacion)
        {
            _citaRepository = citaRepository;
            _canalesNotificacion = canalesNotificacion;
        }

        public void AgendarCita(CitaMedica cita)
        {
            ValidarSoporte.ValidarFechaFutura(cita.FechaHora);

            if(_citaRepository.ExisteColisionHoraria(cita.Medico.Id, cita.FechaHora))
                throw new InvalidOperationException($"El {cita.Medico.NombreCompleto} tiene una cita agendada en ese horario.");
            
            cita.Estado = "Agendada";
            _citaRepository.Guardar(cita);

            NotificarPaciente(cita, $"Su cita ha sido agendada con éxito para el {cita.FechaHora:dd/MM/yyyy hh:mm tt}.");
        }

        public void CancelarCita(int citaId)
        {
            var cita = _citaRepository.ObtenerPorId(citaId);

            if (cita == null)
               throw new KeyNotFoundException("Cita no encontrada.");
            
            cita.Estado = "Cancelada";
            NotificarPaciente(cita, "Su cita médica ha sido cancelada.");
        }

        public void ReprogramarCita(int citaId, DateTime nuevaFechaHora)
        {
            ValidarSoporte.ValidarFechaFutura(nuevaFechaHora);

            var cita = _citaRepository.ObtenerPorId(citaId);

            if (cita == null)
                throw new KeyNotFoundException("Cita no encontrada.");

            if(_citaRepository.ExisteColisionHoraria(cita.Medico.Id, nuevaFechaHora))
                throw new InvalidOperationException($"El {cita.Medico.NombreCompleto} tiene una cita agendada en ese horario.");
            
            cita.FechaHora = nuevaFechaHora;
            cita.Estado = "Reprogramada";

            NotificarPaciente(cita, $"Su cita ha sido reprogramada para el {cita.FechaHora:dd/MM/yyyy hh:mm tt}.");
        }

        public IEnumerable<CitaMedica> ConsultarPorMedico(int medicoId)
        {
            return _citaRepository.ObtenerPorMedico(medicoId);
        }

        public IEnumerable<CitaMedica> ConsultarPorPaciente(int pacienteId)
        {
            return _citaRepository.ObtenerPorPaciente(pacienteId);
        }

        private void NotificarPaciente(CitaMedica cita, string cuerpoMensaje)
        {
            string mensajeFormateado = $"Estimado(a) {cita.Paciente.NombreCompleto}. " + cuerpoMensaje;

            foreach( var canal  in _canalesNotificacion)
            {
                canal.Enviar(cita, mensajeFormateado);
            }
        }
    }
}