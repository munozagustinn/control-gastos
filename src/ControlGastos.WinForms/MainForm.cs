using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ControlGastos.Backend.Models;
using ControlGastos.Backend.Services;

namespace ControlGastos.WinForms
{
    public class MainForm : Form
    {
        private readonly ServicioGastos servicio;
        private readonly DataGridView grilla;
        private readonly DateTimePicker fecha;
        private readonly TextBox descripcion;
        private readonly TextBox categoria;
        private readonly TextBox importe;
        private readonly Label total;

        public MainForm(ServicioGastos servicio)
        {
            this.servicio = servicio;
            Text = "Control de Gastos";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(760, 520);
            Size = new Size(960, 650);
            BackColor = Color.FromArgb(246, 248, 251);
            Font = new Font("Segoe UI", 9F);

            Label titulo = new Label
            {
                Text = "Control de Gastos",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 44, 62),
                AutoSize = true,
                Location = new Point(24, 18)
            };
            Label subtitulo = new Label
            {
                Text = "Registrá tus gastos y consultá cuánto llevás acumulado.",
                ForeColor = Color.FromArgb(100, 112, 128),
                AutoSize = true,
                Location = new Point(27, 58)
            };
            Controls.Add(titulo);
            Controls.Add(subtitulo);

            Panel formulario = new Panel
            {
                Location = new Point(24, 96),
                Size = new Size(896, 108),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.White
            };
            Controls.Add(formulario);

            formulario.Controls.Add(CrearEtiqueta("Fecha", 16, 12));
            fecha = new DateTimePicker
            {
                Location = new Point(16, 37),
                Width = 145,
                Format = DateTimePickerFormat.Short
            };
            formulario.Controls.Add(fecha);

            formulario.Controls.Add(CrearEtiqueta("Descripción", 176, 12));
            descripcion = new TextBox { Location = new Point(176, 37), Width = 245, MaxLength = 100 };
            formulario.Controls.Add(descripcion);

            formulario.Controls.Add(CrearEtiqueta("Categoría", 436, 12));
            categoria = new TextBox { Location = new Point(436, 37), Width = 165, MaxLength = 50 };
            formulario.Controls.Add(categoria);

            formulario.Controls.Add(CrearEtiqueta("Importe", 616, 12));
            importe = new TextBox { Location = new Point(616, 37), Width = 112, MaxLength = 15 };
            formulario.Controls.Add(importe);

            Button agregar = CrearBoton("Agregar gasto", Color.FromArgb(36, 116, 89), 744, 35, 136);
            agregar.Click += AgregarGasto;
            formulario.Controls.Add(agregar);

            grilla = new DataGridView
            {
                Location = new Point(24, 222),
                Size = new Size(896, 310),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoGenerateColumns = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            grilla.EnableHeadersVisualStyles = false;
            grilla.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(233, 238, 244);
            grilla.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(48, 61, 78);
            grilla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grilla.RowTemplate.Height = 34;
            grilla.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Fecha",
                HeaderText = "Fecha",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "d" },
                FillWeight = 18
            });
            grilla.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Descripcion",
                HeaderText = "Descripción",
                FillWeight = 34
            });
            grilla.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Categoria",
                HeaderText = "Categoría",
                FillWeight = 22
            });
            grilla.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Importe",
                HeaderText = "Importe",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "C2",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                },
                FillWeight = 18
            });
            Controls.Add(grilla);

            total = new Label
            {
                Location = new Point(24, 550),
                Size = new Size(500, 36),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 44, 62)
            };
            Controls.Add(total);

            Button eliminar = CrearBoton("Eliminar seleccionado", Color.FromArgb(174, 68, 68), 708, 548, 212);
            eliminar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            eliminar.Click += EliminarGasto;
            Controls.Add(eliminar);

            AcceptButton = agregar;
            Shown += delegate { ActualizarGrilla(); };
        }

        private static Label CrearEtiqueta(string texto, int x, int y)
        {
            return new Label
            {
                Text = texto,
                Location = new Point(x, y),
                AutoSize = true,
                ForeColor = Color.FromArgb(77, 89, 105)
            };
        }

        private static Button CrearBoton(string texto, Color color, int x, int y, int ancho)
        {
            return new Button
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(ancho, 34),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
        }

        private void AgregarGasto(object sender, EventArgs e)
        {
            decimal valor;
            if (!decimal.TryParse(importe.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out valor)
                && !decimal.TryParse(importe.Text, NumberStyles.Currency, CultureInfo.GetCultureInfo("es-AR"), out valor))
            {
                MessageBox.Show("Ingresá un importe válido.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                importe.Focus();
                return;
            }

            try
            {
                servicio.Registrar(fecha.Value, descripcion.Text, categoria.Text, valor);
                descripcion.Clear();
                categoria.Clear();
                importe.Clear();
                descripcion.Focus();
                ActualizarGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EliminarGasto(object sender, EventArgs e)
        {
            Gasto seleccionado = grilla.CurrentRow == null ? null : grilla.CurrentRow.DataBoundItem as Gasto;
            if (seleccionado == null)
            {
                MessageBox.Show("Seleccioná un gasto de la lista.", "Sin selección", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Querés eliminar el gasto seleccionado?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                servicio.Eliminar(seleccionado.Id);
                ActualizarGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ActualizarGrilla()
        {
            var gastos = servicio.ObtenerTodos();
            grilla.DataSource = null;
            grilla.DataSource = gastos.ToList();
            total.Text = "Total acumulado: " + servicio.CalcularTotal().ToString("C2", CultureInfo.CurrentCulture);
        }
    }
}
