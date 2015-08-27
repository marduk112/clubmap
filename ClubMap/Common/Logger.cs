using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using ClubMap.DbModels;
using ClubMap.DbModels.Sqlite;
using ClubMap.Extensions;
using ClubMap.Models;

namespace ClubMap.Common
{
    public class Logger
    {
        public static bool Log(object who, Exception e)
        {
            return e != null && Log(who, e.ToDetailedString());
        }

        public static bool Log(object who, string message)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    string unk = "Unknown";
                    var whoStr = unk;
                    var browserName = unk;

                    if (who != null)
                    {
                        whoStr = who.GetType().ToString();

                        if (who is ControllerBase)
                        {
                            browserName =
                                ((ControllerBase)who).ControllerContext.HttpContext.Request
                                .Browser.Browser;
                            browserName += ";" +
                                ((ControllerBase)who).ControllerContext.HttpContext.Request
                                .Browser.Version;
                        }
                    }

                    db.Logs.Add(new Log()
                    {
                        Content = "[" + whoStr + ";" + browserName + "] " + message,
                        Created = DateTime.Now
                    });
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static XmlDocument GetLogsXML(int skip = 0, int? pageSize = null)
        {
            XmlDocument xml = new XmlDocument();

            var root = xml.CreateElement("logs");
            xml.AppendChild(root);

            try
            {
                using (var db = new ApplicationDbContext())
                {
                    foreach (var log in db.Logs.OrderByDescending(x => x.Created).Skip(skip).Take(pageSize ?? db.Logs.Count()))
                    {
                        var el = (XmlElement)xml.DocumentElement.AppendChild(xml.CreateElement("log"));
                        el.SetAttribute("id", log.Id.ToString());
                        el.SetAttribute("created", log.Created.ToString());
                        el.InnerText = log.Content;
                    }
                }

                return xml;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool DropToFile(string filePath = "")
        {
            if (!string.IsNullOrWhiteSpace(filePath))
                if (!filePath.EndsWith(@"\"))
                    filePath += @"\";

            string fileName = filePath + "logs.xml";

            if (File.Exists(fileName))
                File.Delete(fileName);

            var xml = GetLogsXML();

            if (xml != null)
            {
                xml.Save(fileName);
                return true;
            }

            return false;
        }
    }
}