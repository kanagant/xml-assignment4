using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;



/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Program
    {
        public static string xmlURL = "https://kanagant.github.io/xml-assignment4/Hotels.xml";
        public static string xmlErrorURL = "https://kanagant.github.io/xml-assignment4/HotelsErrors.xml";
        public static string xsdURL = "https://kanagant.github.io/xml-assignment4/Hotels.xsd";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);


            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);


            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                var schemaSet = new XmlSchemaSet();
                schemaSet.Add(null, xsdUrl);

                var errors = new List<string>();

                var settings = new XmlReaderSettings
                {
                    Schemas = schemaSet,
                    ValidationType = ValidationType.Schema
                };

                settings.ValidationEventHandler += (s, e) =>
                {
                    errors.Add($"Line {e.Exception.LineNumber}, Position {e.Exception.LinePosition}: {e.Message}");
                };

                using (var reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read()) { } // force validation
                }

                return errors.Count == 0 ? "No Error" : string.Join("\n", errors);
            }
            catch (Exception ex)
            {
                return "Validation Failed: " + ex.Message;
            }

            //return "No Error" if XML is valid. Otherwise, return the desired exception message.
        }

        public static string Xml2Json(string xmlUrl)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                var doc = new XmlDocument();
                doc.Load(xmlUrl); // loads from your hosted URL
                string jsonText = JsonConvert.SerializeXmlNode(doc);

                // The returned jsonText needs to be de-serializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))
                return jsonText;
            }
            catch (Exception ex)
            {
                return "Conversion Failed: " + ex.Message;
            }
            
        }
    }

}
