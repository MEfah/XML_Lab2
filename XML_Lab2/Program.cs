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

PrintXPathNodeResult(el, ".//lesson");
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
xslt.Transform(xDoc, null, XmlWriter.Create("C:\\Users\\Марк\\Desktop\\test.html", xslt.OutputSettings));

#region functions

void PrintXPathNodeResult(XmlElement element, string xpath)
{
    XmlNodeList? xPathResult = element.SelectNodes(xpath);

    if (xPathResult is not null)
    {
        foreach (XmlNode el in xPathResult)
        {
            Console.WriteLine(el.OuterXml);
        }
    }
    Console.WriteLine();
}

void PrintXPathResult(XmlElement element, string xpath)
{
    XPathNavigator? nav = element.CreateNavigator();

    if (nav is null)
        return;

    XPathExpression expression = nav.Compile(xpath);
    Console.WriteLine(nav.Evaluate(expression));
}

#endregion