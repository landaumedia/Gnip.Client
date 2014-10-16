﻿using System.IO;
using System.Linq;
using System.Xml.Linq;

using Krowiorsch.Model;
using Krowiorsch.Model.Gnip.Facebook;
using Krowiorsch.Model.Gnip.Twitter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Krowiorsch.Converter
{
    /// <summary> Convertiert Gnipdaten in Activities </summary>
    public class GnipActivityConvert
    {
        public static Activity FromJson(string jsonString)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new GnipContractResolver(), 
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };

            return FromJson(jsonString, settings);
        }

        /// <summary> detects, if there is an activity in the string </summary>
        public static bool IsActivity(string jsonString)
        {
            var o = JObject.Parse(jsonString);
            return o["objectType"] != null;
        }

        public static Activity FromXml(string xmlString)
        {
            var document = XDocument.Load(new StringReader(xmlString));
            
            // remove namespaces to flatten attributes
            document.Descendants().Attributes().Where(a => a.IsNamespaceDeclaration).Remove();

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new GnipXmlContractResolver(),
            };

            return JsonConvert.DeserializeObject<FacebookActivity>(JsonConvert.SerializeXNode(document.Root, Formatting.None, true), settings);
        }


        static Activity FromJson(string jsonString, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<TwitterActivity>(jsonString, settings);
        }
    }
}