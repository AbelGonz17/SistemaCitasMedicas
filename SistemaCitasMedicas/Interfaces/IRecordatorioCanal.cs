using SistemaCitasMedicas.Models;

namespace SistemaCitasMedicas.Interfaces
{
    public interface IRecordatorioCanal
    {
        void Enviar(CitaMedica cita, string mensaje);
    }
}