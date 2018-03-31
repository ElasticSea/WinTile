using System;
using System.Collections.Generic;
using System.Windows;
using ElasticSea.Wintile.Model.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ElasticSea.Wintile.Model.Managers
{
    public class LayoutManager
    {
        private readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter> {new StringEnumConverter()},
            Formatting = Formatting.Indented
        };

        public Layout Layout { get; private set; }

        public string Json
        {
            get => JsonConvert.SerializeObject(Layout, settings);
            set
            {
                try
                {
                    Layout = JsonConvert.DeserializeObject<Layout>(value) ?? new Layout();
                }
                catch (Exception e)
                {
                    Json = "{}";
                    MessageBox.Show("User Profile is corrupted: " + e.Message, "Corrupted Data",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
        }
    }
}