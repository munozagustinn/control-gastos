using System;
using System.Collections.Generic;
using System.Linq;
using ControlGastos.Backend.Models;
using ControlGastos.Backend.Persistence;

namespace ControlGastos.Backend.Services
{
    public class ServicioGastos
    {
        private readonly IRepositorioGastos repositorio;

        public ServicioGastos(IRepositorioGastos repositorio)
        {
            this.repositorio = repositorio;
        }

        public IList<Gasto> ObtenerTodos()
        {
            return repositorio.ObtenerTodos()
                .OrderByDescending(gasto => gasto.Fecha)
                .ThenBy(gasto => gasto.Descripcion)
                .ToList();
        }

        public void Registrar(DateTime fecha, string descripcion, string categoria, decimal importe)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("Ingresá una descripción.", "descripcion");
            if (string.IsNullOrWhiteSpace(categoria))
                throw new ArgumentException("Ingresá una categoría.", "categoria");
            if (importe <= 0)
                throw new ArgumentException("El importe debe ser mayor que cero.", "importe");

            IList<Gasto> gastos = repositorio.ObtenerTodos();
            gastos.Add(new Gasto
            {
                Id = Guid.NewGuid(),
                Fecha = fecha.Date,
                Descripcion = descripcion.Trim(),
                Categoria = categoria.Trim(),
                Importe = importe
            });
            repositorio.GuardarTodos(gastos);
        }

        public void Eliminar(Guid id)
        {
            IList<Gasto> gastos = repositorio.ObtenerTodos();
            Gasto gasto = gastos.FirstOrDefault(item => item.Id == id);
            if (gasto == null)
                throw new InvalidOperationException("El gasto seleccionado ya no existe.");

            gastos.Remove(gasto);
            repositorio.GuardarTodos(gastos);
        }

        public decimal CalcularTotal()
        {
            return repositorio.ObtenerTodos().Sum(gasto => gasto.Importe);
        }
    }
}
