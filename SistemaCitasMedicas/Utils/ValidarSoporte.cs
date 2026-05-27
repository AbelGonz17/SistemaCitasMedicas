namespace SistemaCitasMedicas.Utils
{
    public static class ValidarSoporte
    {
        public static void ValidarTextoObligatorio(string valor, string NombreCampo)
        {
            if(string.IsNullOrWhiteSpace(valor))
            {
                throw new ArgumentException($"El campo {NombreCampo} es obligatorio y no puede estar vacío.");
            }
        }

        public static void ValidarFechaFutura(DateTime fecha)
        {
            if(fecha <= DateTime.Now)
            {
                throw new ArgumentException("La fecha seleccionada no puede ser en el pasado ni en el momento actual.");
            }
        }
    }
}