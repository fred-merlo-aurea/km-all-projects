using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMModels.Wrappers;

namespace KMModels.Controls
{
    public class Grid : HeadedControl
    {
        public Grid()
        {
            Rows = new List<string>();
            Columns = new List<string>();
        }

        public Grid(bool setDefaultValues = false) : this()
        {
            if (setDefaultValues)
            {
                Label = "Grid";
                LabelHTML = "Grid";
                Controls = GridControl.RadioButtons;
                Validation = GridValidation.NotRequired;
                Rows = new List<string> 
                           { 
                               "Row1",
                               "Row2"
                           };
                Columns = new List<string> 
                              { 
                                    "Column1",
                                    "Column2"
                              };
            }
        }

        public override ControlType Type
        {
            get { return ControlType.Grid; }
        }

        public GridControl Controls { get; set; }

        public GridValidation Validation { get; set; }

        public IEnumerable<string> Rows { get; set; }

        public IEnumerable<string> Columns { get; set; }

        public override void Fill(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        {
            base.Fill(control, properties);

            Controls = (GridControl)int.Parse(control.GetFormPropertyValue(gridcontrols_property, properties));
            Validation = (GridValidation)int.Parse(control.GetFormPropertyValue(gridvalidation_property, properties));

            List<KMEntities.FormControlPropertyGrid> columnsProperties = control.GetProperties(columns_property, properties).OrderBy(x => x.FormControlPropertyGrid_Seq_ID).ToList();
            List<KMEntities.FormControlPropertyGrid> rowsProperties = control.GetProperties(rows_property, properties).OrderBy(x => x.FormControlPropertyGrid_Seq_ID).ToList();
            List<string> columns = new List<string>();
            foreach (var p in columnsProperties)
            {
                columns.Add(p.Text());
            }
            List<string> rows = new List<string>();
            foreach (var p in rowsProperties)
            {
                rows.Add(p.Text());
            }
            Columns = columns;
            Rows = rows;
        }
    }
}
