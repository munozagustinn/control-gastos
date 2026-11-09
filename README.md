# Control de Gastos

Aplicación de escritorio para registrar gastos personales, organizarlos por categoría y consultar el total acumulado. La información se conserva en un archivo JSON local.

## Requisitos

- Windows
- Visual Studio con soporte para desarrollo de escritorio .NET y el paquete de destino de .NET Framework 4.8

## Ejecutar

1. Abrí `ControlGastos.sln` en Visual Studio.
2. Seleccioná `ControlGastos.WinForms` como proyecto de inicio.
3. Ejecutá con **F5**.

## Uso

- Completá fecha, descripción, categoría e importe y elegí **Agregar gasto**.
- Seleccioná una fila y elegí **Eliminar seleccionado** para quitar un gasto.
- El total acumulado aparece debajo de la tabla.
- Los datos se guardan en `%LOCALAPPDATA%\ControlGastos\gastos.json`.

## Capas

- `ControlGastos.WinForms`: interfaz de usuario hecha con Windows Forms.
- `ControlGastos.Backend`: modelo, reglas de validación, servicio y persistencia JSON.

La solución apunta a .NET Framework 4.8 y mantiene la interfaz separada de las reglas y del repositorio de datos.

## Publicación

Repositorio GitHub: https://github.com/munozagustinn/control-gastos
