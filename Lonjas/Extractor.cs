using AngleSharp.Parser.Html;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Nest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace Lonjas
{
    class Extractor
    {
        ElasticClient elasticClient = Broker.EsClient();
        public void Albacete()
        {
            string contenido = null; ;
            try
            {
                WebRequest req = HttpWebRequest.Create("http://www.itap.es/inicio/lonja/hist%C3%B3rico-de-precios/");
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    contenido = reader.ReadToEnd();
                }

            }
            catch (Exception ex)
            {
            }

            var parser = new HtmlParser();
            var document = parser.Parse(contenido);

            var semanas = document.GetElementsByTagName("h3");
            var tablas = document.GetElementsByClassName("datos");

            List<string> fechas = new List<string>();
            List<int> num_semana = new List<int>();
            string fecha = null;
            int contador = 0;
            DateTime ultAlb = ultimoAlb();
            foreach (var semana in semanas)
            {
                fecha = semana.TextContent;
                string[] partes = fecha.Split('.');
                string[] partes_fecha = partes[0].Split(' ');
                string[] partes_semana = partes[1].Trim(' ').Split(' ');
                int n_semana = System.Convert.ToInt16(partes_semana[1]);
                string[] comp_fecha = partes_fecha[3].Split('/');
                fecha = comp_fecha[0] + "-" + comp_fecha[1] + "-" + comp_fecha[2];
                fechas.Add(fecha);
                num_semana.Add(n_semana);

            }
            foreach (var tabla in tablas)
            {
                var filas = tabla.GetElementsByTagName("tr");
                foreach (var fila in filas)
                {
                    var celdas = fila.GetElementsByTagName("td");
                    if (celdas[1].TextContent.ToLower().Contains("trigo") && celdas[2].TextContent.ToLower().Contains("panificables"))
                    {

                        string codigo = "TB";
                        CreaPrecioAlbaceteCereal(codigo, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("trigo") && celdas[2].TextContent.ToLower().Contains("duro grupo 1"))
                    {

                        string codigo = "TD";
                        CreaPrecioAlbaceteCereal(codigo, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("maíz"))
                    {

                        string codigo = "MZ";
                        CreaPrecioAlbaceteCereal(codigo, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("leche") && celdas[1].TextContent.ToLower().Contains("cabra"))
                    {

                        string codigo = "LC";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("leche") && celdas[1].TextContent.ToLower().Contains("oveja") && celdas[1].TextContent.ToLower().Contains("con"))
                    {

                        string codigo = "LOCDO";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("leche") && celdas[1].TextContent.ToLower().Contains("oveja") && celdas[1].TextContent.ToLower().Contains("sin"))
                    {

                        string codigo = "LOSDO";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("cabrito") && celdas[2].TextContent.ToLower().Contains("basto"))
                    {

                        string codigo = "C7B10";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("cabrito") && celdas[2].TextContent.ToLower().Contains("fino"))
                    {

                        string codigo = "C7F9";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("cordero") && celdas[1].TextContent.ToLower().Contains("con") && celdas[2].TextContent.ToLower().Contains("10.5"))
                    {

                        string codigo = "CM10CI15";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("cordero") && celdas[1].TextContent.ToLower().Contains("con") && celdas[2].TextContent.ToLower().Contains("15.1"))
                    {

                        string codigo = "CM15CI19";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("cordero") && celdas[1].TextContent.ToLower().Contains("con") && celdas[2].TextContent.ToLower().Contains("19.1"))
                    {

                        string codigo = "CM19CI23";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("cordero") && celdas[1].TextContent.ToLower().Contains("con") && celdas[2].TextContent.ToLower().Contains("23.1"))
                    {

                        string codigo = "CM23CI25";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("cordero") && celdas[1].TextContent.ToLower().Contains("con") && celdas[2].TextContent.ToLower().Contains("25.5"))
                    {

                        string codigo = "CM25CI28";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("cordero") && celdas[1].TextContent.ToLower().Contains("con") && celdas[2].TextContent.ToLower().Contains("28.1"))
                    {
                        string codigo = "CM28CI34";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("cordero") && celdas[1].TextContent.ToLower().Contains("con") && celdas[2].TextContent.ToLower().Contains("media"))
                    {

                        string codigo = "CMCIM10";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("cordero") && celdas[1].TextContent.ToLower().Contains("sin") && celdas[2].TextContent.ToLower().Contains("10.5"))
                    {

                        string codigo = "CSI10YO15";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("cordero") && celdas[1].TextContent.ToLower().Contains("sin") && celdas[2].TextContent.ToLower().Contains("15.1"))
                    {

                        string codigo = "CSI15YO19";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("cordero") && celdas[1].TextContent.ToLower().Contains("sin") && celdas[2].TextContent.ToLower().Contains("19.1"))
                    {

                        string codigo = "CSI19YO23";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("cordero") && celdas[1].TextContent.ToLower().Contains("sin") && celdas[2].TextContent.ToLower().Contains("23.1"))
                    {

                        string codigo = "CSI23YO25";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("cordero") && celdas[1].TextContent.ToLower().Contains("sin") && celdas[2].TextContent.ToLower().Contains("25.5"))
                    {

                        string codigo = "CSI25YO28";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("cordero") && celdas[1].TextContent.ToLower().Contains("sin") && celdas[2].TextContent.ToLower().Contains("28.1"))
                    {

                        string codigo = "CSI28YO34";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }
                    if (celdas[1].TextContent.ToLower().Contains("cordero") && celdas[1].TextContent.ToLower().Contains("sin") && celdas[2].TextContent.ToLower().Contains("media"))
                    {

                        string codigo = "CSIYOM10";
                        CreaPrecioAlbacete(codigo, celdas[3].TextContent, celdas[4].TextContent, celdas[5].TextContent.Trim(' '), fechas[contador], num_semana[contador], ultAlb);

                    }

                }
                contador++;
            }
        }
        public void CreaPrecioAlbacete(string code, string min, string max, string medida, string fecha, int semana, DateTime date)
        {
            string[] datos = fecha.Split('-');
            Precio Pmin = new Precio();
            Pmin.codigo = code;
            Pmin.precio = double.Parse(min);
            Pmin.dia = System.Convert.ToInt16(datos[0]);
            Pmin.semana = semana;
            Pmin.numMes = System.Convert.ToInt16(datos[1]);
            Pmin.nomMes = ConvierteMesInv(datos[1]);
            Pmin.año = System.Convert.ToInt16(datos[2]);
            Pmin.fecha = fecha;
            Pmin.tipoPrecio = "min";
            Pmin.fuente = "Albacete";
            Pmin.medida = medida;
            Precio Pmax = new Precio();
            Pmax.codigo = code;
            Pmax.precio = double.Parse(max);
            Pmax.dia = System.Convert.ToInt16(datos[0]);
            Pmax.semana = semana;
            Pmax.numMes = System.Convert.ToInt16(datos[1]);
            Pmax.nomMes = ConvierteMesInv(datos[1]);
            Pmax.año = System.Convert.ToInt16(datos[2]);
            Pmax.fecha = fecha;
            Pmax.tipoPrecio = "max";
            Pmax.fuente = "Albacete";
            Pmax.medida = medida;
            GuardaPrecioAlbacete(Pmin, date);
            GuardaPrecioAlbacete(Pmax, date);

        }
        public void CreaPrecioAlbaceteCereal(string code, string max, string medida, string fecha, int semana, DateTime date)
        {
            string[] datos = fecha.Split('-');
            Precio Pmax = new Precio();
            Pmax.codigo = code;
            Pmax.precio = double.Parse(max);
            Pmax.dia = System.Convert.ToInt16(datos[0]);
            Pmax.semana = semana;
            Pmax.numMes = System.Convert.ToInt16(datos[1]);
            Pmax.nomMes = ConvierteMesInv(datos[1]);
            Pmax.año = System.Convert.ToInt16(datos[2]);
            Pmax.fecha = fecha;
            Pmax.tipoPrecio = "max";
            Pmax.fuente = "Albacete";
            Pmax.medida = medida;
            GuardaPrecioAlbacete(Pmax, date);

        }
        public void GuardaPrecioAlbacete(Precio p, DateTime date2)
        {
            DateTime date1 = new DateTime(p.año, p.numMes, p.dia);
            if (date1 > date2)
            {
                elasticClient.Index(p, es => es
                                       .Index("agroesi")
                                       .Type("precio")
                          );
            }
        }
        public DateTime ultimoAlb()
        {
            DateTime date = new DateTime(2000, 1, 1);
            var ultimoP = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                        .Type("precio")
                                                        .Size(1)
                                                        .Sort(ss => ss.Descending(pre => pre.fecha))
                                                        .Query(q => q.Term(pre => pre.fuente, "Albacete".ToLower())));

            foreach (var hit in ultimoP.Hits)
            {
                date = new DateTime(hit.Source.año, hit.Source.numMes, hit.Source.dia);
            }

            return date;

        }
        public DateTime ultimoTal()
        {
            DateTime date = new DateTime(2000, 1, 1);
            var ultimoP = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                        .Type("precio")
                                                        .Size(1)
                                                        .Sort(ss => ss.Descending(pre => pre.fecha))
                                                        .Query(q => q.Term(pre => pre.fuente, "Talavera".ToLower())));

            foreach (var hit in ultimoP.Hits)
            {
                date = new DateTime(hit.Source.año, hit.Source.numMes, hit.Source.dia);
            }

            return date;

        }
        public void DatosVac()
        {
            if (Directory.GetFiles("C:/Users/Miguel Angel/Documents/Datos/Lonjas/LonjaTalavera/Servicio", "*.pdf").Length > 0)
            {
                foreach (string file in Directory.EnumerateFiles("C:/Users/Miguel Angel/Documents/Datos/Lonjas/LonjaTalavera/Servicio", "*.pdf"))
                {
                    StringBuilder text = null;
                    PdfReader pdfReader = null;

                    text = new StringBuilder();
                    pdfReader = new PdfReader(file);
                    int contador = 0;
                    for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                    {
                        if (contador == 0)
                        {
                            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                            string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                            currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                            text.Append(currentText);
                        }
                        contador++;

                    }
                    pdfReader.Close();
                    string tabla = text.ToString();
                    string[] filas = tabla.Split('\n');

                    for (int i = 0; i < filas.Length; i++)
                    {
                        if (filas[i].ToLower().Contains("ternero") && filas[i].ToLower().Contains("cruzado") && filas[i].ToLower().Contains("base"))
                        {
                            if (filas[i].ToLower().Contains("1ª"))
                            {
                                string codigo1 = "TO200C1";
                                string[] partes = filas[i].Split(' ');
                                PreciosVac(partes, codigo1, file);

                            }
                            else
                            {
                                string codigo2 = "TO200C2";
                                string[] partes = filas[i].Split(' ');
                                PreciosVac(partes, codigo2, file);
                            }


                        }
                        if (filas[i].ToLower().Contains("ternera") && filas[i].ToLower().Contains("cruzada") && filas[i].ToLower().Contains("base"))
                        {
                            if (filas[i].ToLower().Contains("1ª"))
                            {
                                string codigo1 = "TA200C1";
                                string[] partes = filas[i].Split(' ');
                                PreciosVac(partes, codigo1, file);
                            }
                            else
                            {
                                string codigo2 = "TA200C2";
                                string[] partes = filas[i].Split(' ');
                                PreciosVac(partes, codigo2, file);
                            }

                        }
                        if (filas[i].ToLower().Contains("ternero") && filas[i].ToLower().Contains("pais") && filas[i].ToLower().Contains("base"))
                        {
                            string codigo = "TO200PA";
                            string[] partes = filas[i].Split(' ');
                            PreciosVac(partes, codigo, file);

                        }
                        if (filas[i].ToLower().Contains("ternera") && filas[i].ToLower().Contains("pais") && filas[i].ToLower().Contains("base"))
                        {
                            string[] partes = filas[i].Split(' ');
                            string codigo = "TA200PA";
                            PreciosVac(partes, codigo, file);
                        }
                    }
                }

            }

        }
        public void PreciosVac(string[] partes, string code, string file)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            string[] partes_nombre = file.Split('\\');
            string[] nombre = partes_nombre[1].Split('.')[0].Split('_');

            int año = System.Convert.ToInt16(nombre[2]);
            int num_mes = System.Convert.ToInt16(ConvierteMes(nombre[1]));
            string nom_mes = nombre[1];
            int dia = System.Convert.ToInt16(nombre[0]);
            int semana = 0;
            string fecha = null;
            if (dia < 10)
            {
                fecha = "0" + dia + "-" + CompruebaMes(num_mes) + "-" + año;
            }
            else
            {
                fecha = dia + "-" + CompruebaMes(num_mes) + "-" + año;
            }

            DateTime date = new DateTime(año, num_mes, dia);
            semana = System.Convert.ToInt16(cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek).ToString());

            string medida = "Euros/kg";
            string tipo_precio = "med";
            Precio p = new Precio();
            p.codigo = code;
            p.precio = double.Parse(partes[7]);
            p.dia = dia;
            p.semana = semana;
            p.numMes = num_mes;
            p.nomMes = nom_mes;
            p.año = año;
            p.fecha = fecha;
            p.tipoPrecio = tipo_precio;
            p.fuente = "Talavera";
            p.medida = medida;
            GuardaPrecioVac(p, ultimoTal());

        }
        public void GuardaPrecioVac(Precio p, DateTime date)
        {
            DateTime dateP = new DateTime(p.año, p.numMes, p.dia);
            if (dateP > date)
            {
                elasticClient.Index(p, es => es
                                   .Index("agroesi")
                                   .Type("precio")
                      );
            }


        }
        public string ConvierteMes(string dato)
        {
            string mes = null;
            switch (dato)
            {
                case "enero":
                    mes = "01";
                    break;
                case "febrero":
                    mes = "02";
                    break;
                case "marzo":
                    mes = "03";
                    break;
                case "abril":
                    mes = "04";
                    break;
                case "mayo":
                    mes = "05";
                    break;
                case "junio":
                    mes = "06";
                    break;
                case "julio":
                    mes = "07";
                    break;
                case "agosto":
                    mes = "08";
                    break;
                case "septiembre":
                    mes = "09";
                    break;
                case "octubre":
                    mes = "10";
                    break;
                case "noviembre":
                    mes = "11";
                    break;
                case "diciembre":
                    mes = "12";
                    break;
            }


            return mes;

        }
        public string CompruebaMes(int mes)
        {
            string month = null;
            if (mes < 10)
            {
                month = "0" + mes;
            }
            else
            {
                month = string.Concat(mes);
            }
            return month;
        }
        public string ConvierteMesInv(string dato)
        {
            string mes = null;
            switch (dato)
            {
                case "01":
                    mes = "enero";
                    break;
                case "02":
                    mes = "febrero";
                    break;
                case "03":
                    mes = "marzo";
                    break;
                case "04":
                    mes = "abril";
                    break;
                case "05":
                    mes = "mayo";
                    break;
                case "06":
                    mes = "junio";
                    break;
                case "07":
                    mes = "julio";
                    break;
                case "08":
                    mes = "agosto";
                    break;
                case "09":
                    mes = "septiembre";
                    break;
                case "10":
                    mes = "octubre";
                    break;
                case "11":
                    mes = "noviembre";
                    break;
                case "12":
                    mes = "diciembre";
                    break;
            }


            return mes;

        }
    }
}
