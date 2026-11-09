using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using ControlGastos.Backend.Models;

namespace ControlGastos.Backend.Persistence
{
    public class RepositorioGastosJson : IRepositorioGastos
    {
        private readonly string rutaArchivo;

        public RepositorioGastosJson(string rutaArchivo)
        {
            this.rutaArchivo = rutaArchivo;
        }

        public IList<Gasto> ObtenerTodos()
        {
            if (!File.Exists(rutaArchivo))
                return new List<Gasto>();

            using (FileStream stream = File.OpenRead(rutaArchivo))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Gasto>));
                return (List<Gasto>)serializer.ReadObject(stream);
            }
        }

        public void GuardarTodos(IList<Gasto> gastos)
        {
            string directorio = Path.GetDirectoryName(rutaArchivo);
            if (!string.IsNullOrEmpty(directorio))
                Directory.CreateDirectory(directorio);

            string archivoTemporal = rutaArchivo + ".tmp";
            using (FileStream stream = File.Create(archivoTemporal))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Gasto>));
                serializer.WriteObject(stream, new List<Gasto>(gastos));
            }

            if (File.Exists(rutaArchivo))
                File.Replace(archivoTemporal, rutaArchivo, null);
            else
                File.Move(archivoTemporal, rutaArchivo);
        }
    }
}
