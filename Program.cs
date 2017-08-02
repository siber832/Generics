using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Metainformacion
{
    /// <summary>
    /// Java : Annotation
    /// C# : Attribute
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var people = new List<Person>()
            {
                new Person(){Nombre = "Jorge", Id=1, Surname="Lopez"},
                //Compila, pero el nombre dentro de la base de datos no puede ser nulo.
                new Person(){Id=2, Surname="Hurtado"},
            };

            // Hacemos genericos todos los objetos, para tratarlos a todos con el mismo código
            var generics = ToGeneric(people);
            validateGeneric(generics);
        }

        /// <summary>
        /// Validamos una lista de objetos genericos
        /// </summary>
        /// <param name="genericList"></param>
        static void validateGeneric(List<Generic> genericList)
        {
            Console.WriteLine("Generando tablas...");

            //Tenemos que validar los elementos antes de guardarlos en la base de datos
            foreach (var generic in genericList)
            {
                string values = String.Empty;
                foreach(PropertyInfo prop in generic.type.GetProperties())
                {
                    //Console.WriteLine("Tipo: " + prop.PropertyType.Name);
                    //Console.WriteLine("Nombre: " + prop.Name);
                    //Console.WriteLine("Valor: " + prop.GetValue(generic.obj));
                    //Console.WriteLine();

                    if (prop.PropertyType.Name.Equals("String"))
                    {
                        //values += "\"" + prop.GetValue(generic.obj) + "\",";
                        values += "'" + prop.GetValue(generic.obj) + "',";
                    } else
                    {
                        values += prop.GetValue(generic.obj) + ",";
                    }
                }

                string tabla = "INSERT INTO T" + generic.type.Name + " VALUES (" + values.Substring(0, values.Length-1) + ")";
                Console.WriteLine(tabla);
            }

            Console.ReadLine();
        }

        /// <summary>
        /// Hacemos una lista de objetos generica a partir de una lista de cualquier tipo
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        static List<Generic> ToGeneric(IEnumerable<Object> list)
        {

            return list.Select(c => new Generic { obj = c, type = c.GetType() }).ToList();
        }
    }

    //Clase generica para pasar todos los objetos de cualquier tipo a un objeto de tipo generico
    public class Generic
    {
        public Object obj { get; set; }
        public Type type { get; set; }
    }

    /// <summary>
    /// Limitamos la longitud del nombre para la base de datos
    /// en una longitud no superior a 50 cartacteres
    /// </summary>
    public class Person
    {
        public int Id { get; set; }
        [StringDataBase(Length = 50)]
        public string Nombre { get; set; }
        [StringDataBase(Length = 15, Required = false)]
        public string Surname { get; set; }
    }

    /// <summary>
    /// Definición para el atributo (Anotacion)
    /// * Heredan de Attribute
    /// * Terminan su nombre por "Atribute"
    /// </summary>
    public class StringDataBaseAttribute : Attribute, IValid
    {
        public StringDataBaseAttribute() { this.Required = true; }

        public int Length { get; set; }
        public bool Required { get; set; }

        public bool IsValid(String value)
        {
            //TODO: (Implementar)
            return true;
        }
    }

    /// <summary>
    /// Interfaz para comprobar si un valor es valido o no
    /// </summary>
    public interface IValid
    {
        bool IsValid(String value);
    }
}