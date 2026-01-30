using ControladorEntities;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace ControladorData
{
    public static class Archivos
    {
        public static List<CategoriasEntities> LeerCategoriasEnJson()
        {
            string directorioDestino = "../ControladorData/DataBase";
            string rutaCompleta = Path.Combine(directorioDestino, "Categorias.json");
            string rutaAbsolutaDestino = Path.GetFullPath(rutaCompleta);

            if (File.Exists(rutaAbsolutaDestino))
            {
                string json = File.ReadAllText(rutaAbsolutaDestino);
                return JsonConvert.DeserializeObject<List<CategoriasEntities>>(json);
            }
            return new List<CategoriasEntities>();
        }
        public static CategoriasEntities GuardarCategoriaEnJson(CategoriasEntities data)
        {
            var listado = LeerCategoriasEnJson();

            if (data.id != 0)
            {
                listado.RemoveAll(b => b.id == data.id);
            }
            else
            {
                int nuevoId = listado.Any() ? listado.Max(b => b.id) + 1 : 1;
                data.id = nuevoId;
            }
            listado.Add(data);
            string directorioDestino = "../ControladorData/DataBase";
            string rutaCompleta = Path.Combine(directorioDestino, "Categorias.json");
            string rutaAbsolutaDestino = Path.GetFullPath(rutaCompleta);

            Directory.CreateDirectory(Path.GetDirectoryName(rutaAbsolutaDestino));
            string json = JsonConvert.SerializeObject(listado, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(rutaAbsolutaDestino, json);
            return data;
        }
        public static bool EliminarCategoria(int id)
        {
            var categorias = LeerCategoriasEnJson();
            var categoria=categorias.FirstOrDefault(b=>b.id == id);
            if (categoria != null)
            {
                categorias.Remove(categoria);
                string directorioDestino = "../ControladorData/DataBase";
                string rutaCompleta = Path.Combine(directorioDestino, "Categorias.json");
                string rutaAbsolutaDestino = Path.GetFullPath(rutaCompleta);
                string json = JsonConvert.SerializeObject(categorias, Formatting.Indented);
                File.WriteAllText(rutaAbsolutaDestino, json);
                return true;
            }
            return false;
        }
        public static List<ObjetivosEntities> LeerObjetivosEnJson()
        {
            string directorioDestino = "../ControladorData/DataBase";
            string rutaCompleta = Path.Combine(directorioDestino, "Objetivos.json");
            string rutaAbsolutaDestino = Path.GetFullPath(rutaCompleta);

            if (File.Exists(rutaAbsolutaDestino))
            {
                string json = File.ReadAllText(rutaAbsolutaDestino);
                return JsonConvert.DeserializeObject<List<ObjetivosEntities>>(json);
            }
            return new List<ObjetivosEntities>();
        }
        public static ObjetivosEntities GuardarObjetivoEnJson(ObjetivosEntities data)
        {
            var listado = LeerObjetivosEnJson();

            if (data.Id != 0)
            {
                listado.RemoveAll(b => b.Id == data.Id);
            }
            else
            {
                int nuevoId = listado.Any() ? listado.Max(b => b.Id) + 1 : 1;
                data.Id = nuevoId;
            }
            listado.Add(data);
            string directorioDestino = "../ControladorData/DataBase";
            string rutaCompleta = Path.Combine(directorioDestino, "Objetivos.json");
            string rutaAbsolutaDestino = Path.GetFullPath(rutaCompleta);

            Directory.CreateDirectory(Path.GetDirectoryName(rutaAbsolutaDestino));
            string json = JsonConvert.SerializeObject(listado, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(rutaAbsolutaDestino, json);
            return data;
        }
        public static bool EliminarObjetivo(int id)
        {
            var objetivos = LeerObjetivosEnJson();
            var objetivo = objetivos.FirstOrDefault(b => b.Id == id);
            if (objetivo != null)
            {
                objetivos.Remove(objetivo);
                string directorioDestino = "../ControladorData/DataBase";
                string rutaCompleta = Path.Combine(directorioDestino, "Objetivos.json");
                string rutaAbsolutaDestino = Path.GetFullPath(rutaCompleta);
                string json = JsonConvert.SerializeObject(objetivos, Formatting.Indented);
                File.WriteAllText(rutaAbsolutaDestino, json);
                return true;
            }
            return false;
        }
        public static List<MovimientosEntities> LeerMovimientosEnJson()
        {
            string directorioDestino = "../ControladorData/DataBase";
            string rutaCompleta = Path.Combine(directorioDestino, "Movimientos.json");
            string rutaAbsolutaDestino = Path.GetFullPath(rutaCompleta);

            if (File.Exists(rutaAbsolutaDestino))
            {
                string json = File.ReadAllText(rutaAbsolutaDestino);
                return JsonConvert.DeserializeObject<List<MovimientosEntities>>(json);
            }
            return new List<MovimientosEntities>();
        }
        public static MovimientosEntities GuardarMovimientoEnJson(MovimientosEntities data)
        {
            var listado = LeerMovimientosEnJson();

            if (data.Id != 0)
            {
                listado.RemoveAll(b => b.Id == data.Id);
            }
            else
            {
                int nuevoId = listado.Any() ? listado.Max(b => b.Id) + 1 : 1;
                data.Id = nuevoId;
            }
            listado.Add(data);
            string directorioDestino = "../ControladorData/DataBase";
            string rutaCompleta = Path.Combine(directorioDestino, "Movimientos.json");
            string rutaAbsolutaDestino = Path.GetFullPath(rutaCompleta);

            Directory.CreateDirectory(Path.GetDirectoryName(rutaAbsolutaDestino));
            string json = JsonConvert.SerializeObject(listado, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(rutaAbsolutaDestino, json);
            return data;
        }
        public static bool EliminarMovimiento(int id)
        {
            var movimientos = LeerMovimientosEnJson();
            var movimiento = movimientos.FirstOrDefault(b => b.Id == id);
            if (movimiento != null)
            {
                movimientos.Remove(movimiento);
                string directorioDestino = "../ControladorData/DataBase";
                string rutaCompleta = Path.Combine(directorioDestino, "Movimientos.json");
                string rutaAbsolutaDestino = Path.GetFullPath(rutaCompleta);
                string json = JsonConvert.SerializeObject(movimientos, Formatting.Indented);
                File.WriteAllText(rutaAbsolutaDestino, json);
                return true;
            }
            return false;
        }
    }
}
