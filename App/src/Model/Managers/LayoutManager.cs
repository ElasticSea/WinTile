using System;
using System.Windows;
using Newtonsoft.Json;

namespace App.Model.Managers
{
    public class LayoutManager
    {
        public Layout Layout { get; private set; }

        public string Json
        {
            get => JsonConvert.SerializeObject(Layout, Formatting.Indented);
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