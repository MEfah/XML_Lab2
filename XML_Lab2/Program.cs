using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

XmlDocument xDoc = new();
xDoc.PreserveWhitespace = true;
xDoc.Load("Schedule.xml");
XmlElement? el = xDoc.DocumentElement;


if (el == null)
{
    Console.WriteLine("Возникла ошибка при прочтении XML с расписанием. Не удалось обнаружить корневой элемент");
    return;
}

string aud = "510[3]";
string resultPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
string htmlPath = resultPath;
resultPath = Path.Join(resultPath, "result.txt");
htmlPath = Path.Join(htmlPath, "result.html");

using (StreamWriter sw = new(resultPath, false, Encoding.UTF8))
{
    sw.WriteLine("1. Все занятия на неделе:");
    sw.WriteLine(GetXPathNodeResult(el, ".//lesson"));

    sw.WriteLine("2. Все аудитории, в которых проходят занятия:");
    sw.WriteLine(GetXPathNodeResult(el, ".//lesson/@aud"));

    sw.WriteLine("3. Все практические занятия:");
    sw.WriteLine(GetXPathNodeResult(el, ".//lesson[@type=\"seminar\"]"));

    sw.WriteLine($"4. Все лекции в аудитории {aud}:");
    sw.WriteLine(GetXPathNodeResult(el, ".//lesson[@type=\"seminar\" and @aud=\"510[3]\"]"));

    sw.WriteLine($"5. Все преподаватели, проводящие лекции в аудитории {aud}:");
    sw.WriteLine(GetXPathNodeResult(el, ".//lesson[@type=\"seminar\" and @aud=\"510[3]\"]/@lecturer"));

    sw.WriteLine("6. Последнее занятие каждого дня недели:");
    sw.WriteLine(GetXPathNodeResult(el, ".//lesson[position() = last()]"));

    sw.WriteLine("7. Общее количество всех занятий:");
    sw.WriteLine(GetXPathResult(el, "count(.//lesson)"));


    sw.WriteLine("\n\n\nXSLT XML в TXT:");
    XslCompiledTransform xslt = new XslCompiledTransform();
    xslt.Load(XmlReader.Create("XMLtoTXT.xsl", new XmlReaderSettings() { DtdProcessing = DtdProcessing.Ignore }));
    xslt.Transform(xDoc, null, XmlWriter.Create(sw, xslt.OutputSettings));


    sw.WriteLine("\n\n\nXSLT XML в HTML:");
    xslt = new XslCompiledTransform();
    xslt.Load(XmlReader.Create("XMLtoHTML.xsl", new XmlReaderSettings() { DtdProcessing = DtdProcessing.Ignore }));
    xslt.Transform(xDoc, null, XmlWriter.Create(sw, xslt.OutputSettings));

    xslt.Transform(xDoc, null, XmlWriter.Create(htmlPath, xslt.OutputSettings));
}




/*PrintXPathNodeResult(el, ".//lesson");
PrintXPathNodeResult(el, ".//lesson/@aud");
PrintXPathNodeResult(el, ".//lesson[@type=\"seminar\"]");
PrintXPathNodeResult(el, ".//lesson[@type=\"seminar\" and @aud=\"510[3]\"]");
PrintXPathNodeResult(el, ".//lesson[@type=\"seminar\" and @aud=\"510[3]\"]/@lecturer");
PrintXPathNodeResult(el, ".//lesson[position() = last()]");
PrintXPathResult(el, "count(.//lesson)");

XslCompiledTransform xslt = new XslCompiledTransform();
xslt.Load(XmlReader.Create("XMLtoTXT.xsl", new XmlReaderSettings() { DtdProcessing = DtdProcessing.Ignore }));
xslt.Transform(xDoc, null, XmlWriter.Create(Console.Out, xslt.OutputSettings));

xslt = new XslCompiledTransform();
xslt.Load(XmlReader.Create("XMLtoHTML.xsl", new XmlReaderSettings() { DtdProcessing = DtdProcessing.Ignore }));
xslt.Transform(xDoc, null, XmlWriter.Create("C:\\Users\\Марк\\Desktop\\test.html", xslt.OutputSettings));*/



#region functions

string GetXPathNodeResult(XmlElement element, string xpath)
{
    string result = string.Empty;
    XmlNodeList? xPathResult = element.SelectNodes(xpath);

    if (xPathResult is not null)
    {
        foreach (XmlNode el in xPathResult)
        {
            result += el.OuterXml + "\n";
        }
    }

    return result;
}

string? GetXPathResult(XmlElement element, string xpath)
{
    XPathNavigator? nav = element.CreateNavigator();

    if (nav is null)
        return null;

    XPathExpression expression = nav.Compile(xpath);
    return nav.Evaluate(expression).ToString();
}

#endregion