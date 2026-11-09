using System;
using System.Collections.Generic;
using ControlGastos.Backend.Models;

namespace ControlGastos.Backend.Persistence
{
    public interface IRepositorioGastos
    {
        IList<Gasto> ObtenerTodos();
        void GuardarTodos(IList<Gasto> gastos);
    }
}
