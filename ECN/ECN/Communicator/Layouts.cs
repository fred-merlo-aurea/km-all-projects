using System;
using System.Data;
using ecn.common.classes;

namespace ecn.communicator.classes {
    
    /// As we update the layout editor, we can add functionality to this class.
    
    public class Layouts : DatabaseAccessor {

        private const string TableOptionsValue = " cellpadding=0 cellspacing=0 ";
        private const int MinLength = 1;
        private const int EmptySlot = 0;

        public Layouts(int input_id):base(input_id) {
        }
        public Layouts(string input_id):base(input_id) {
        }
        public Layouts():base() {
        }


		
		/// Returns true if a layout uses the dynamic content engine. False Otherwise.
		
		/// <param name="LayoutID"> The Layout we want to check for dynamic content.</param>
		/// <returns>true or false depending on if it needs dynamic content</returns>
        //public bool hasDynamicContent() {
        //    string sqlQuery=
        //        " SELECT count(*) FROM ContentFilters "+
        //        " WHERE LayoutID="+ID();
        //    string count = DataFunctions.ExecuteScalar(sqlQuery).ToString();
        //    return count.Equals("0")?false:true;
        //}


		
		/// Returns a simple, non-dynamic preview of a particular layout.
		
		/// <param name="setLayoutID">The layout we want to create</param>
		/// <returns>HTML view for that layout</returns>
        public string GetHTMLPreview()
        {
            return GetPreview();
        }

        //WGH - For mobile support
        public string GetHTMLPreview(bool IsMobile)
        {
            return GetPreview(IsMobile);
        }

        private string GetPreview(bool? IsMobile = null)
        {
            var body = string.Empty;
            var templateSource = string.Empty;
            var tableOptions = string.Empty;
            var slot1 = EmptySlot;
            var slot2 = EmptySlot;
            var slot3 = EmptySlot;
            var slot4 = EmptySlot;
            var slot5 = EmptySlot;
            var slot6 = EmptySlot;
            var slot7 = EmptySlot;
            var slot8 = EmptySlot;
            var slot9 = EmptySlot;

            var sqlQuery = " select * from layout l, template t" +
                           $" where l.layoutid={ID()} " +
                           " and l.templateid=t.templateid and l.IsDeleted = 0 and t.IsDeleted = 0 ";

            var dataTable = DataFunctions.GetDataTable(sqlQuery);

            foreach (DataRow dataRow in dataTable.Rows)
            {
                templateSource = dataRow["TemplateSource"].ToString();
                tableOptions = dataRow["TableOptions"].ToString();
                slot1 = (int)dataRow["ContentSlot1"];
                slot2 = (int)dataRow["ContentSlot2"];
                slot3 = (int)dataRow["ContentSlot3"];
                slot4 = (int)dataRow["ContentSlot4"];
                slot5 = (int)dataRow["ContentSlot5"];
                slot6 = (int)dataRow["ContentSlot6"];
                slot7 = (int)dataRow["ContentSlot7"];
                slot8 = (int)dataRow["ContentSlot8"];
                slot9 = (int)dataRow["ContentSlot9"];
            }

            if (IsMobile.HasValue)
            {
                body = TemplateFunctions.EmailHTMLBody(templateSource, tableOptions, slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9, IsMobile.Value);
            }
            else
            {
                if (tableOptions.Length < MinLength)
                {
                    tableOptions = TableOptionsValue;
                }

                body = TemplateFunctions.EmailHTMLBody(templateSource, tableOptions, slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9);
            }

            return body;
        }

		
		/// Creates a static Text View for a layout
		
		/// <param name="setLayoutID"> The layout we want to see</param>
		/// <returns>Text view of the layout</returns>
		public string GetTextPreview(){
			string body="";
			string TemplateText="";
			int Slot1=0;
			int Slot2=0;
			int Slot3=0;
			int Slot4=0;
			int Slot5=0;
			int Slot6=0;
			int Slot7=0;
			int Slot8=0;
			int Slot9=0;
			string sqlQuery=
				" select * from layout l, template t "+
				" where l.layoutid="+ID()+" "+
				" and l.templateid=t.templateid and l.IsDeleted = 0 and t.IsDeleted = 0 ";
			DataTable dt = DataFunctions.GetDataTable(sqlQuery);
			foreach ( DataRow dr in dt.Rows ) {
				TemplateText=dr["TemplateText"].ToString();
				Slot1=(int)dr["ContentSlot1"];
				Slot2=(int)dr["ContentSlot2"];
				Slot3=(int)dr["ContentSlot3"];
				Slot4=(int)dr["ContentSlot4"];
				Slot5=(int)dr["ContentSlot5"];
				Slot6=(int)dr["ContentSlot6"];
				Slot7=(int)dr["ContentSlot7"];
				Slot8=(int)dr["ContentSlot8"];
				Slot9=(int)dr["ContentSlot9"];
			}
			body=TemplateFunctions.EmailTextBody(TemplateText,Slot1,Slot2,Slot3,Slot4,Slot5,Slot6,Slot7,Slot8,Slot9);
			return body;
		}
		
    }
}
