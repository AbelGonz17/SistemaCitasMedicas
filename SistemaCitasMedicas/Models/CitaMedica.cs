namespace SistemaCitasMedicas.Models
{
    public class CitaMedica
    {
        public int Id { get; set; }
        public Paciente Paciente { get; set; } = new();
        public Medico Medico { get; set; } = new();
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } = "Agendada";
    }
}