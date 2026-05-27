namespace SistemaCitasMedicas.Models
{
    public class Medico
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public Especialidad EspecialidadAsignada { get; set; } = new();
    }
}