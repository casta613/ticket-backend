using ClosedXML.Excel;
using System.Reflection;

namespace APITicket.BLL
{
    public class GenerarDocumento
    {
        public IConfiguration configuration;
        private string? uid;
        public GenerarDocumento(IConfiguration configuration)
        {
            this.configuration = configuration;

        }

        public object GetExcel(dynamic requestHeader, dynamic request, ref int status)
        {
            object resp;
            try
            {

                XLWorkbook wb = new XLWorkbook();
                var worksheet = wb.Worksheets.Add("Hoja1");

                int row = 0;
                foreach (dynamic ob in requestHeader)
                {
                    row++;
                    worksheet.Cell(1, row).Value = ob.Value;

                }
                int fila = 1;
                foreach (dynamic ob in request)
                {
                    fila++;
                    row = 0;

                    
                    foreach (dynamic value in requestHeader)
                    {
                        row++;
                        string valor = value.Key;

                        Type type = ob.GetType();
                        PropertyInfo propertyInfo = type.GetProperty(valor);
                        string result = propertyInfo.GetValue(ob).ToString();

                       // ((IDictionary<string, dynamic>)ob).TryGetValue(valor, out var result);
                        if (result != null)
                        {
                            worksheet.Cell(fila, row).Value = result;
                        }
                        else
                        {
                            worksheet.Cell(fila, row).Value = "";
                        }

                    }

                }


                MemoryStream stream = new MemoryStream();
                wb.SaveAs(stream);

                string base64 = Convert.ToBase64String(stream.ToArray());
                resp = new { base64 = base64 };


            }
            catch (Exception ex)
            {
                status = 500;
                resp = new { mensage = "Ocurrió un error al momento de generar el EXCEL " };

            }

            return resp;
        }
    }
}
