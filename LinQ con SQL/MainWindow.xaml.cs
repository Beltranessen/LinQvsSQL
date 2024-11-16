using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;

namespace LinQ_con_SQL
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UniversidadDataContext uniDC;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["UniversidadConnectionString"].ConnectionString;
            uniDC = new UniversidadDataContext(connectionString);

            MessageBox.Show($"Aplicacion conectada a {connectionString}");
            Console.WriteLine("Rama master");
            Console.WriteLine("Rama Sucripción Local");
            Console.WriteLine("Rama local Feature/Contacto");
            AgregarUniversidades();
            AgregarEstudiantes();
            AgregarMaterias();
            AgregarMateriasAEstudiantes();
            VisualizarUniversidadParaEstudiantes();
            RecorrerUniversidadesPorEstudiante();

            //MessageBox.Show("Obteniendo estudiantes de la universidad de Alicante");
            GetStudentsFromUni("Alicante");

            ActualizarNombrePier();

            EliminarMateriaIngleSandro();
        }

        public void AgregarUniversidades()
        {
            uniDC.ExecuteCommand("delete Universidad; DBCC CHECKIDENT('Universidad',RESEED,0)");

            Universidad universidad = new Universidad() { Nombre = "Moncloa" };
            uniDC.Universidad.InsertOnSubmit(universidad);

            universidad = new Universidad() { Nombre = "Autónoma" };
            uniDC.Universidad.InsertOnSubmit(universidad);

            universidad = new Universidad() { Nombre = "Alicante" };
            uniDC.Universidad.InsertOnSubmit(universidad);

            universidad = new Universidad() { Nombre = "Berlín" };
            uniDC.Universidad.InsertOnSubmit(universidad);

            universidad = new Universidad() { Nombre = "Buenos Aires" };
            uniDC.Universidad.InsertOnSubmit(universidad);

            universidad = new Universidad() { Nombre = "Barcelona" };
            uniDC.Universidad.InsertOnSubmit(universidad);

            universidad = new Universidad() { Nombre = "París" };
            uniDC.Universidad.InsertOnSubmit(universidad);

            uniDC.SubmitChanges();

            DGPrincipal.ItemsSource = uniDC.Universidad;
        }

        public void AgregarEstudiantes()
        {
            uniDC.ExecuteCommand("if exists(select top 1 Nombre from Estudiante) delete Estudiante; DBCC CHECKIDENT('Estudiante', RESEED,0)");

            // Usando expresiones lambda
            Universidad uni = uniDC.Universidad.First(x => x.Nombre == "Moncloa");
            Estudiante estu = new Estudiante() { Nombre = "Elsa", Genero = "M", UniversidadId = uni.Id };
            uniDC.Estudiante.InsertOnSubmit(estu);


            estu = new Estudiante() { Nombre = "Alfon", Genero = "H", UniversidadId = (int)Entorno.EUniversidades.Alicante };
            uniDC.Estudiante.InsertOnSubmit(estu);

            estu = new Estudiante() { Nombre = "Carmen", Genero = "M", UniversidadId = (int)Entorno.EUniversidades.Alicante };
            uniDC.Estudiante.InsertOnSubmit(estu);

            estu = new Estudiante() { Nombre = "Ana", Genero = "M", UniversidadId = (int)Entorno.EUniversidades.Autonoma };
            uniDC.Estudiante.InsertOnSubmit(estu);

            uni = uniDC.Universidad.First(u => u.Id.Equals((int)Entorno.EUniversidades.Autonoma));
            uniDC.Estudiante.InsertOnSubmit(new Estudiante() { Nombre = "Bea", Genero = "M", UniversidadId = uni.Id });

            //Añadir a una lista para su posterior inserción
            List<Estudiante> listaEstu = new List<Estudiante>();
            listaEstu.Add(new Estudiante() { Nombre = "Pier", Genero = "H", UniversidadId = (int)Entorno.EUniversidades.Paris });
            listaEstu.Add(new Estudiante() { Nombre = "Sandro", Genero = "H", UniversidadId = (int)Entorno.EUniversidades.BuenosAires });
            listaEstu.Add(new Estudiante() { Nombre = "Sandra Mayer", Genero = "M", UniversidadId = (int)Entorno.EUniversidades.Berlin });

            //Asigno a la propiedad Universidad el objeto universidad creado previamente
            uni = uniDC.Universidad.First(u => u.Id.Equals((int)Entorno.EUniversidades.Paris));
            listaEstu.Add(new Estudiante() { Nombre = "Carlton", Genero = "H", Universidad = uni });

            //Insertar registros desde una lista
            uniDC.Estudiante.InsertAllOnSubmit(listaEstu);

            uniDC.SubmitChanges();

            DGPrincipal.ItemsSource = uniDC.Estudiante;

        }

        public void AgregarMaterias()
        {
            List<Materia> listaM = new List<Materia>();

            uniDC.ExecuteCommand("delete Materia; DBCC CHECKIDENT('Materia', RESEED,0)");

            Materia mater = new Materia() { Nombre = Entorno.EMaterias.BaseDeDatos.ToString() };
            listaM.Add(mater);
            mater = new Materia() { Nombre = Entorno.EMaterias.ComercioExterior.ToString() };
            listaM.Add(mater);
            mater = new Materia() { Nombre = Entorno.EMaterias.DerechoRomano.ToString() };
            listaM.Add(mater);
            mater = new Materia() { Nombre = Entorno.EMaterias.DibujoTenico.ToString() };
            listaM.Add(mater);
            mater = new Materia() { Nombre = Entorno.EMaterias.Escultura.ToString() };
            listaM.Add(mater);
            mater = new Materia() { Nombre = Entorno.EMaterias.Estadistica.ToString() };
            listaM.Add(mater);
            mater = new Materia() { Nombre = Entorno.EMaterias.HistoriaDelArte.ToString() };
            listaM.Add(mater);
            mater = new Materia() { Nombre = Entorno.EMaterias.Ingles.ToString() };
            listaM.Add(mater);
            mater = new Materia() { Nombre = Entorno.EMaterias.Pintura.ToString() };
            listaM.Add(mater);
            mater = new Materia() { Nombre = Entorno.EMaterias.ProgramacionFundamentos.ToString() };
            listaM.Add(mater);
            mater = new Materia() { Nombre = Entorno.EMaterias.RelacionesLaborales.ToString() };
            listaM.Add(mater);

            uniDC.Materia.InsertAllOnSubmit(listaM);

            uniDC.SubmitChanges();

            DGPrincipal.ItemsSource = uniDC.Materia;

        }

        public void AgregarMateriasAEstudiantes()
        {
            uniDC.ExecuteCommand("delete EstudianteMateria; DBCC CHECKIDENT('EstudianteMateria', RESEED,0)");

            List<EstudianteMateria> le = new List<EstudianteMateria>();

            // Inserción de materias a estudiantes
            Estudiante estudiante = uniDC.Estudiante.First(e => e.Nombre.Equals("Elsa"));
            Materia mat = uniDC.Materia.First(m => m.Nombre.Equals(Entorno.EMaterias.Pintura.ToString()));

            EstudianteMateria em = new EstudianteMateria() { Estudiante = estudiante, Materia = mat };
            le.Add(em);

            mat = uniDC.Materia.First(m => m.Nombre.Equals(Entorno.EMaterias.Escultura.ToString()));
            em = new EstudianteMateria() { Estudiante = estudiante, Materia = mat };
            le.Add(em);

            mat = uniDC.Materia.First(m => m.Nombre.Equals(Entorno.EMaterias.DibujoTenico.ToString()));
            em = new EstudianteMateria() { Estudiante = estudiante, Materia = mat };
            le.Add(em);

            mat = uniDC.Materia.First(m => m.Nombre.Equals(Entorno.EMaterias.HistoriaDelArte.ToString()));
            em = new EstudianteMateria() { Estudiante = estudiante, Materia = mat }; le.Add(em);

            mat = uniDC.Materia.First(m => m.Nombre.Equals(Entorno.EMaterias.Ingles.ToString()));
            em = new EstudianteMateria() { Estudiante = estudiante, Materia = mat }; le.Add(em);

            estudiante = uniDC.Estudiante.First(e => e.Nombre.Equals("Carmen"));
            mat = uniDC.Materia.First(m => m.Nombre.Equals(Entorno.EMaterias.BaseDeDatos.ToString()));
            em = new EstudianteMateria() { Estudiante = estudiante, Materia = mat }; le.Add(em);
            mat = uniDC.Materia.First(m => m.Nombre.Equals(Entorno.EMaterias.ProgramacionFundamentos.ToString()));
            em = new EstudianteMateria() { Estudiante = estudiante, Materia = mat }; le.Add(em);

            estudiante = uniDC.Estudiante.First(e => e.Nombre.Equals("Alfon"));
            mat = uniDC.Materia.First(m => m.Nombre.Equals(Entorno.EMaterias.BaseDeDatos.ToString()));
            em = new EstudianteMateria() { Estudiante = estudiante, Materia = mat }; le.Add(em);
            mat = uniDC.Materia.First(m => m.Nombre.Equals(Entorno.EMaterias.ProgramacionFundamentos.ToString()));
            em = new EstudianteMateria() { Estudiante = estudiante, Materia = mat }; le.Add(em);

            estudiante = uniDC.Estudiante.First(e => e.Nombre.Equals("Pier"));
            mat = uniDC.Materia.First(m => m.Nombre.Equals(Entorno.EMaterias.DerechoRomano.ToString()));
            em = new EstudianteMateria() { Estudiante = estudiante, Materia = mat }; le.Add(em);

            estudiante = uniDC.Estudiante.First(e => e.Nombre.Equals("Bea"));
            mat = uniDC.Materia.First(m => m.Nombre.Equals(Entorno.EMaterias.HistoriaDelArte.ToString()));
            em = new EstudianteMateria() { Estudiante = estudiante, Materia = mat }; le.Add(em);

            estudiante = uniDC.Estudiante.First(e => e.Nombre.Equals("Ana"));
            mat = uniDC.Materia.First(m => m.Nombre.Equals(Entorno.EMaterias.DerechoRomano.ToString()));
            em = new EstudianteMateria() { Estudiante = estudiante, Materia = mat }; le.Add(em);
            mat = uniDC.Materia.First(m => m.Nombre.Equals(Entorno.EMaterias.Ingles.ToString()));
            em = new EstudianteMateria() { Estudiante = estudiante, Materia = mat }; le.Add(em);

            estudiante = uniDC.Estudiante.First(e => e.Nombre.Equals("Sandro"));
            mat = uniDC.Materia.First(m => m.Nombre.Equals(Entorno.EMaterias.RelacionesLaborales.ToString()));
            em = new EstudianteMateria() { Estudiante = estudiante, Materia = mat }; le.Add(em);
            mat = uniDC.Materia.First(m => m.Nombre.Equals(Entorno.EMaterias.Ingles.ToString()));
            em = new EstudianteMateria() { Estudiante = estudiante, Materia = mat }; le.Add(em);

            uniDC.EstudianteMateria.InsertAllOnSubmit(le);

            uniDC.SubmitChanges();

            DGPrincipal.ItemsSource = uniDC.EstudianteMateria;

        }

        public Universidad ObtenerUniversidadPorEstudiante(string NombreEstu)
        {
            //var unilist = from uni in uniDC.Universidad 
            //          join estu in uniDC.Estudiante on uni.Id.equals(estu.UniversidadId)
            //          sele
            Estudiante estudiante = uniDC.Estudiante.First(e => e.Nombre.Equals(NombreEstu));
            Universidad uni = uniDC.Universidad.First(u => u.Id.Equals(estudiante.UniversidadId));
            return uni;
        }

        public void RecorrerUniversidadesPorEstudiante()
        {
            var u = from uni in uniDC.Universidad
                    join estu in uniDC.Estudiante on uni.Id equals estu.UniversidadId
                    join matesEstu in uniDC.EstudianteMateria on estu.Id equals matesEstu.EstudianteId
                    join mat in uniDC.Materia on matesEstu.MateriaId equals mat.Id
                    select new
                    {
                        Estudiante = estu.Nombre,
                        Universidad = uni.Nombre,
                        Materia = mat.Nombre
                    };

            DGPrincipal.ItemsSource = u;
        }

        public void VisualizarUniversidadParaEstudiantes()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Estudiante estudiante in uniDC.Estudiante)
            {
                sb.Append($"El estudiante {estudiante.Nombre} estudia en la uni {ObtenerUniversidadPorEstudiante(estudiante.Nombre).Nombre}");
                sb.Append("\n");

            }
            Console.WriteLine(sb.ToString());
        }


        public void GetStudentsFromUni(string uni)
        {

            var estuAlicante = from est in uniDC.Estudiante
                               join un in uniDC.Universidad on est.UniversidadId equals un.Id
                               join me in uniDC.EstudianteMateria on est.Id equals me.EstudianteId
                               join mat in uniDC.Materia on me.MateriaId equals mat.Id
                               where est.Universidad.Nombre.Equals(uni)
                               select new
                               {
                                   Estu = est.Nombre,
                                   Uni = un.Nombre,
                                   Asig = mat.Nombre
                               };

            var estuLucentum = from em in uniDC.EstudianteMateria
                               join estu in uniDC.Estudiante on em.EstudianteId equals estu.Id
                               where estu.Universidad.Nombre == uni
                               select new
                               {
                                   Uni = estu.Universidad.Nombre,
                                   Materia = em.Materia.Nombre
                               };

            DGPrincipal.ItemsSource = estuAlicante;
        }

        //Actualizar datos
        public void ActualizarNombrePier()
        {
            Estudiante estu = uniDC.Estudiante.FirstOrDefault(e => e.Nombre.Equals("Pier"));
            if (estu != null)
            {
                estu.Nombre = "Pier Luiggi";
                uniDC.SubmitChanges();
            }

            DGPrincipal.ItemsSource = uniDC.Estudiante.ToList();
        }

        public void EliminarMateriaIngleSandro()
        {
            //Estudiante estudiante = uniDC.Estudiante.FirstOrDefault(e => e.Nombre.Equals("Ana"));
            //uniDC.Estudiante.DeleteOnSubmit(estudiante);

            List<Estudiante> listaBorrados = uniDC.Estudiante.ToList().Where(x => x.Nombre != "Ana").ToList();

            uniDC.Estudiante.DeleteAllOnSubmit(listaBorrados);

            uniDC.SubmitChanges();

            DGPrincipal.ItemsSource = from estu in uniDC.Estudiante
                                      where estu.UniversidadId != 0
                                      select estu;

        }

    }
}
