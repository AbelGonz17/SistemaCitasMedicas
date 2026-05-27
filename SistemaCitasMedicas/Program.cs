using SistemaCitasMedicas.Infrastructure;
using SistemaCitasMedicas.Interfaces;
using SistemaCitasMedicas.Models;
using SistemaCitasMedicas.Services;

class Program
{
    private static GestionPersonalServices _personalService = null!;
    private static GestionCitasService _citasService = null!;
    private static List<Especialidad> _especialidadesGlobales = new List<Especialidad>();

    static void Main(string[] args)
    {
        Console.Title = "Sistema de Gestión de Citas Médicas - Clínica ITLA";

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        _personalService = new GestionPersonalServices();
        var citaRepository = new CitaRepository();
        var canalesActivos = new List<IRecordatorioCanal> { new NotificacionCorreo() };
        _citasService = new GestionCitasService(citaRepository, canalesActivos);

        CargarDatosSemilla();

        bool salir = false;
        while (!salir)
        {
            try
            {
                MostrarMenuPrincipal();
                string? opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1": 
                        MenuRegistrarPaciente();
                        break;
                    case "2": 
                        MenuRegistrarMedico();
                        break;
                    case "3": 
                        MenuRegistrarEspecialidad();
                        break;
                    case "4": 
                        MenuAgendarCita();
                        break;
                    case "5": 
                        MenuConsultarPorPaciente();
                        break;
                    case "6": 
                        MenuConsultarPorMedico();
                        break;
                    case "7": 
                        MenuCancelarCita();
                        break;
                    case "8":
                        MenuReprogramarCita();
                        break;
                    case "9":
                        salir = true;
                        Console.WriteLine("\n👋 Saliendo del sistema. ¡Hasta luego!");
                        break;
                    default:
                        ImprimirMensajeError("Opción no válida. Intente de nuevo.");
                        break;
                }
            }
            catch (Exception ex)
            {
                ImprimirMensajeError($"Ocurrió un error inesperado: {ex.Message}");
            }

            if (!salir)
            {
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    private static void MostrarMenuPrincipal()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("================================================================");
        Console.WriteLine("       SISTEMA DE GESTIÓN DE CITAS MÉDICAS - CLINICA ITLA       ");
        Console.WriteLine("================================================================");
        Console.ResetColor();
        Console.WriteLine("1.  Registrar Nuevo Paciente");
        Console.WriteLine("2.  Registrar Médico (Con asignación de especialidad)");
        Console.WriteLine("3.  Registrar Nueva Especialidad Médica");
        Console.WriteLine("4.  Agendar Cita Médica (Envía recordatorio automático)");
        Console.WriteLine("5.  Consultar Citas por Paciente");
        Console.WriteLine("6.  Consultar Citas por Médico");
        Console.WriteLine("7.  Cancelar una Cita");
        Console.WriteLine("8.  Reprogramar una Cita (Envía recordatorio)");
        Console.WriteLine("9.  Salir del Sistema");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("\nSeleccione una opción de operación (1-9): ");
        Console.ResetColor();
    }

    private static void MenuRegistrarPaciente()
    {
        Console.Clear();
        Console.WriteLine("--- 1. REGISTRAR NUEVO PACIENTE ---");

        Console.Write("Ingrese ID Único del Paciente: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { ImprimirMensajeError("ID no válido."); return; }

        Console.Write("Nombre Completo: ");
        string nombre = Console.ReadLine() ?? "";

        Console.Write("Teléfono: ");
        string telefono = Console.ReadLine() ?? "";

        Console.Write("Correo Electrónico: ");
        string correo = Console.ReadLine() ?? "";

        var nuevoPaciente = new Paciente { Id = id, NombreCompleto = nombre, Telefono = telefono, CorreoElectronico = correo };
        _personalService.RegistrarPaciente(nuevoPaciente);

        ImprimirMensajeExito("¡Paciente añadido de forma exitosa!");
    }

    private static void MenuRegistrarEspecialidad()
    {
        Console.Clear();
        Console.WriteLine("--- 3. REGISTRAR ESPECIALIDAD MÉDICA ---");

        Console.Write("Ingrese ID de Especialidad: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { ImprimirMensajeError("ID no válido."); return; }

        Console.Write("Nombre de la Especialidad (ej: Pediatría): ");
        string nombre = Console.ReadLine() ?? "";

        Console.Write("Descripción: ");
        string desc = Console.ReadLine() ?? "";

        var esp = new Especialidad { Id = id, Nombre = nombre, Descripcion = desc };
        _personalService.RegistrarEspecialidad(esp);
        _especialidadesGlobales.Add(esp);

        ImprimirMensajeExito($"¡Especialidad '{nombre}' catalogada correctamente!");
    }

    private static void MenuRegistrarMedico()
    {
        Console.Clear();
        Console.WriteLine("--- 2. REGISTRAR MÉDICO ---");

        if (!_especialidadesGlobales.Any())
        {
            ImprimirMensajeError("Primero debe registrar al menos una especialidad médica (Opción 3).");
            return;
        }

        Console.Write("Ingrese ID Único del Médico: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { ImprimirMensajeError("ID no válido."); return; }

        Console.Write("Nombre Completo del Médico: ");
        string nombre = Console.ReadLine() ?? "";

        Console.WriteLine("\nSeleccione la Especialidad a asignar (REQ 4):");
        for (int i = 0; i < _especialidadesGlobales.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_especialidadesGlobales[i].Nombre}");
        }
        Console.Write("Seleccione el número: ");
        if (!int.TryParse(Console.ReadLine(), out int index) || index <= 0 || index > _especialidadesGlobales.Count)
        {
            ImprimirMensajeError("Selección inválida.");
            return;
        }

        var especialidadSeleccionada = _especialidadesGlobales[index - 1];

        var nuevoMedico = new Medico { Id = id, NombreCompleto = nombre, EspecialidadAsignada = especialidadSeleccionada };
        _personalService.RegistrarMedico(nuevoMedico);

        ImprimirMensajeExito($"¡Dr(a). {nombre} registrado con éxito en {especialidadSeleccionada.Nombre}!");
    }

    private static void MenuAgendarCita()
    {
        Console.Clear();
        Console.WriteLine("--- 4. AGENDAR CITA MÉDICA ---");

        Console.Write("Ingrese ID numérico para la Cita: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { ImprimirMensajeError("ID inválido."); return; }

        var paciente = SeleccionarPaciente();
        if (paciente == null) return;

        var medico = SeleccionarMedico();
        if (medico == null) return;

        Console.Write("\nIngrese Fecha y Hora (Formato: AAAA-MM-DD HH:MM): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaHora))
        {
            ImprimirMensajeError("Formato de tiempo inválido.");
            return;
        }

        var cita = new CitaMedica { Id = id, Paciente = paciente, Medico = medico, FechaHora = fechaHora };

        try
        {
            _citasService.AgendarCita(cita);
            ImprimirMensajeExito("¡Cita agendada de forma exitosa! (Recordatorio enviado por Correo)");
        }
        catch (Exception ex)
        {
            ImprimirMensajeError(ex.Message);
        }
    }

    private static void MenuConsultarPorPaciente()
    {
        Console.Clear();
        Console.WriteLine("--- 5. CONSULTAR CITAS POR PACIENTE ---");
        var paciente = SeleccionarPaciente();
        if (paciente == null) return;

        var citas = _citasService.ConsultarPorPaciente(paciente.Id);
        VisualizarTablaCitas(citas);
    }

    private static void MenuConsultarPorMedico()
    {
        Console.Clear();
        Console.WriteLine("--- 6. CONSULTAR CITAS POR MÉDICO ---");
        var medico = SeleccionarMedico();
        if (medico == null) return;

        var citas = _citasService.ConsultarPorMedico(medico.Id);
        VisualizarTablaCitas(citas);
    }

    private static void MenuCancelarCita()
    {
        Console.Clear();
        Console.WriteLine("--- 7. CANCELAR CITA MÉDICA ---");
        Console.Write("Ingrese el ID de la cita que desea cancelar: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { ImprimirMensajeError("ID no válido."); return; }

        try
        {
            _citasService.CancelarCita(id);
            ImprimirMensajeExito("¡La cita médica fue cancelada y liberada del sistema!");
        }
        catch (Exception ex)
        {
            ImprimirMensajeError(ex.Message);
        }
    }

    private static void MenuReprogramarCita()
    {
        Console.Clear();
        Console.WriteLine("--- 8. REPROGRAMAR CITA ---");
        Console.Write("Ingrese el ID de la cita que desea reprogramar: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { ImprimirMensajeError("ID no válido."); return; }

        Console.Write("Ingrese la nueva Fecha y Hora (AAAA-MM-DD HH:MM): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime nuevaFecha))
        {
            ImprimirMensajeError("Formato de tiempo inválido.");
            return;
        }

        try
        {
            _citasService.ReprogramarCita(id, nuevaFecha);
            ImprimirMensajeExito("¡La cita fue reprogramada con éxito! (Nuevo recordatorio despachado)");
        }
        catch (Exception ex)
        {
            ImprimirMensajeError(ex.Message);
        }
    }

    #region UTILERÍAS DE CONTROL INTERACTIVO

    private static Paciente? SeleccionarPaciente()
    {
        var lista = _personalService.ObtenerPacientes().ToList();
        if (!lista.Any()) { ImprimirMensajeError("No hay pacientes registrados."); return null; }

        Console.WriteLine("\nPacientes:");
        for (int i = 0; i < lista.Count; i++)
            Console.WriteLine($"{i + 1}. [ID: {lista[i].Id}] - {lista[i].NombreCompleto}");

        Console.Write("Seleccione el número correlativo: ");
        if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= lista.Count) return lista[idx - 1];

        ImprimirMensajeError("Selección no válida."); return null;
    }

    private static Medico? SeleccionarMedico()
    {
        var lista = _personalService.ObtenerMedicos().ToList();
        if (!lista.Any()) { ImprimirMensajeError("No hay médicos registrados."); return null; }

        Console.WriteLine("\nMédicos:");
        for (int i = 0; i < lista.Count; i++)
            Console.WriteLine($"{i + 1}. [ID: {lista[i].Id}] - {lista[i].NombreCompleto} ({lista[i].EspecialidadAsignada.Nombre})");

        Console.Write("Seleccione el número correlativo: ");
        if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= lista.Count) return lista[idx - 1];

        ImprimirMensajeError("Selección no válida."); return null;
    }

    private static void VisualizarTablaCitas(IEnumerable<CitaMedica> citas)
    {
        Console.WriteLine("\n==========================================================================");
        Console.WriteLine(string.Format("| {0,-7} | {1,-15} | {2,-18} | {3,-16} | {4,-10} |", "CITA ID", "PACIENTE", "MÉDICO", "FECHA/HORA", "ESTADO"));
        Console.WriteLine("==========================================================================");
        var lista = citas.ToList();
        if (!lista.Any())
        {
            Console.WriteLine("        --- No se registran citas bajo este criterio ---                  ");
        }
        else
        {
            foreach (var c in lista)
                Console.WriteLine(string.Format("| {0,-7} | {1,-15} | {2,-18} | {3,-16:g} | {4,-10} |", c.Id, c.Paciente.NombreCompleto, c.Medico.NombreCompleto, c.FechaHora, c.Estado));
        }
        Console.WriteLine("==========================================================================");
    }

    private static void CargarDatosSemilla()
    {
        var cardiologia = new Especialidad { Id = 1, Nombre = "Cardiología", Descripcion = "Cuidado del corazón" };
        _personalService.RegistrarEspecialidad(cardiologia);
        _especialidadesGlobales.Add(cardiologia);

        var medico = new Medico { Id = 101, NombreCompleto = "Dr. Francis Ramírez", EspecialidadAsignada = cardiologia };
        _personalService.RegistrarMedico(medico);

        var paciente = new Paciente { Id = 1, NombreCompleto = "Abel González", Telefono = "8095551234", CorreoElectronico = "abel.gonzalez@itla.edu.do" };
        _personalService.RegistrarPaciente(paciente);
    }

    private static void ImprimirMensajeError(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n⚠ [ERROR]: {msg}");
        Console.ResetColor();
    }

    private static void ImprimirMensajeExito(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n✔ [ÉXITO]: {msg}");
        Console.ResetColor();
    }

    #endregion
}